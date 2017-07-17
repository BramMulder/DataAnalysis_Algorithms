using System;

namespace GrandeOmegaWebApp.Algorithms.Correlation
{
    public class PearsonCoefficient
    {
        /// <summary>
        /// Calculates the Pearson Coefficient
        /// </summary>
        /// <param name="dataX">Array with doubles containing data</param>
        /// <param name="dataY">>Array with doubles containing data</param>
        /// <returns>Pearson Coefficient</returns>
        public double CalculatePearsonCoefficient(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                throw new Exception("Coordinates lengths differ. Please make sure all point have the same amount of coordinates");

            // n
            var n = dataX.Length;

            // ∑ x
            var xSum = 0.0;
            // ∑ y     
            var ySum = 0.0;
            // ∑ ( x(i) * y(i) )
            var xySum = 0.0;

            // ∑ x(i)²
            var xSquareSum = 0.0;

            // ∑ y(i)²
            var ySquareSum = 0.0;

            for (int i = 0; i < dataX.Length; i++)
            {
                // ∑ x
                xSum += dataX[i];
                // ∑ y
                ySum += dataY[i];

                // ∑ ( x(i) * y(i) )
                xySum += (dataX[i] * dataY[i]);
                // ∑ x(i)²
                xSquareSum += Math.Pow(dataX[i], 2);
                // ∑ y(i)²
                ySquareSum += Math.Pow(dataY[i], 2);
            }

            //∑ (x(i))²
            var xSumSquared = Math.Pow(xSum, 2);
            //∑ (y(i))²
            var ySumSquared = Math.Pow(ySum, 2);

            double r = (xySum - ((xSum * ySum) / n)) / (Math.Sqrt(xSquareSum - (xSumSquared / n)) * Math.Sqrt(ySquareSum - (ySumSquared / n)));

            return r;
        }

    }
}