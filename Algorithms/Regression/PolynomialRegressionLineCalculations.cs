using System;
using System.Linq;

namespace GrandeOmegaWebApp.Algorithms.Regression
{
    public class PolynomialRegressionLineCalculations
    {
        /// <summary>
        /// Calculates the values of the matrix based on the passed in data
        /// </summary>
        /// <param name="dataX">Array with doubles containing data</param>
        /// <param name="dataY">Array with doubles containing data</param>
        /// <returns>A matrix with the processed values</returns>
        public static double[,] CalculateMatrix(double[] dataX, double[] dataY)
        {
            //3 by 4 matrix containing the values to calculate a, a², a³
            var matrix = new double[3, 4];
            var n = dataX.Length;

            //0
            matrix[0, 0] = n;
            matrix[0, 1] = dataX.Sum();
            matrix[0, 2] = SummationOfDataSquared(dataX, 2);
            //1
            matrix[1, 0] = matrix[0, 1];
            matrix[1, 1] = matrix[0, 2];
            matrix[1, 2] = SummationOfDataSquared(dataX, 3);
            //2
            matrix[2, 0] = matrix[0, 2];
            matrix[2, 1] = matrix[1, 2];
            matrix[2, 2] = SummationOfDataSquared(dataX, 4);

            //Sum of dataY
            var sumY = dataY.Sum();
            //Sum of each element in dataX * dataY at the same index
            var sumXandY = dataX.Select((x, i) => x * dataY[i]).Sum();
            //Sum of each element in dataX squared * dataY at the same index
            var sumXandYsquared = dataX.Select((x, i) => Math.Pow(x, 2) * dataY[i]).Sum();

            //0,1,2,  3
            matrix[0, 3] = sumY;
            matrix[1, 3] = sumXandY;
            matrix[2, 3] = sumXandYsquared;

            return matrix;
        }

        /// <summary>
        /// Sum data after raising each value to the power given as parameter
        /// </summary>
        /// <param name="T">Array with doubles containing data</param>
        /// <param name="power">The power to raise it to (2,3,4)</param>
        /// <returns>The result of the sum and power calculations</returns>
        public static double SummationOfDataSquared(double[] T, double power)
        {
            return T.Sum(data => Math.Pow(data, power));
        }


        /// <summary>
        /// Uses Gaussian Elimination (Row Reduction) technique to solve the linear equasion
        /// </summary>
        /// <param name="matrix">The matrix to perform the Gaussian Elimination on</param>
        /// <returns>Double Array containing the a, a² and a³ values (in this order)</returns>
        //Written By @Travis J
        //https://stackoverflow.com/questions/8569473/how-can-i-solve-a-matrix-in-mathnet
        public static double[] CalculateGuassEliminationPt2(double[,] matrix)
        {
            int lead = 0, rowCount = 3, columnCount = 4;
            for (int r = 0; r < rowCount; r++)
            {
                if (columnCount <= lead) break;
                int i = r;
                while (matrix[i, lead] == 0)
                {
                    i++;
                    if (i == rowCount)
                    {
                        i = r;
                        lead++;
                        if (columnCount == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }
                for (int j = 0; j < columnCount; j++)
                {
                    double temp = matrix[r, j];
                    matrix[r, j] = matrix[i, j];
                    matrix[i, j] = temp;
                }
                double div = matrix[r, lead];
                for (int j = 0; j < columnCount; j++) matrix[r, j] /= div;
                for (int j = 0; j < rowCount; j++)
                {
                    if (j != r)
                    {
                        double sub = matrix[j, lead];
                        for (int k = 0; k < columnCount; k++) matrix[j, k] -= (sub * matrix[r, k]);
                    }
                }
                lead++;
            }

            return new double[] {matrix[0, 3], matrix[1, 3], matrix[2, 3]};
        }
    }
}