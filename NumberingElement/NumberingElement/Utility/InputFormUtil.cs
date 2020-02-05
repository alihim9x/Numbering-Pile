using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using SingleData;
using Utility;

namespace Utility
{
    public static class InputFormUtil
    {
        private static RevitData revitData
        {
            get
            {
                return RevitData.Instance;
            }
        }
        private static ModelData modelData
        {
            get
            {
                return ModelData.Instance;
            }
        }
        private static FormData formData
        {
            get
            {
                return FormData.Instance;
            }
        }
        public static void PickElement()
        {

            var form = formData.InputForm;
            form.Hide();
            var setting = ModelData.Instance.Setting;
            var sel = revitData.Selection;
            double a = 1;
            var prefix = setting.PreFix;
            Func<Autodesk.Revit.DB.Element, bool> filter = x =>
            {
                var cate = x.Category.Id.IntegerValue;
                if (cate == setting.Category.Id.IntegerValue) return true;
                return false;
            };
            while (true)
            {
                try
                {
                    var elem = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, new SelectionUtil(filter, null)).GetRevitElement();
                    elem.SetValue("Comments", prefix + a.ToString());
                    a += 1;
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    form.ShowDialog();
                    break;
                }
            }
        }
        public static void SelectPathElement()
        {
            var form = formData.InputForm;
            form.Hide();
            var sel = revitData.Selection;
            var setting = ModelData.Instance.Setting;
            var parameterString = setting.Parameter.Definition.Name;
            var prefix = setting.PreFix;

            var settingCate = setting.Category.Id.IntegerValue;
            
            //var ettElem = new Model.Entity.Element();
            Func<Autodesk.Revit.DB.Element, bool> filterLine = x =>
            {
                var cate = x.Category.Id.IntegerValue;
                if (cate == (int)Autodesk.Revit.DB.BuiltInCategory.OST_Lines) return true;
                return false;
            };
            Func<Autodesk.Revit.DB.Element, bool> filterCate = x =>
             {
                 var cate = x.Category.Id.IntegerValue;
                 if (cate == settingCate) return true;
                 return false;
             };
            //var elemsRef = sel.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new SelectionUtil(filter, null)).ToList();
            //elemsRef.ForEach(x => setting.EttElements.Add( new Model.Entity.Element { RevitElement = x.GetRevitElement()}));
            //Autodesk.Revit.UI.TaskDialog.Show("Revit", setting.EttElements.Count().ToString());

            
            //var path = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, new SelectionUtil(filterLine, null),"Vui lòng chọn đường dẫn").GetRevitElement();
            var paths = sel.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new SelectionUtil(filterLine, null), "Vui lòng chọn đường dẫn").Select(x=>x.GetRevitElement()).ToList();
            //var piles = sel.PickElementsByRectangle(new SelectionUtil(filterCate, null), "Vui lòng chọn nhóm cọc").ToList();
            //setting.EttElement = new Model.Entity.Element { RevitElement = path};
            paths.ForEach(x => setting.EttElements.Add(new Model.Entity.Element { RevitElement = x }));
            //foreach (var item in paths)
            //{
            //    setting.EttElements.Add(new Model.Entity.Element { RevitElement = item });
            //}
            //piles.ForEach(x => setting.EttElement.EttPiles.Add(new Model.Entity.Pile { RevitElement = x}));
            var ettElems = setting.EttElements;
            int index = setting.StartNo - 1;
            foreach (var item in ettElems)
            {
                var ettPiles = item.EttPiles;                
                item.EttPiles.ForEach(x => x.HostEttElement = item);
                var ettPilesSorted1 = item.EttPiles.OrderBy(x => x.Value2Sort).ToList();
                for (int i = 0; i < ettPilesSorted1.Count; i++)
                {
                    index++;
                    ettPilesSorted1[i].RevitElement.SetValue(parameterString, prefix + index.ToString());
                }
            }
            //foreach (var item in ettPiles)
            //{
            //    item.HostEttElement = setting.EttElement;
            //}
            //var ettPilesSorted = ettPiles.OrderBy(x => x.Value2Sort).ToList();
            //for (int i = 0; i < ettPilesSorted.Count; i++)
            //{

            //    ettPilesSorted[i].RevitElement.SetValue("Comments", prefix + "." + (i+1).ToString());

            //}



            
        }
        public static void Run()
        {

        }


    }
}
