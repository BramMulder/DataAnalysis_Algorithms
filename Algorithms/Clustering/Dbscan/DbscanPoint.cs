namespace GrandeOmegaWebApp.Algorithms.Clustering.Dbscan
{
    public class DbscanPoint
    {
        public bool IsVisited;
        public double[] Coordinates;
        public int ClusterId;

        public DbscanPoint(double[] coordinates)
        {
            Coordinates = coordinates;
            IsVisited = false;
            ClusterId = 0;
        }
    }
}