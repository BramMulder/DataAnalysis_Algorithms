using System.Collections.Generic;
using System.Linq;

namespace GrandeOmegaWebApp.Algorithms.Classification
{
    public class KNearestNeighbour
    {
        private readonly Dictionary<int, double[]> _nearestNeighbours;
        private readonly int _maxAmountOfNearestNeighbours;
        private readonly Dictionary<int, double> _euclideanDistanceValues;
        private double _highestEuclideanDistanceOfNeighbours;

        /// <summary>
        /// Constructor of KNN Algorithm
        /// Sets the starting values required to execute the algorithm
        /// </summary>
        /// <param name="maxAmountOfNearestNeighbours">
        /// Determines the maximum amount of nearest neighbours that the list can contain at any time during the execution of the algorithm 
        /// </param>
        public KNearestNeighbour(int maxAmountOfNearestNeighbours)
        {
            //Initialize
            _nearestNeighbours = new Dictionary<int, double[]>(maxAmountOfNearestNeighbours);
            _maxAmountOfNearestNeighbours = maxAmountOfNearestNeighbours;
            _euclideanDistanceValues = new Dictionary<int, double>();
            _highestEuclideanDistanceOfNeighbours = 0.0;
        }

        /// <summary>
        /// Manages the main functionality of the algorithm, call all functions with logic
        /// </summary>
        /// <param name="targetStudentData">Student of which you want to get the nearest neighbours</param>
        /// <param name="studentData">Data of all students</param>
        /// <returns>KeyValueObject array containing all nearest neighbours for the given student</returns>
        public KeyValueObject[] GetNearestNeighbours(double[] targetStudentData, Dictionary<int, double[]> studentData)
        {
            foreach (var student in studentData)
            {
                //Calculate the similarity between the individual and the neighbour
                var similarity = CalculateSimilarity(targetStudentData, student.Value);

                //If the euclidean distance could not be calculated due to an error
                if (similarity <= 0)
                    return null;

                //Add the similarity value to a dictornary for later usage (Student key, similarity)
                _euclideanDistanceValues.Add(student.Key, similarity);
                //If the similairty is larger or equal to the threshhold and the individual has the rated the same items 
                ProcessNeighbour(student.Key, student.Value, similarity);
            }

            return (from i in _euclideanDistanceValues
                    join n in _nearestNeighbours
                    on i.Key equals n.Key
                    select new KeyValueObject { Key = i.Key, EDistance = i.Value }).ToArray();
        }

        /// <summary>
        /// Calls method which calculates the Euclidean distance between the inserted parameters
        /// </summary>
        /// <param name="dataIndividual">Individual for which you want to know the distance to the given neighbour </param>
        /// <param name="dataNeighbour">The given neighbour </param>
        /// <returns>Double value of the Euclidean Distance between there parameters</returns>
        private double CalculateSimilarity(double[] dataIndividual, double[] dataNeighbour)
        {
            var eDistance = SimilarityCalculations.CalculateEuclideanDistance(dataIndividual, dataNeighbour);

            return eDistance;
        }

        /// <summary>
        /// Inserts the neighbours if the list isnt full yet, else checks if the distance is lower than the last value of the neighbours in the nearest neighbour list, 
        /// if so it replaces it and updates the highest value in the list
        /// </summary>
        /// <param name="neighbourId">Id of the neighbour you want to process</param>
        /// <param name="neighbourData">Data of the neighbour you want to process</param>
        /// <param name="eDistance">The Eclidean Distance the target student (Calculated in CalculateSimilarity method)</param>
        private void ProcessNeighbour(int neighbourId, double[] neighbourData, double eDistance)
        {
            //If the Euclidean Distance is larger than the highest Euclidean Distance in the nearest neighbour list, move on to the next student
            if (eDistance > _highestEuclideanDistanceOfNeighbours && _highestEuclideanDistanceOfNeighbours != 0.0)
            {
                return;
            }

            //If the NearestNeighbours List isn't 'full' yet, add the student
            if (_nearestNeighbours.Count < _maxAmountOfNearestNeighbours)
            {
                _nearestNeighbours.Add(neighbourId, neighbourData);

            }
            //If the list is 'full'
            else if (_nearestNeighbours.Count == _maxAmountOfNearestNeighbours)
            {
                //Find the key of the student with the largest Euclidean Distance in the nearest neighbour list
                var lastKey = _euclideanDistanceValues.OrderBy(x => x.Value).Last(x => _nearestNeighbours.ContainsKey(x.Key)).Key;
                //Check if this student has a better Euclidean Distance than the values in the list
                if (_euclideanDistanceValues[lastKey] > eDistance)
                {
                    //Remove the item with the lowest Euclidean Distance from the Dictionary
                    _nearestNeighbours.Remove(lastKey);
                    //Add new student with a better Euclidean Distance
                    _nearestNeighbours.Add(neighbourId, neighbourData);
                    //Update the threshhold by taking the Euclidean Distance of the last student in the nearest neighbour list
                    _highestEuclideanDistanceOfNeighbours = _euclideanDistanceValues.OrderBy(x => x.Value).Last(x => _nearestNeighbours.ContainsKey(x.Key)).Value;
                }
            }
        }
    }

    public class KeyValueObject
    {
        public int Key { get; set; }
        public double EDistance { get; set; }
    }
}