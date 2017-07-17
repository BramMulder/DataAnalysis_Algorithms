namespace GrandeOmegaWebApp.Algorithms.Clustering
{
    public class KmeansVector
    {
        public KmeansVector(double[] coordinates)
        {
            Coordinates = coordinates;
            ShortestDistanceToCentroid = -1;
            ClusterId = -1;
        }

        public double[] Coordinates;
        public double ShortestDistanceToCentroid;
        public int ClusterId;
    }
}