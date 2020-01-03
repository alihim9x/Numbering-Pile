using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class GeomUtil
    {
        
        public static double feet2milimet { get; set; } = 304.8;
        public static double feet2meter { get { return feet2milimet / 1000; } }
        public static double Feet2Milimeter (this double value)
        {
            return value * feet2milimet;
        }
        public static double Milimet2Feet(this double value)
        {
            return  value / feet2milimet;
        }
        public static double Feet2Meter(this double value)
        {
            return Feet2Meter(value) / 1000;
        }
        public static double FeetSq2MilimeterSq(this double value)
        {
            return value * feet2milimet * feet2milimet;
        }
        public static double FeetSq2MeterSq(this double value)
        {
            return FeetSq2MilimeterSq(value) / (1000*1000);
        }
        public static double FeetCb2MeterCb(this double value)
        {
            return value * 0.02831685;
        }
        private static double epsilon = 0.00001;
        public static bool IsEqual(this double first, double second)
        {
            return (Math.Abs(first - second) < epsilon);
        }
        private static double GetNormalizeDotMatrix (this Autodesk.Revit.DB.XYZ firstVector, Autodesk.Revit.DB.XYZ secondVector)
        {
            return (firstVector.Normalize().DotProduct(secondVector.Normalize()));
        }
        public static bool IsParallelDirection (this Autodesk.Revit.DB.XYZ firstVector, Autodesk.Revit.DB.XYZ secondVector)
        {            
            
            return Math.Abs(firstVector.GetNormalizeDotMatrix(secondVector)).IsEqual(1);
        }
        public static bool IsSameDirection(this Autodesk.Revit.DB.XYZ firstVector, Autodesk.Revit.DB.XYZ secondVector)
        {
            return (firstVector.GetNormalizeDotMatrix(secondVector)).IsEqual(1);

        }
        public static bool IsOppositeDirection(this Autodesk.Revit.DB.XYZ firstVector, Autodesk.Revit.DB.XYZ secondVector)
        {
            return (firstVector.GetNormalizeDotMatrix(secondVector)).IsEqual(-1);

        }
        public static bool IsPerpendicularDirection(this Autodesk.Revit.DB.XYZ firstVector, Autodesk.Revit.DB.XYZ secondVector)
        {
            return (firstVector.GetNormalizeDotMatrix(secondVector)).IsEqual(0);

        }
        public static double RadianToDegree (this double value)
        {
            return Math.Round(value * 180 / Math.PI, 0);
        }
        public static bool IsXOrY(this Autodesk.Revit.DB.XYZ vector)
        {
            var angle = vector.AngleTo(Autodesk.Revit.DB.XYZ.BasisX).RadianToDegree();
            if ((angle >= 0 && angle < 45) || (angle > 135 && angle < 225) || (angle > 315 && angle <= 360))
            { return true; }
            else
            {
                return false;
            }
        }
        public static double Distance2P(this Autodesk.Revit.DB.XYZ firstP, Autodesk.Revit.DB.XYZ secondP)
        {
            double dis = 0;
            dis = Math.Sqrt(Math.Pow(firstP.X - secondP.X,2) + Math.Pow(firstP.Y - secondP.Y,2)) ;
            return dis;
        }
        






    }
}
