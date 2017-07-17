using System;
using System.Linq;

namespace GrandeOmegaWebApp.Algorithms.Regression
{
    public class LinearRegressionLineCalculations
    {
        /// <summary>
        /// Calculates the formula  y = ax + b  for a linear regression line
        /// </summary>
        /// <param name="dataX">Array with doubles containing data</param>
        /// <param name="dataY">Array with doubles containing data</param>
        /// <returns>Tuple containing the starting points of a line and the endiong points of a line</returns>
        public static Tuple<double[], double[]> CalculateTrendLineCoordinates(double[] dataX, double[] dataY)
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
            var x2Sum = 0.0;

            for (var i = 0; i < n; i++)
            {
                xSum += dataX[i];
                ySum += dataY[i];
                x2Sum += dataX[i] * dataX[i];
                xySum += dataX[i] * dataY[i];
            }
            
            var slope = (n * xySum - xSum * ySum) / (n * x2Sum - xSum * xSum);
            double intercept = (ySum - (slope * xSum)) / n ;

            return GetTrendLineCoordinates(dataX, slope, intercept);
        }

        /// <summary>
        /// Uses the function  y = ax+b, calculated in the CalculateTrendLineCoordinates method,
        /// to determine the starting and ending points of the linear line
        /// </summary>
        /// <param name="dataX">Array with doubles containing data X coordinates</param>
        /// <param name="slope">The slope of the line</param>
        /// <param name="intercept">The interception with the Y-axis</param>
        /// <returns>Tuple containing the starting points of a line and the endiong points of a line</returns>
        private static Tuple<double[], double[]> GetTrendLineCoordinates(double[] dataX, double slope, double intercept)
        {
            var startCoordinates = new double[2];
            var endCoordinates = new double[2];

            var xMax = dataX.Max();
            var xMin = dataX.Min();

            //Start coordinates of the line
            startCoordinates[0] = xMin;
            startCoordinates[1] = intercept;
            //End coordinates of the line
            endCoordinates[0] = xMax;
            endCoordinates[1] = (xMax - xMin) * slope; 

            return new Tuple<double[], double[]>(startCoordinates, endCoordinates);
        }      
    }
}