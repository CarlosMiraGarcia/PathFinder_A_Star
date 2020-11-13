namespace pathfinder
{
    public class Cave
    {
        public double GCost { get; set; } = int.MaxValue;
        public double HCost { get; set; }
        public double FCost { get; set; }
        public double XAxys { get; set; }
        public double YAxys { get; set; }
        public bool IsLastCave { get; set; }
        public int CaveNumber { get; set; }
        public Cave Parent { get; set; }

        public Cave(double x, double y)
        {
            this.XAxys = (double)x;
            this.YAxys = (double)y;
        }
        public void CalculateFCost()
        {
            // We calculate the F cost by adding the cave's G cost to the cave's H cost
            FCost = GCost + HCost;
        }
    }
}