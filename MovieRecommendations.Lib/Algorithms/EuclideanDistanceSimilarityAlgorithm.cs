using System;
using System.Collections.Generic;
using System.Linq;
using MovieRecommendations.Entities;
using MovieRecommendations.Lib.Helpers;

namespace MovieRecommendations.Lib.Algorithms
{
    public class EuclideanDistanceSimilarityAlgorithm : ISimilarityAlgorithm
    {
        public double CalculateSimilarity<TKey>(IDictionary<TKey, double> valuesA, IDictionary<TKey, double> valuesB)
        {
            var matchingKeys = valuesA.Keys.Where(valuesB.ContainsKey);

            if (!matchingKeys.Any())
                return 0;

            var sumOfSquares = matchingKeys
                .Select(k => Math.Pow(valuesA[k] - valuesB[k], 2))
                .Sum();

            //return MathHelper.Inverse(1 + Math.Sqrt(sumOfSquares));
            return MathHelper.Inverse(1 + sumOfSquares);
        }
    }
}

