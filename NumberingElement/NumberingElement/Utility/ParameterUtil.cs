using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;
using Utility;
using Autodesk.Revit.DB;
using System.IO;
using Model;
namespace Utility
{
    public static class ParameterUtil
    {
        
        private static RevitData revitData = RevitData.Instance;
        public static Model.ParameterValue AsValue(this Autodesk.Revit.DB.Element element, string param)
        {
            return element.LookupParameter(param).AsValue();
        }
        public static Model.ParameterValue AsValue(this Autodesk.Revit.DB.Parameter p)
        {
            string text = null;
            double number = 0;
            switch(p.StorageType)
            {
                case Autodesk.Revit.DB.StorageType.String:
                    text = p.AsString();
                    return new Model.ParameterValue { ParameterValueType = ParameterValueType.Text,ValueText = text};
                case Autodesk.Revit.DB.StorageType.ElementId:
                    text = revitData.Document.GetElement(p.AsElementId()).Name;
                    return new Model.ParameterValue { ParameterValueType = ParameterValueType.Text, ValueText = text };
                case Autodesk.Revit.DB.StorageType.Integer:
                    number = p.AsInteger();
                    return new Model.ParameterValue { ParameterValueType = ParameterValueType.Number, ValueNumber = number };
                case Autodesk.Revit.DB.StorageType.Double:
                    number = p.AsDouble().GetConvertValue(p.DisplayUnitType);
                    return new Model.ParameterValue { ParameterValueType = ParameterValueType.Number, ValueNumber = number };
            }
            throw new Exception("Đoạn code này không được đọc tới.");
        }
        public static void SetValue(this Autodesk.Revit.DB.Element e, string param, object obj)
        {
            e.LookupParameter(param).SetValue(obj);
        }
        public static void SetValue(this Autodesk.Revit.DB.Parameter p, object obj)
        {
            var tx = revitData.Transaction;
            if(tx == null)
            {
                throw new Exception("Chưa khởi tạo Transaction");
            }
            if(tx.GetStatus() != TransactionStatus.Started)
            {
                throw new Exception("Transaction chưa bắt đầu.");
            }
            if(p.IsReadOnly)
            {
                throw new Exception("Parameter không được phép điều chỉnh.");
            }
            if(p==null)
            {
                throw new Exception("Parameter không tồn tại");
            }
           
            var invalidTypeError = "Giá trị không đúng kiểu dữ liệu";
            switch (p.StorageType)
            {
                case StorageType.Double:
                    if (!(obj is Double)) throw new Exception(invalidTypeError);
                    p.Set((double)obj);
                    break;
                case StorageType.String:
                    if (!(obj is String)) throw new Exception(invalidTypeError);
                    p.Set((String)obj);
                    break;
                case StorageType.Integer:
                    if (!(obj is Int32)) throw new Exception(invalidTypeError);
                    p.Set((Int32)obj);
                    break;
                case StorageType.ElementId:
                    if (obj is Autodesk.Revit.DB.Element)
                    {
                        p.Set(((Autodesk.Revit.DB.Element)obj).Id);
                    }
                    else if (obj is Autodesk.Revit.DB.ElementId)
                    {
                        p.Set((Autodesk.Revit.DB.ElementId)obj);
                    }
                    else throw new Exception(invalidTypeError);
                    break;

            }
        }
        public static double GetConvertValue(this double value, Autodesk.Revit.DB.DisplayUnitType dut)
        {
            switch (dut)
            {
                case Autodesk.Revit.DB.DisplayUnitType.DUT_MILLIMETERS:
                case DisplayUnitType.DUT_METERS:
                    return UnitUtils.Convert(value,DisplayUnitType.DUT_DECIMAL_FEET,dut);
                case Autodesk.Revit.DB.DisplayUnitType.DUT_SQUARE_MILLIMETERS:
                    return UnitUtils.Convert(value, DisplayUnitType.DUT_SQUARE_FEET, dut);
                case Autodesk.Revit.DB.DisplayUnitType.DUT_CUBIC_MILLIMETERS:
                    return UnitUtils.Convert(value, DisplayUnitType.DUT_CUBIC_FEET, dut);
                case DisplayUnitType.DUT_SQUARE_METERS:
                    return UnitUtils.Convert(value, DisplayUnitType.DUT_SQUARE_FEET, dut);
                case DisplayUnitType.DUT_CUBIC_METERS:
                    return UnitUtils.Convert(value, DisplayUnitType.DUT_CUBIC_FEET, dut);
                case DisplayUnitType.DUT_DECIMAL_DEGREES:
                    return UnitUtils.Convert(value, DisplayUnitType.DUT_RADIANS, dut);
                default:
                    throw new Exception("Trường hợp này chưa xét tới:");

            }

            
        }
        public static CategorySet GetCategory(this Parameter p)
        {
            var pBinding = revitData.BindingMap;
            var bindings1 = pBinding.get_Item(p.Definition) as ElementBinding;
            return  bindings1?.Categories;
        }
        public static List<Category> ConvertList(this CategorySet cateSet)
        {
            var cates = new List<Category>();
            foreach (Category item in cateSet)
            {
                cates.Add(item);
            }
            return cates;
        }
        public static void CreateSharedParameterFile(string fileName)
        {
            if(!File.Exists(fileName))
            {
                using (var fs = File.Create(fileName)) { }
            }
            
            var app = revitData.Application;
            app.SharedParametersFilename = fileName;
        }
        public static Definition GetDefinition (string name, Model.DefinitionGroupType defGroupType, ParameterType paramType )
        {
            Definition def = null;
            DefinitionFile defFile = revitData.Application.OpenSharedParameterFile();
            if(defFile == null)
            {
                CreateSharedParameterFile(Constant.ConstantValue.SharedParameterFileName);
                defFile = revitData.Application.OpenSharedParameterFile();
            }
            DefinitionGroups defGroups = defFile.Groups;
            var defGroup = defGroups.get_Item(defGroupType.ToString());
            if(defGroup == null)
            {  defGroup = defGroups.Create(defGroupType.ToString()); }
            var defs = defGroup.Definitions;
            def = defs.get_Item(name);
            if(def == null)
                {
                def = defGroup.Definitions.Create(new ExternalDefinitionCreationOptions(name, paramType)
                {
                    UserModifiable = true
                });
            }
            return def;


        }
        public static void AddParameter(string name, Model.AddParameterType addParamType,Model.DefinitionGroupType defGroupType ,ParameterType paramType,
            BuiltInParameterGroup paramGroup, List<BuiltInCategory> bics)
        {
            var def = GetDefinition(name, defGroupType, paramType);
            var bindingMap = revitData.BindingMap;
            switch(addParamType)
            {
                case AddParameterType.Instance:
                    var insBinding = bindingMap.get_Item(def) as InstanceBinding;
                    
                    if(insBinding == null)
                    {
                        bindingMap.Insert(def, revitData.Application.Create.NewInstanceBinding(bics.GetCategorySet()), paramGroup);
                    }
                    else
                    {

                        var existcateSet = insBinding.Categories;
                        if(!existcateSet.GetBuiltinCategories().IsEqualCate(bics))
                        {
                            bindingMap.ReInsert(def, revitData.Application.Create.NewInstanceBinding(bics.GetCategorySet()));
                        }
                    }

                    break;
                case AddParameterType.Type:
                    var typeBinding = bindingMap.get_Item(def) as TypeBinding;
                    if (typeBinding == null)
                    {
                        bindingMap.Insert(def, revitData.Application.Create.NewTypeBinding(bics.GetCategorySet()), paramGroup);
                    }
                    else
                    {
                        var existcateSet = typeBinding.Categories;
                        if(!existcateSet.GetBuiltinCategories().IsEqualCate(bics))
                        {
                            bindingMap.ReInsert(def, revitData.Application.Create.NewTypeBinding(bics.GetCategorySet()));
                        }
                    }
                    break;
            }
            
        }
        public static Autodesk.Revit.DB.Parameter LookupParameter(this BuiltInCategory bic, string name, bool isElementType = false) // mặc định là false => lấy instance
        {

            //var inselem = revitData.InstanceElements.Where(x => x.Category.Id.IntegerValue == (int)bic).FirstOrDefault();
            //var typeelem = revitData.TypeElements.Where(x=>x.Category.Id.IntegerValue==(int)bic).FirstOrDefault();
            //Parameter insParam = null;
            //Parameter typeParam = null;
            //if(isElementType)
            //{
            //    return insParam = inselem.LookupParameter(name);
            //}
            //else
            //{
            //    return typeParam = typeelem.LookupParameter(name);
            //}

            var elems = isElementType ? revitData.TypeElements : revitData.InstanceElements; // nếu islementype true lấy trước :, false lấy sau :
            var elem = elems.Where(x => x.Category?.Id.IntegerValue == (int)bic).FirstOrDefault();
            if(elem == null)
            {
                throw new Model.Exception.ElementNotFoundException();
            }
            return elem.LookupParameter(name);
        }
    }
}
