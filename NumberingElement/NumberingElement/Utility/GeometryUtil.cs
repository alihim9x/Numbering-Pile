using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class GeometryUtil
    {
        public static Model.Entity.Geometry GetGeometry(this Model.Entity.Pile pile)
        {
            var geometry = new Model.Entity.Geometry();
            var revitElem = pile.RevitElement;

            geometry.Origin = (revitElem.Location as Autodesk.Revit.DB.LocationPoint).Point;


            return geometry;
        }
    }
}
