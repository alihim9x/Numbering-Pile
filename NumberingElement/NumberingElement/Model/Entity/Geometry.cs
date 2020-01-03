using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Model.Entity
{
    public class Geometry
    {

        public XYZ Origin { get; set; }
        public XYZ BasisX { get; set; }
        public XYZ BasisY { get; set; }
        public XYZ BasisZ { get; set; }
        public double Value2Sort { get; set; }
    }
}
