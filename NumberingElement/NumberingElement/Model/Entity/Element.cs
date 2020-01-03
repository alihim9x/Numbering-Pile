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
    public class Element : NotifyClass
    {

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
        //private List<Model.Entity.Pile> intersectEttPiles;
        //public List<Model.Entity.Pile> IntersectEttPiles
        //{
        //    get
        //    {
        //        if (intersectEttPiles == null)
        //        {
        //            intersectEttPiles = this.GetIntersectEttPiles();

        //        }
        //        return intersectEttPiles;
        //    }
        //}
        //public List<Model.Entity.Pile> EttPiles { get; set; } = new List<Pile>();

        private List<Model.Entity.Pile> ettPiles;
        public List<Model.Entity.Pile> EttPiles
        {
            get
            {
                if(ettPiles == null)
                {
                    ettPiles = this.GetIntersectEttPiles();
                }
                return ettPiles;
            }
        }


    }

       
}
