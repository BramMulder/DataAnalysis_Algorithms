using System;
using System.Collections.Generic;
using System.Linq;

namespace GrandeOmegaWebApp.Algorithms.Clustering
{
    public class KmeansAlgorithm
    {
        private List<KmeansVector> _vectors = new List<KmeansVector>();
        private List<KmeansVector> _centroids = new List<KmeansVector>();
        private List<KmeansVector> _prevCentroids = new List<KmeansVector>();
        private readonly int _amountOfClusters;
        private readonly int _maxAmountOfIterations;
        private readonly int _vectorLength;

        /// <summary>
        /// Instantiates values to run the Kmeans Algorithm
        /// </summary>
        /// <param name="amountOfClusters">The Amount of Clusters</param>
        /// <param name="maxAmountOfIterations">The Max amount of iterations of the recomputing of the centroid</param>
        /// <param name="vectorLength">The amount of dimensions of the data</param>
        public KmeansAlgorithm(int amountOfClusters, int maxAmountOfIterations, int vectorLength)
        {
            _amountOfClusters = amountOfClusters;
            _maxAmountOfIterations = maxAmountOfIterations;
            _vectorLength = vectorLength;
        }

        /// <summary>
        /// Calls the methods to run the algorithm
        /// </summary>
        /// <param name="data">The data you want to cluster</param>
        /// <returns>A list with the clustered vectors</returns>
        public List<KmeansVector> RunAlgorithm(double[][] data)
        {
            //Ensure that the data is not empty
            if (!data.Any())
                return null;

            //Clear previous values
            _centroids.Clear();
            _vectors.Clear();

            CreateVectors(data);

            PickCentroids();

            ComputeDistanceToCentroids();

            for (int iteration = 0; iteration < _maxAmountOfIterations; iteration++)
            {
                _prevCentroids = _centroids;
                RecomputeCentroids();
                ComputeDistanceToCentroids();
                //If centroid didn't change since last iteration
                if (CheckIfCentroidsChanged())
                {
                    break;
                }
            }

            return _vectors;
        }

        /// <summary>
        /// Generates vectors from the data
        /// </summary>
        /// <param name="fields">The data you want to cluster</param>
        private void CreateVectors(double[][] fields)
        {
            foreach (var field in fields)
            {
                _vectors.Add(new KmeansVector(field));
            }
        }

        /// <summary>
        /// Randomly selects centroids for every cluster
        /// </summary>
        private void PickCentroids()
        {
            Random random = new Random();
            for (int amountOfCentroids = 0; amountOfCentroids < _amountOfClusters; amountOfCentroids++)
            {
                var r = random.Next(_vectors.Count);
                var vector = _vectors[r];

                //Select a new centroid, if the picked one is already in the _centroid list
                while (_centroids.Contains(vector))
                {
                    r = random.Next(_vectors.Count);
                    vector = _vectors[r];
                }
                _centroids.Add(vector);
            }
        }

        /// <summary>
        /// Computes the distance of the vectors to the centroids
        /// </summary>
        private void ComputeDistanceToCentroids()
        {
            foreach (KmeansVector vector in _vectors)
            {
                FindClosestCentroid(vector);

                //No valid centroid found
                if (vector.ClusterId == -1)
                {
                    Console.WriteLine("No valid centroid found for vector: " + vector);
                    break;
                }
            }
        }

        /// <summary>
        /// Uses the Euclidean Distance to return the nearest centroid to the given vector
        /// </summary>
        /// <param name="vector">The vector to calculate the euclidean distance for</param>
        private void FindClosestCentroid(KmeansVector vector)
        {
            double shortestDistance = -1;
            //Find the shortest distance from the given vector to a centroid
            for (int centroidId = 0; centroidId < _centroids.Count; centroidId++)
            {
                double distance = 0;

                //For every centroid dimension - Subtract every coordinate from the centroid
                for (int dimension = 0; dimension < _centroids[centroidId].Coordinates.Length; dimension++)
                {
                    double delta = _centroids[centroidId].Coordinates[dimension] - vector.Coordinates[dimension];
                    distance = distance + Math.Pow(delta, 2);
                }
                distance = Math.Sqrt(distance);

                if (shortestDistance < 0 || distance < shortestDistance)
                {
                    shortestDistance = distance;
                    vector.ShortestDistanceToCentroid = distance;
                    vector.ClusterId = centroidId;
                }
            }
        }

        /// <summary>
        /// Recomputes the centroids per cluster
        /// </summary>
        private void RecomputeCentroids()
        {
            List<KmeansVector> newCentroids = new List<KmeansVector>();

            //For every cluster/centroid
            for (int clusterIndex = 0; clusterIndex < _amountOfClusters; clusterIndex++)
            {
                newCentroids.Add(CalculateCentroidPerCluster(clusterIndex));
            }
            _centroids = newCentroids;
        }

        /// <summary>
        /// Calculates a new centroid for every cluster based on the data of the points in the cluster
        /// </summary>
        /// <param name="clusterIndex">The current clusterId</param>
        /// <returns>A new centroid for the given cluster</returns>
        private KmeansVector CalculateCentroidPerCluster(int clusterIndex)
        {
            KmeansVector newCentroid = new KmeansVector(new double[_vectorLength]);
            //Total amount of vectors in a cluster
            int totalAmountVectors = 0;

            //Sum all the vectors with the same cluster ID
            foreach (KmeansVector vector in _vectors)
            {
                if (vector.ClusterId != clusterIndex) continue;
                //Add every dimension of the selected vector to the existing vector
                for (int dimension = 0; dimension < vector.Coordinates.Length; dimension++)
                {
                    newCentroid.Coordinates[dimension] = newCentroid.Coordinates[dimension] + vector.Coordinates[dimension];
                }
                totalAmountVectors++;
            }

            for (int s = 0; s < newCentroid.Coordinates.Length; s++)
            {
                newCentroid.Coordinates[s] = newCentroid.Coordinates[s] / totalAmountVectors;
            }

            return newCentroid;
        }

        /// <summary>
        /// Checks if centroids changed since to last operation
        /// </summary>
        /// <returns>
        /// True, if the centroids have NOT changed
        /// False, if the centroids have changed
        /// </returns>
        private bool CheckIfCentroidsChanged()
        {
            var xCoordinates = _centroids.Where(x => _prevCentroids.Any(y => y.Coordinates[0] == x.Coordinates[0]));
            var yCoordinates = _centroids.Where(x => _prevCentroids.Any(y => y.Coordinates[1] == x.Coordinates[1]));
            return (xCoordinates.Count() == _amountOfClusters) && (yCoordinates.Count() == _amountOfClusters);
        }

        /// <summary>
        /// Calculates the sum square errors for every vector
        /// </summary>
        /// <returns>The SSE value</returns>
        private double CalculateSumSquaredErrors()
        {
            double totalSse = 0;

            if (_vectors.Count == 0)
                return -1;

            foreach (var vector in _vectors)
            {
                var squaredShortestDistance = Math.Pow(vector.ShortestDistanceToCentroid, 2);
                totalSse = totalSse + squaredShortestDistance;
            }

            return totalSse;
        }
    }
}