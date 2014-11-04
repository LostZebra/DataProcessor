using System;
using System.Configuration;
using System.Collections.Generic;
// Testing
using System.Diagnostics;

namespace DataProcessorLib
{
    public static class CompareUtilities
    {
        public static int? ValueTypeComparer<T>(T firstValue, T secondValue)
        {
            IComparer<T> defaultComparer = Comparer<T>.Default;
            int? ret = defaultComparer.Compare(firstValue, secondValue);
            return ret == 0 ? new int?() : ret;
        }

        public static int? ReferenceTypeComparer<T>(T firstValue, T secondValue) where T : class
        {
            return firstValue == secondValue
                ? 0
                : firstValue == null
                    ? -1
                    : secondValue == null ? 1 : new int?();

        }
    }

    public static class DataFormatUtilities
    {
        public static double ToPostDigitsDouble(this string dataStr, int numberOfDigits)
        {
            int tempInt = (int)(double.Parse(dataStr) / Math.Pow(0.1, numberOfDigits + 1));
            int offset = (tempInt % 10) >= 5 ? 10 : 0;
            tempInt += offset;
            double tempDouble = tempInt * Math.Pow(0.1, numberOfDigits + 1);
            string processedDataStr = tempDouble.ToString("G");
            processedDataStr = processedDataStr.Substring(0, processedDataStr.LastIndexOf(".") + numberOfDigits + 1);
            return double.Parse(processedDataStr);
        }

        public static double CutLeadingCharacters(this string dataStr, int lengthOfLeadingCharaters = 1)
        {
            return double.Parse(dataStr.Substring(lengthOfLeadingCharaters));
        }
    }
}
