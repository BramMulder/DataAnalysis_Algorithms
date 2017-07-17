using System;
using System.Linq;

namespace GrandeOmegaWebApp.Algorithms.Correlation
{
    public class SpearmansRho
    {
        private readonly double[] _dataArray1;
        private readonly double[] _dataArray2;

        /// <summary>
        /// Constructor, Gets the ranking for each data array passed into it
        /// </summary>
        /// <param name="dataArray1">Array with doubles containing data</param>
        /// <param name="dataArray2">Array with doubles containing data</param>
        public SpearmansRho(double[] dataArray1, double[] dataArray2)
        {
            _dataArray1 = GetRanking(dataArray1);
            _dataArray2 = GetRanking(dataArray2);
        }

        /// <summary>
        /// Ranks the data based on the values given
        /// </summary>
        /// <param name="values">Data you want to rank</param>
        /// <returns>Ranked Data</returns>
        private static double[] GetRanking(double[] values)
        {
            //Creates object out of data, groups them by value
            var groupedValues = values.OrderByDescending(n => n)
                                      .Select((val, i) => new { Value = val, IndexedRank = i + 1 })
                                      .GroupBy(i => i.Value);

            //Ranks values
            double[] rankings = (from n in values
                                 join grp in groupedValues on n equals grp.Key
                                 select grp.Average(g => g.IndexedRank)).ToArray();

            return rankings;
        }

        /// <summary>
        /// Execute Calculations to calculate the Spearman's Rho Coefficient
        /// </summary>
        /// <returns>double value of the Spearman's Rho Coefficient</returns>
        public double CalculateSpearmansRho()
        {
            if (_dataArray1.Length != _dataArray2.Length)
            {
                return 0.0;
            }

            var total = 0.0;
            var n = _dataArray1.Length;

            for (int i = 0; i < n; i++)
            {
                total += Math.Pow(_dataArray1[i] - _dataArray2[i], 2);
            }

            var sValue = 1 - (6 * total) / (n * (Math.Pow(n, 2) - 1));

            return sValue;

        }
    }
}