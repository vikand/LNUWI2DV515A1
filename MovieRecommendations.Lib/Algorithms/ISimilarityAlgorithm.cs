using MovieRecommendations.Entities;
using System.Collections.Generic;

namespace MovieRecommendations.Lib.Algorithms
{
    public interface ISimilarityAlgorithm
    {
        double CalculateSimilarity<TKey>(IDictionary<TKey, double> valuesA, IDictionary<TKey, double> valuesB);
    }
}
