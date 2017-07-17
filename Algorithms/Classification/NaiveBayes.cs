using System;
using System.Linq;

namespace GrandeOmegaWebApp.Algorithms.Classification
{
    public class NaiveBayes
    {
        /// <summary>
        /// Calculates Sample Variance of given data
        /// </summary>
        /// <param name="data">The data to calculate the Sample Variance of</param>
        /// <returns>double - Sample Variance</returns>
        public static double CalculateSampleVariance(double[] data)
        {
            var mean = data.Average();
            var variance = data.Sum(value => Math.Pow((value - mean), 2));
            return variance / (data.Length - 1);
        }

        /// <summary>
        /// Calculated the Normal (or Gaussian) Distribution for the given data
        /// </summary>
        /// <param name="data"> Double Array with data to calculate the nDis for </param>
        /// <param name="sVar"> Sample Variance of the data </param>
        /// <param name="classValue">The variable to calculate the </param>
        /// <returns>double - Normal Distribution value</returns>
        public static double CalcuateNormalDistribution(double[] data, double sVar, double classValue)
        {
            var mean = data.Average();
            // 1 / sqrt( 2 * π * σ² )
            var nDis = 1 / Math.Sqrt(2 * Math.PI * sVar);
            // (× -μ)² / (2 * σ²)
            var nDis2 = -Math.Pow(classValue - mean, 2) / (2 * sVar);
            // 1 / sqrt( 2 * π * σ² ) e (-(× - μ)² / (2 * σ²))
            var nDis3 = nDis * Math.Exp(nDis2);

            return nDis3;
        }
    }
}