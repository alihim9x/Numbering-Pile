using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;
namespace Utility
{
    public static class GeometryUtil
    {
        private static ModelData modelData
        {
            get
            {
                return ModelData.Instance;
            }
        }
        public static Model.Entity.Geometry GetGeometry(this Model.Entity.Pile pile)
        {
            var settingCate = modelData.Setting.Category.Id.IntegerValue;
            var geometry = new Model.Entity.Geometry();
            var revitElem = pile.RevitElement;
            if(settingCate == (int)Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFoundation)
            {
                geometry.Origin = (revitElem.Location as Autodesk.Revit.DB.LocationPoint).Point;
            }
            else if(settingCate == (int)Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFraming)
            {
                var locCurve = (revitElem.Location as Autodesk.Revit.DB.LocationCurve).Curve;

                geometry.Origin = (locCurve.GetEndPoint(0) + locCurve.GetEndPoint(1))/ 2;
            }


            return geometry;
        }
    }
}
