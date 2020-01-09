using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;
using Autodesk.Revit.DB;

namespace Utility
{
    public static class ElementUtil
    {
        private static RevitData revitData
        {
            get
            {
                return RevitData.Instance;
            }
        }

        public static Autodesk.Revit.DB.Element GetRevitElement(this Autodesk.Revit.DB.ElementId elemId)
        {
            return RevitData.Instance.Document.GetElement(elemId);
        }
        public static Autodesk.Revit.DB.Element GetRevitElement(this Autodesk.Revit.DB.Reference reference)
        {
            return RevitData.Instance.Document.GetElement(reference);
        }
        //public static Model.Entity.Element GetEntityElement(this Autodesk.Revit.DB.Element elem)
        //{
        //    var ettElem = ModelData.Instance.EttElements.SingleOrDefault(x => x.RevitElement.UniqueId == elem.UniqueId);
        //    if (ettElem == null)
        //    {
        //        ettElem = new Model.Entity.Element()
        //        {
        //            RevitElement = elem
        //        };
        //        ModelData.Instance.EttElements.Add(ettElem);
        //    }
        //    return ettElem;
        //}
        //public static void CreateRebars(this Model.Entity.Element element)
        //{
        //    foreach (var rebar in element.Rebars)
        //    {
        //        rebar.CreateRebar();
        //    }
        //    foreach (var stirrup in element.Stirrups)
        //    {
        //        stirrup.CreateStirrup();
        //    }
        //}
        //public static Model.Entity.ElementType GetElementType(this Model.Entity.Element ettElem)
        //{
        //    var revitElem = ettElem.RevitElement;
        //    var cate = revitElem.Category;
        //    if (revitElem is Autodesk.Revit.DB.Floor)
        //        return Model.Entity.ElementType.Floor;
        //    if (revitElem is Autodesk.Revit.DB.Wall)
        //        return Model.Entity.ElementType.Wall;
        //    if (cate.Id.IntegerValue == (int)Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFraming)
        //        return Model.Entity.ElementType.Framing;
        //    if (cate.Id.IntegerValue == (int)Autodesk.Revit.DB.BuiltInCategory.OST_StructuralColumns)
        //        return Model.Entity.ElementType.Column;
        //    return Model.Entity.ElementType.Undefined;
        //}
        
        public static Autodesk.Revit.DB.XYZ MaxRepeatedItem(this List<Autodesk.Revit.DB.XYZ> listXYZ)
        {
            var maxRepeatedItems = listXYZ.GroupBy(x => x.Z).OrderByDescending(x => x.Count()).First().Select(x => x).First();
            return maxRepeatedItems;

        }
        public static List<Model.Entity.Pile> GetIntersectEttPiles (this Model.Entity.Element ettElem)
        {
            
            var setting = ModelData.Instance.Setting;
            var settingCate = setting.Category.Id;
            var verOrHorFraming = setting.VerOrHor;
            var distanceFromPile2Path = setting.DistanceFromPile2Path;
            List<Model.Entity.Pile> intersectEttPiles = new List<Model.Entity.Pile>();
            //var foundationCate = new Autodesk.Revit.DB.ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);
            var cateFilter = new Autodesk.Revit.DB.ElementCategoryFilter(settingCate);
            var revitElem = ettElem.RevitElement;
            var bbRevitElem = revitElem.get_BoundingBox(null);
            var ol = new Autodesk.Revit.DB.Outline(bbRevitElem.Min, bbRevitElem.Max);
            var bbIntersectFilter = new Autodesk.Revit.DB.BoundingBoxIntersectsFilter(ol);
            var intersectPiles = new FilteredElementCollector(revitData.Document).WherePasses(cateFilter)
                .WherePasses(bbIntersectFilter).ToList();
            var curvePath = (revitElem.Location as LocationCurve).Curve;
            XYZ itemPoint = null;
            Curve curveFraming = null; 
            //revitData.Selection.SetElementIds(intersectPiles.Select(x => x.Id).ToList()); // Test Intersected Pile
            foreach (var item in intersectPiles)
            {
                if(settingCate.IntegerValue == (int)BuiltInCategory.OST_StructuralFoundation)
                {
                    itemPoint = (item.Location as LocationPoint).Point;
                    
                }
                else if(settingCate.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming)
                {
                    curveFraming = (item.Location as LocationCurve).Curve;
                    itemPoint = (curveFraming.GetEndPoint(0) + curveFraming.GetEndPoint(1)) / 2;                  
                }
                var intersectionResult = curvePath.Project(itemPoint);
                var projection2curve = intersectionResult.XYZPoint;
                double distance2P = itemPoint.Distance2P(projection2curve);
                
                if (distance2P < distanceFromPile2Path.milimeter2Feet())
                {
                    switch (verOrHorFraming)
                    {
                        case Model.Entity.VerOrHor.HorizontalX:
                            {
                                if((curveFraming as Line).Direction.IsXOrY())
                                {
                                    intersectEttPiles.Add(new Model.Entity.Pile { RevitElement = item });

                                }
                                break;
                            }
                        case Model.Entity.VerOrHor.VerticalY:
                            {
                                if (!(curveFraming as Line).Direction.IsXOrY())
                                {
                                    intersectEttPiles.Add(new Model.Entity.Pile { RevitElement = item });
                                }
                                break;
                            }
                        default:
                            {
                                intersectEttPiles.Add(new Model.Entity.Pile { RevitElement = item });

                                break;
                            }

                    }
                    //intersectEttPiles.Add(new Model.Entity.Pile { RevitElement = item });
                    //revitData.Document.Create.NewDetailCurve(revitData.ActiveView, Line.CreateBound(projection2curve, itemPoint));
                }


            }
            //ettElem.IntersectEttPiles.ForEach(x => x.HostEttElement = ettElem);
            return intersectEttPiles;
        }
        
        public static double GetDis2FirstPoint (this Model.Entity.Element ettElem, XYZ pilePoint)
        {
            double value = 0;
            var revitElem = ettElem.RevitElement;
            var firstPoint = (revitElem.Location as LocationCurve).Curve.GetEndPoint(0);
            value = firstPoint.Distance2P(pilePoint);
            return value;
        }
        public static List<XYZ> GetParamPoints( this Curve curve, int num)
        {
            int MIN_NUM = 1, MAX_NUM = 10000;
            if(num <0)
            {
                num = -num;
            }
            if(num <MIN_NUM)
            {
                num = MIN_NUM;
            }
            else if(num >MAX_NUM)
            {
                num = MAX_NUM;
            }
            double param0 = curve.GetEndParameter(0);
            double param1 = curve.GetEndParameter(1);
            double paramStep = (param1 - param0) / num;

            var toReturn = new List<XYZ>();
            double currParam = param0;
            do
            {
                toReturn.Add(curve.Evaluate(currParam, false));
                currParam += paramStep;
            }
            while (currParam <= param1);
            return toReturn;

        }
        public static double GetValue2Sort(this Model.Entity.Pile ettPile)
        {
            double value = 0;
            var hostEttElem = ettPile.HostEttElement;
            var revitElem = hostEttElem.RevitElement;
            var pathCurve = (revitElem.Location as LocationCurve).Curve;
            var pnt = ettPile.Geometry.Origin;
            var intersectionResult = pathCurve.Project(pnt);
            value = intersectionResult.Parameter;
            return value;
        }
    }
}
