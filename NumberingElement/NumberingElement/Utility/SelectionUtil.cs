using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public  class SelectionUtil : ISelectionFilter
    {
        private   Func<Autodesk.Revit.DB.Element, bool> filterElement;
        private   Func<Autodesk.Revit.DB.Reference, bool> filterReference;
        public SelectionUtil(Func<Element, bool> filterElement, Func<Reference, bool> filterReference = null)
        {
            this.filterElement = filterElement;
            this.filterReference = filterReference;
        }

        public bool AllowElement(Element elem)
        {
            return filterElement(elem);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return filterReference(reference);
        }
    }
}
