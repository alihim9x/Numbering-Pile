using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constant
{
    public class ConstantValue
    {
        public static string SharedParameterFileName { get; set; } = @"D:\Study\RV Api";
        public static Func<Autodesk.Revit.DB.Element, bool> FilterColumns { get; set; }
        = x => x.Category.Id.IntegerValue == (int)Autodesk.Revit.DB.BuiltInCategory.OST_StructuralColumns;
        public static Func<Autodesk.Revit.DB.Element, bool> FilterFramings { get; set; }
        = x => x.Category.Id.IntegerValue == (int)Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFraming;
        public static Func<Autodesk.Revit.DB.Element, bool> FilterWalls { get; set; }
        = x => x is Autodesk.Revit.DB.Wall;
        public static Func<Autodesk.Revit.DB.Element, bool> FilterGrids { get; set; }
        = x => x is Autodesk.Revit.DB.Grid;
    }

}
