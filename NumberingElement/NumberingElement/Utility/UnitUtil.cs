using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;

namespace Utility
{
    public static class UnitUtil
    {         
        const double FEET_TO_METERS = 0.3048;
        const double FEET_TO_CENTIMETERS = FEET_TO_METERS * 100;
        const double FEET_TO_MILIMETERS = FEET_TO_METERS * 1000;
        public static double feet2Meter(this double feet)
        {
            return feet * FEET_TO_METERS;
        }
        public static double feet2MeterSquare(this double feet)
        {
            return feet * FEET_TO_METERS * FEET_TO_METERS;
        }
        public static double feet2MeterCubic(this double feet)
        {
            return feet * FEET_TO_METERS * FEET_TO_METERS * FEET_TO_METERS;
        }
        public static double feet2Meter(this int feet)
        {
            return feet * FEET_TO_METERS;
        }
        public static double feet2MeterSquare(this int feet)
        {
            return feet * FEET_TO_METERS * FEET_TO_METERS;
        }
        public static double feet2Centimeter(this double feet)
        {
            return feet * FEET_TO_CENTIMETERS;
        }
        public static double feet2CentimeterSquare(this double feet)
        {
            return feet * FEET_TO_CENTIMETERS * FEET_TO_CENTIMETERS;
        }
        public static double feet2CentimeterCubic(this double feet)
        {
            return feet * FEET_TO_CENTIMETERS * FEET_TO_CENTIMETERS * FEET_TO_CENTIMETERS;
        }
        public static double feet2Milimeter(this double feet)
        {
            return feet * FEET_TO_MILIMETERS;
        }
        public static double feet2MilimeterSquare(this double feet)
        {
            return feet * FEET_TO_MILIMETERS * FEET_TO_MILIMETERS;
        }
        public static double feet2MilimeterCubic(this double feet)
        {
            return feet * FEET_TO_MILIMETERS * FEET_TO_MILIMETERS * FEET_TO_CENTIMETERS;
        }
        public static double feet2Milimeter(this int feet)
        {
            return feet * FEET_TO_MILIMETERS;
        }
        public static double feet2MilimeterSquare(this int feet)
        {
            return feet * FEET_TO_MILIMETERS * FEET_TO_MILIMETERS;
        }
        public static double meter2Feet(this double meter)
        {
            return meter / FEET_TO_METERS;
        }
        public static double meter2FeetSquare(this double meter)
        {
            return meter / (FEET_TO_METERS * FEET_TO_METERS);
        }
        public static double meter2Feet(this int meter)
        {
            return meter / FEET_TO_METERS;
        }
        public static double meter2FeetSquare(this int meter)
        {
            return meter / (FEET_TO_METERS * FEET_TO_METERS);
        }
        public static double milimeter2Feet(this double milimeter)
        {
            return milimeter / FEET_TO_MILIMETERS;
        }
        public static double milimeter2FeetSquare(this double milimeter)
        {
            return milimeter / (FEET_TO_MILIMETERS * FEET_TO_MILIMETERS);
        }
        public static double milimeter2Feet(this int milimeter)
        {
            return milimeter / FEET_TO_MILIMETERS;
        }
        public static double milimeter2FeetSquare(this int milimeter)
        {
            return milimeter / (FEET_TO_MILIMETERS * FEET_TO_MILIMETERS);
        }
        public static double radian2Degree(this double rad)
        {
            return rad * 180 / Math.PI;
        }
        public static double radian2Degree(this int rad)
        {
            return rad * 180 / Math.PI;
        }
        public static double degree2Radian(this double deg)
        {
            return deg * Math.PI / 180;
        }
        public static double degree2Radian(this int deg)
        {
            return deg * Math.PI / 180;
        }
        public static double GetUnitValue(this double value, DisplayUnitType? displayUnitType)
        {
            if (displayUnitType == null) return value;
            switch (displayUnitType.Value)
            {
                case DisplayUnitType.DUT_MILLIMETERS:
                    return Math.Round(value.feet2Milimeter());
                case DisplayUnitType.DUT_CENTIMETERS:
                    return Math.Round(value.feet2Centimeter(), 2);
                case DisplayUnitType.DUT_METERS:
                    return Math.Round(value.feet2Meter(), 4);
                case DisplayUnitType.DUT_SQUARE_MILLIMETERS:
                    return Math.Round(value.feet2MilimeterSquare(), 0);
                case DisplayUnitType.DUT_SQUARE_CENTIMETERS:
                    return Math.Round(value.feet2CentimeterSquare(), 2);
                case DisplayUnitType.DUT_SQUARE_METERS:
                    return Math.Round(value.feet2MeterSquare(), 4);
                case DisplayUnitType.DUT_CUBIC_MILLIMETERS:
                    return Math.Round(value.feet2MilimeterCubic(), 0);
                case DisplayUnitType.DUT_CUBIC_CENTIMETERS:
                    return Math.Round(value.feet2CentimeterCubic(), 2);
                case DisplayUnitType.DUT_CUBIC_METERS:
                    return Math.Round(value.feet2MeterCubic(), 4);
                case DisplayUnitType.DUT_CURRENCY:
                default:
                    return value;
            }
            throw new Exception("This code should not have reached!");
        }

        public static int RoundUp(double d)
        {
            return Math.Round(d, 0) < d ? (int)(Math.Round(d, 0) + 1) : (int)(Math.Round(d, 0));
        }
        public static int RoundDown(double d)
        {
            return Math.Round(d, 0) < d ? (int)(Math.Round(d, 0)) : (int)(Math.Round(d, 0) - 1);
        }
    }
}
