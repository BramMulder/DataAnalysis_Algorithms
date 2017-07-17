using System;

namespace GrandeOmegaWebApp.Algorithms.Classification
{
    public class SimilarityCalculations
    {
        /// <summary>
        /// Calculates the Euclidean distance between dataX array and dataY array
        /// </summary>
        /// <param name="dataX">Array of doubles containing the data of which you want calculate the distance</param>
        /// <param name="dataY">Second Array of doubles containing the data of which you want calculate the distance</param>
        /// <returns>Double Euclidean Distance between the data in the datasets</returns>
        public static double CalculateEuclideanDistance(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                return -2;

            double distance = 0.0;

            //Calculate euclidean distance
            for (int i = 0; i < dataX.Length; i++)
            {
                double delta = dataX[i] - dataY[i];
                distance += Math.Pow(delta, 2);
            }

            distance = Math.Sqrt(distance);

            return distance;
        }

        /// <summary>
        /// Calculates the Manhattan distance between dataX array and dataY array
        /// </summary>
        /// <param name="dataX">Array of doubles containing the data of which you want calculate the distance</param>
        /// <param name="dataY">Second Array of doubles containing the data of which you want calculate the distance</param>
        /// <returns>Double Euclidean Distance between the data in the datasets</returns>
        public static double CalculateManhattanDistance(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                return -2;

            double distance = 0.0;

            //Sum all (absolute) delta values between Xi and Yi
            for (int i = 0; i < dataX.Length; i++)
            {
                double delta = dataX[i] - dataY[i];
                distance = distance + Math.Abs(delta);
            }

            return distance;
        }

    }
}