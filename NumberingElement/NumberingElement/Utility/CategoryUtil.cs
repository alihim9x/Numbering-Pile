using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;

namespace Utility
{
    public static class CategoryUtil
    {
        private static RevitData revitData = RevitData.Instance;
        public static Autodesk.Revit.DB.Category GetBuiltinCategory(this Autodesk.Revit.DB.BuiltInCategory bic)
        {      
            return revitData.Categories.Single(x=>x.Id.IntegerValue == (int)bic);
        }
        public static Autodesk.Revit.DB.CategorySet GetCategorySet(this List<Autodesk.Revit.DB.BuiltInCategory> bics)
        {
            Autodesk.Revit.DB.CategorySet cateSet = new Autodesk.Revit.DB.CategorySet();
            foreach (var item in bics)
            {
                cateSet.Insert(item.GetBuiltinCategory());
            }
            return cateSet;
        }
        public static bool IsEqualCate(this List<Autodesk.Revit.DB.BuiltInCategory> bics, List<Autodesk.Revit.DB.BuiltInCategory> ortherBics)
        {
            return bics.Except(ortherBics).Count() == 0;
        }
        public static List<Autodesk.Revit.DB.BuiltInCategory> GetBuiltinCategories(this Autodesk.Revit.DB.CategorySet cateSet)
        {
            List<Autodesk.Revit.DB.BuiltInCategory> bics = new List<Autodesk.Revit.DB.BuiltInCategory>();
            foreach (Autodesk.Revit.DB.Category item in cateSet)
            {
                bics.Add((Autodesk.Revit.DB.BuiltInCategory)item.Id.IntegerValue);
            }
            return bics;
        }
    }
}
