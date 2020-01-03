using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class SolidUtil
    {
        public static double GetProximityHeight(this Autodesk.Revit.DB.Element elem)
        {
            var bb = elem.get_BoundingBox(null);
            return bb.Max.Z - bb.Min.Z;
        }
        public static Autodesk.Revit.DB.Solid MoveToOrigin(this Autodesk.Revit.DB.Solid solid)
        {

            var bb = solid.GetBoundingBox();
            var originBb = bb.Transform.Origin;
            var translateTf = Autodesk.Revit.DB.Transform.CreateTranslation(-originBb);
            return Autodesk.Revit.DB.SolidUtils.CreateTransformed(solid, translateTf);
        }
        public static Autodesk.Revit.DB.Solid GetSingleSolid(this Autodesk.Revit.DB.Element element)
        {
            var geoElem = element.get_Geometry(new Autodesk.Revit.DB.Options());
            return geoElem.GetSingleSolid();
            //Autodesk.Revit.DB.Solid solid = null;
            //foreach (Autodesk.Revit.DB.GeometryObject item in geoElem)
            //{
            //    if(item is Autodesk.Revit.DB.GeometryInstance)
            //    {
            //        var geoIns = item as Autodesk.Revit.DB.GeometryInstance;
            //        foreach (var item2 in geoIns.GetInstanceGeometry())
            //        {
            //            var s = item2 as Autodesk.Revit.DB.Solid;
            //            if(s != null && s.Faces.Size != 0 && s.Edges.Size != 0)
            //            {
            //                return s;
            //            }

            //        }
            //    }
            //    if(item is Autodesk.Revit.DB.Solid)
            //    {
            //        var s = solid as Autodesk.Revit.DB.Solid;
            //        if (s != null && s.Faces.Size != 0 && s.Edges.Size != 0)
            //        {
            //            return s;
            //        }
            //    }
            //}
            return null;
        }
        public static Autodesk.Revit.DB.Solid GetSingleSolidValidRef(this Autodesk.Revit.DB.Element element)
        {
            var geoElem = element.get_Geometry(new Autodesk.Revit.DB.Options() { ComputeReferences = true});
            return geoElem.GetSingleSolid();
            //Autodesk.Revit.DB.Solid solid = null;
            //foreach (Autodesk.Revit.DB.GeometryObject item in geoElem)
            //{
            //    if(item is Autodesk.Revit.DB.GeometryInstance)
            //    {
            //        var geoIns = item as Autodesk.Revit.DB.GeometryInstance;
            //        foreach (var item2 in geoIns.GetInstanceGeometry())
            //        {
            //            var s = item2 as Autodesk.Revit.DB.Solid;
            //            if(s != null && s.Faces.Size != 0 && s.Edges.Size != 0)
            //            {
            //                return s;
            //            }

            //        }
            //    }
            //    if(item is Autodesk.Revit.DB.Solid)
            //    {
            //        var s = solid as Autodesk.Revit.DB.Solid;
            //        if (s != null && s.Faces.Size != 0 && s.Edges.Size != 0)
            //        {
            //            return s;
            //        }
            //    }
            //}
            return null;
        }
        public static Autodesk.Revit.DB.Solid GetSingleSolid(this IEnumerable<Autodesk.Revit.DB.GeometryObject> geoObjs)
        {
            foreach (Autodesk.Revit.DB.GeometryObject item1 in geoObjs)
            {
                if (item1 is Autodesk.Revit.DB.GeometryInstance)
                {
                    var s = (item1 as Autodesk.Revit.DB.GeometryInstance).GetSingleSolid();
                    if (s != null) return s;
                }
                if (item1 is Autodesk.Revit.DB.Solid)
                {
                    var solid = item1 as Autodesk.Revit.DB.Solid;
                    if (solid != null && solid.Faces.Size != 0 && solid.Edges.Size != 0)
                    {
                        return solid;
                    }
                }
            }
            return null;
        }
        public static Autodesk.Revit.DB.Solid GetSingleSolid(this Autodesk.Revit.DB.GeometryInstance geoIns)
        {
            return GetSingleSolid(geoIns.GetInstanceGeometry());
        }
        public static Autodesk.Revit.DB.Solid GetOriginalSolid(this Autodesk.Revit.DB.Element elem)
        {
            if(elem is Autodesk.Revit.DB.FamilyInstance)
            {
                var fi = elem as Autodesk.Revit.DB.FamilyInstance;
                var originalGeoElem = fi.GetOriginalGeometry(new Autodesk.Revit.DB.Options());               
                var originalSolid = GetSingleSolid(originalGeoElem);

                var tsElem = fi.GetTransform();
                return Autodesk.Revit.DB.SolidUtils.CreateTransformed(originalSolid, tsElem);
            }
            if(elem is Autodesk.Revit.DB.Floor)
            {
                var floor = elem as Autodesk.Revit.DB.Floor;
                var botFaceRef = Autodesk.Revit.DB.HostObjectUtils.GetBottomFaces(floor).First();
                var botFace = elem.GetGeometryObjectFromReference(botFaceRef) as Autodesk.Revit.DB.PlanarFace;
                return Autodesk.Revit.DB.GeometryCreationUtilities.CreateExtrusionGeometry(botFace.GetEdgesAsCurveLoops()
                    , -botFace.FaceNormal, floor.GetProximityHeight()); // FaceNormal là vector pháp tuyến
            }
            if(elem is Autodesk.Revit.DB.Wall)
            {
                var wall = elem as Autodesk.Revit.DB.Wall;
                var sideFaceRef = Autodesk.Revit.DB.HostObjectUtils.GetSideFaces(wall as Autodesk.Revit.DB.HostObject
                    ,Autodesk.Revit.DB.ShellLayerType.Exterior).First();
                var sideFace = wall.GetGeometryObjectFromReference(sideFaceRef) as Autodesk.Revit.DB.PlanarFace;
                return Autodesk.Revit.DB.GeometryCreationUtilities.CreateExtrusionGeometry(sideFace.GetEdgesAsCurveLoops()
                    , -sideFace.FaceNormal, wall.Width);
            }
            throw new Model.Exception.CaseNotCheckException();
        }
        public static Autodesk.Revit.DB.Solid ScaleSolid(this Autodesk.Revit.DB.Solid solid, double factor)
        {
            var centerPoint = solid.ComputeCentroid();

            var tf = Autodesk.Revit.DB.Transform.Identity.ScaleBasis(factor);
            var scaledSolid =  Autodesk.Revit.DB.SolidUtils.CreateTransformed(solid, tf);
            var newCenterPoint = scaledSolid.ComputeCentroid();
            var translateVector = centerPoint - newCenterPoint; // Vecto tịnh tiến từ tâm mới về tâm cũ
            var translateTf = Autodesk.Revit.DB.Transform.CreateTranslation(translateVector);

            return Autodesk.Revit.DB.SolidUtils.CreateTransformed(scaledSolid, translateTf);
        }
        public static Autodesk.Revit.DB.Solid MergeSolid(this IEnumerable<Autodesk.Revit.DB.Solid> solids)
        {
            var mergeSolid = solids.First();
            foreach (var item in solids)
            {
                if(mergeSolid == item)
                {
                    continue;
                }
                else
                {
                    mergeSolid = Autodesk.Revit.DB.BooleanOperationsUtils.ExecuteBooleanOperation(mergeSolid, item
                        , Autodesk.Revit.DB.BooleanOperationsType.Union);
                }
            }
            return mergeSolid;
        }
        public static Autodesk.Revit.DB.Solid DifferenceSolid (this Autodesk.Revit.DB.Solid targetSolid,
            IEnumerable<Autodesk.Revit.DB.Solid> otherSolids)
        {
            var mergeOtherSolid = otherSolids.MergeSolid();
            otherSolids.ToList().Add(targetSolid);
            var mergeAllSolid = Autodesk.Revit.DB.BooleanOperationsUtils.ExecuteBooleanOperation(mergeOtherSolid, targetSolid,
                Autodesk.Revit.DB.BooleanOperationsType.Union);
            return Autodesk.Revit.DB.BooleanOperationsUtils.ExecuteBooleanOperation(mergeAllSolid, mergeOtherSolid, Autodesk.Revit.DB.BooleanOperationsType.Difference);
        }
        
    }
}
