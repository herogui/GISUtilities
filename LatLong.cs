using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

//经纬度转换
namespace GISUtilities
{
    public class LatLong
    {
        /// <summary>
        /// Normalize Longitude value - returns value within range of -180 to +180 decimal degrees
        /// </summary>
        /// <param name="longitude">de-normalized longitude in decimal degrees</param>
        /// <returns>longitude value within range of -180 to +180 decimal degrees</returns>
        public static decimal NormalizeLongitude(decimal longitude)
        {
            return NormalizeLong(longitude);
        }

        public static decimal NormalizeLongitude(int longitudeDegrees, decimal longitudeMinutes)
        {
            decimal longitudeDD = DegDecMinToDD(longitudeDegrees, longitudeMinutes);
            return NormalizeLong(longitudeDD);
        }

        public static decimal NormalizeLongitude(int longitudeDegrees, int longitudeMinutes, decimal longitudeSeconds)
        {
            decimal longitudeDD = DegMinSecToDD(longitudeDegrees, longitudeMinutes, longitudeSeconds);
            return NormalizeLong(longitudeDD);
        }

        private static decimal NormalizeLong(decimal longitudeDD)
        {
            decimal retVal;
            // if the passed in value is already within range, no need for further processing
            if (longitudeDD >= -180.0M && longitudeDD <= 180.0M)
                return longitudeDD;

            // we need to know how many times 180 goes into the passed number, as well as the remainder
            int factor = (int)Math.Truncate(longitudeDD / 180.0M);
            decimal remainder = Math.Abs(longitudeDD % 180M);

            bool isEven = (factor % 2 == 0);

            // if the value passed creates an even factor, the remainder is the negative longitude
            // if the value passed creates an odd factor, then the remainder subracted from 180 is the longitude
            // if the value passed creates an odd factor, we also need to multiply by -1 if the factor is positive

            if (isEven)
                retVal = (-1.0M * remainder) * ((factor > 0) ? -1.0M : 1M);
            else
                retVal = (180.0M - remainder) * ((factor > 0) ? -1.0M : 1M) ;

            return retVal;
        }

        #region conversion utility methods

        public static decimal DegDecMinToDD(int degrees, decimal minutes)
        {
            return Convert.ToDecimal(degrees + (minutes/60.0M));
        }

        public static decimal DegMinSecToDD(int degrees, int minutes, decimal seconds)
        {
            decimal decimalMinutes = Convert.ToDecimal(minutes) + (seconds/60.0M);
            return DegDecMinToDD(degrees, decimalMinutes);
        }

        #endregion
    }
}
