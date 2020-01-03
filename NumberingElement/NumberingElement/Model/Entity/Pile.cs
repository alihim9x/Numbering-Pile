using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using SingleData;
using Autodesk.Revit.DB.Structure;

namespace Model.Entity
{
    public class Pile : NotifyClass
    {
        public Model.Entity.Element HostEttElement { get; set; }
        public Autodesk.Revit.DB.Element RevitElement { get; set; }
        //private ElementType? elementType;
        //public ElementType? ElementType
        //{
        //    get
        //    {
        //        if (elementType == null)
        //        {
        //            elementType = this.GetElementType();
        //        }
        //        return elementType;
        //    }
        //}
        private Geometry geometry;
        public Geometry Geometry
        {
            get
            {
                if (geometry == null)
                {
                    geometry = this.GetGeometry();
                }
                return geometry;
            }
        }
        private double value2Sort;
        public double Value2Sort
        {
            get
            {
                if(value2Sort == 0)
                {
                    value2Sort = this.GetValue2Sort();
                }
                return value2Sort;
            }
        }

    }

       
}
