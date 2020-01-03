using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using SingleData;
using Utility;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace NumberingElement
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region Initial
            var singleTon = Singleton.Instance = new Singleton();
            var modelData = singleTon.ModelData;
            var revitData = singleTon.RevitData;
            revitData.UIApplication = commandData.Application;
            var sel = revitData.Selection;
            var doc = revitData.Document;
            var activeView = revitData.ActiveView;
            var tx = revitData.Transaction;
            var uidoc = revitData.UIDocument;
            var app = revitData.Application;
            tx.Start();
            #endregion
            var form = FormData.Instance.InputForm;

            form.ShowDialog();

            //var foundationCateFil = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);
            //var pickedLine = doc.GetElement(sel.PickObject(ObjectType.Element, "Vui lòng chọn đường Line"));


            //var bBoxpLine = pickedLine.get_BoundingBox(null);
            //var ol = new Outline(bBoxpLine.Min, bBoxpLine.Max);
            //var bbIntersectFilter = new BoundingBoxIntersectsFilter(ol);
            //var elementIntersect = new ElementIntersectsElementFilter(pickedLine);
            //var collector = new FilteredElementCollector(doc).WherePasses(foundationCateFil).WherePasses(bbIntersectFilter)
            //    .WherePasses(elementIntersect).ToList();
            //List<Element> sortElem = new List<Element>();
            //var firstPointBeam = (pickedLine.Location as LocationCurve).Curve.GetEndPoint(0);

            //collector.OrderBy(x =>(x.Location as LocationPoint).Point.Distance2P(firstPointBeam));
            //double a = 1;
            //foreach (var item in collector)
            //{
            //    item.SetValue("Mark", a.ToString());
            //    a++;
            //}

            //var pickedPiles = sel.PickElementsByRectangle(new SelectionUtil(x=>x.Category.Id.IntegerValue 
            //== (int)BuiltInCategory.OST_StructuralFoundation),"Vui lòng chọn cọc").ToList();
            //pickedPiles.OrderByDescending(x => (x.Location as LocationPoint).Point.Y).ThenBy(x => (x.Location as LocationPoint).Point.X);
            //for (int i = 0; i < pickedPiles.Count; i++)
            //{
            //    pickedPiles[i].SetValue("Comments", i.ToString());
            //}

            //var pickedC = sel.PickObject(ObjectType.Element).GetRevitElement();
            //var pnt = sel.PickPoint();
            ////TaskDialog.Show("Revit", $"{(c.Location as LocationCurve).Curve.GetEndParameter(1).feet2Milimeter()}");
            //var c = (pickedC.Location as LocationCurve).Curve;
            //var listPoints =c.GetParamPoints(3);
            //var ir = c.Project(pnt);
            //double a = ir.Parameter.feet2Milimeter();
            //doc.Create.NewDetailCurve(activeView, Line.CreateBound(ir.XYZPoint, ir.XYZPoint + 500.0.milimeter2Feet() * activeView.UpDirection));
            //TaskDialog.Show("Revit", a.ToString());


            //foreach (var item in listPoints)
            //{
            //    doc.Create.NewDetailCurve(activeView, Line.CreateBound(item, item + 500.0.milimeter2Feet() * activeView.UpDirection));
            //}
            

            #region Test
            //Func<Autodesk.Revit.DB.Element, bool> filterLine = x =>
            //{
            //    var cate = x.Category.Id.IntegerValue;
            //    if (cate == (int)Autodesk.Revit.DB.BuiltInCategory.OST_Lines) return true;
            //    return false;
            //};
            //var ettElem = new Model.Entity.Element();
            //var path = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, new SelectionUtil(filterLine, null), "Vui lòng chọn đường dẫn").GetRevitElement();
            //ettElem.RevitElement = path;

            //List<Model.Entity.Pile> intersectEttPiles = new List<Model.Entity.Pile>();
            //var foundationCate = new Autodesk.Revit.DB.ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);
            //var revitElem = ettElem.RevitElement;
            //var bbRevitElem = revitElem.get_BoundingBox(null);
            //var ol = new Autodesk.Revit.DB.Outline(bbRevitElem.Min, bbRevitElem.Max);
            //var bbIntersectFilter = new Autodesk.Revit.DB.BoundingBoxIntersectsFilter(ol);
            //var intersectPiles = new FilteredElementCollector(revitData.Document).WherePasses(foundationCate)
            //    .WherePasses(bbIntersectFilter).ToList();
            //var curvePath = (revitElem.Location as LocationCurve).Curve;
            //revitData.Selection.SetElementIds(intersectPiles.Select(x => x.Id).ToList());
            //foreach (var item in intersectPiles)
            //{
            //    var itemPoint = (item.Location as LocationPoint).Point;
            //    var intersectionResult = curvePath.Project(itemPoint);
            //    var projection2curve = intersectionResult.XYZPoint;
            //    double distance2P = itemPoint.Distance2P(projection2curve);
            //    if (distance2P < 500.0.milimeter2Feet())
            //    {
            //        intersectEttPiles.Add(new Model.Entity.Pile { RevitElement = item });
            //        revitData.Document.Create.NewDetailCurve(revitData.ActiveView, Line.CreateBound(projection2curve, itemPoint));

            //    }

            //}
            #endregion

            //var foun = revitData.Foundations.FirstOrDefault();
            //var paramset = foun.Parameters.GetEnumerator();
            //List<Parameter> parameters = new List<Parameter>();
            //while (paramset.MoveNext())
            //{
            //    parameters.Add(paramset.Current as Parameter);

            //}
            //TaskDialog.Show("revit", $"{parameters.Count}");

            //var parameters = modelData.Parameters;
            //TaskDialog.Show("Revit", $"{parameters.Count}");

            tx.Commit();

            return Result.Succeeded;
        }
    }
}
