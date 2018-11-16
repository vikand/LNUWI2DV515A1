using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendations.Lib.Helpers
{
    public static class MathHelper
    {
        public static double Inverse(double value)
        {
            return value == 0 ? 0 : 1 / value;
        }

        public static double Euclidean(double value1, double value2)
        {
            return Math.Sqrt(Math.Pow(value1 - value2, 2));
        }

        public static double EuclideanSimilarity(double value1, double value2)
        {
            return Inverse(1 + Euclidean(value1, value2));
        }
    }
}
