using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using SingleData;

namespace Model.Entity
{
    public class Setting : NotifyClass
    {
        
        private Autodesk.Revit.DB.Category category;
        public virtual Autodesk.Revit.DB.Category Category
        {
            get
            {
                return category;
            }
            set
            {
                if (category == value) return;
                category = value;
                OnPropertyChanged();
            }
        }
        private string preFix;
        public string PreFix
        {
            get
            {
                return preFix;
            }
            set
            {
                if (preFix == value) return;
                preFix = value;
                OnPropertyChanged();
            }
        }
        private int startNo;
        public int StartNo
        {
            get
            {
                return startNo;
            }
            set
            {
                if (startNo == value) return;
                startNo = value;
                OnPropertyChanged();
            }
        }
        
        
        public Model.Entity.Element EttElement { get; set; }
        public List<Model.Entity.Element> EttElements { get; set; } = new List<Element>();
        private double distanceFromPile2Path;
        public double DistanceFromPile2Path
        {
            get
            {
                return distanceFromPile2Path;
            }
            set
            {
                if (distanceFromPile2Path == value) return;
                distanceFromPile2Path = value;
                OnPropertyChanged();
            }
        }
        private Autodesk.Revit.DB.Parameter parameter;
        public Autodesk.Revit.DB.Parameter Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                if (parameter == value) return;
                parameter = value;
                OnPropertyChanged();
            }
        }
        private VerOrHor verOrHor;
        public VerOrHor VerOrHor
        {
            get
            {
                return verOrHor;
            }
            set
            {
                if (verOrHor == value) return;
                verOrHor = value;
                OnPropertyChanged();
            }
        }


    }
}
