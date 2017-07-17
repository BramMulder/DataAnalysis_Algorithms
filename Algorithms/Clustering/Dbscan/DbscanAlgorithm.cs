using System;
using System.Linq;
using GrandeOmegaWebApp.Algorithms.Classification;

namespace GrandeOmegaWebApp.Algorithms.Clustering.Dbscan
{
    public class DbscanAlgorithm
    {
        /// <summary>
        /// Computes every point in the data to dertermine which cluster it belongs to and if it is and outlier
        /// </summary>
        /// <param name="points">The data you want to cluster (double array per point containing all dimensions of that point)</param>
        /// <param name="epsilon">The max amount of range around a point to still be considered to be part of a cluster (be a neighbour)</param>
        /// <param name="minPts">The minimum amount of points a set of points should have to be marked as a cluster</param>
        /// <returns></returns>
        public static DbscanPoint[] ComputeCluster(double[][] points, double epsilon, int minPts)
        {
            //Generate DbScanPoints from the given data
            var allPoints = new DbscanPoint[points.Length];
            for (int i = 0; i < allPoints.Length; i++)
            {
                allPoints[i] = new DbscanPoint(points[i]);
            }

            int clusterId = 0;
            //Visit every point in the data
            foreach (DbscanPoint p in allPoints)
            {
                if (p.IsVisited)
                    continue;
                p.IsVisited = true;
                //Execute the region query to retrieve all neighbours points to the given point
                DbscanPoint[] neighborPts = RegionQuery(allPoints, p, epsilon);

                //If the amount of neighbours from the query is less than the defined min amount of point, mark them as outliers
                if (neighborPts.Length < minPts)
                    p.ClusterId = -1;
                //Else visit every neighbour point that is in range of the epsilon
                else
                {
                    clusterId++;
                    ExpandCluster(allPoints, p, neighborPts, clusterId, epsilon, minPts);
                }
            }
            return allPoints;
        }

        /// <summary>
        /// Visit every neighbour point to find if there are any neighbours to the neighbours of the starting point
        /// </summary>
        /// <param name="allPoints">The given data for clustering</param>
        /// <param name="point">The point you want to check the neighbours of</param>
        /// <param name="neighborPts">The neighbours of the given point</param>
        /// <param name="clusterId">Current Id for a cluster</param>
        /// <param name="epsilon">The max amount of range around a point to still be considered to be part of a cluster (be a neighbour)</param>
        /// <param name="minPts">The minimum amount of points a set of points should have to be marked as a cluster</param>
        private static void ExpandCluster(DbscanPoint[] allPoints, DbscanPoint point, DbscanPoint[] neighborPts, int clusterId, double epsilon, int minPts)
        {
            //Visit each neighbours point if it hasn't been visited yet
            point.ClusterId = clusterId;
            for (int i = 0; i < neighborPts.Length; i++)
            {
                DbscanPoint pn = neighborPts[i];
                if (!pn.IsVisited)
                {
                    pn.IsVisited = true;
                    //Retrieve all neighbour points to this given point
                    var neighborPts2 = RegionQuery(allPoints, pn, epsilon);
                    //If the found points are more than the min amount of points, union it with the already selected points
                    if (neighborPts2.Length >= minPts)
                    {
                        neighborPts = neighborPts.Union(neighborPts2).ToArray();
                    }
                }
                //Assign the current clusterId to the point
                if (pn.ClusterId == 0)
                    pn.ClusterId = clusterId;
            }
        }

        /// <summary>
        /// Calculates the euclidean distance between the Core Point for every given point 
        /// </summary>
        /// <param name="allPoints">All points you want to know the distance of between the corePoint</param>
        /// <param name="corePoint">The point you are using to calculate the distances to</param>
        /// <param name="epsilon">The max amount of range around a point to still be considered to be part of a cluster (be a neighbour)</param>
        /// <returns></returns>
        private static DbscanPoint[] RegionQuery(DbscanPoint[] allPoints, DbscanPoint corePoint, double epsilon)
        {
            //If the length of Dimensions are not equal to eachother, return an empty array
            if (allPoints.Count(x => x.Coordinates.Count() != corePoint.Coordinates.Length) != 0)
            {
                return new DbscanPoint[0];
            }
            //Return all points where the euclidean distance is within the epsilon range
            return allPoints.Where(point => SimilarityCalculations.CalculateEuclideanDistance(point.Coordinates, corePoint.Coordinates) < epsilon).ToArray();
        }
    }
}