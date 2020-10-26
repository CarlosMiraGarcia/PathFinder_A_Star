namespace pathfinder
{
    public class Cave
    {
        public int GCost { get; set; } = int.MaxValue;
        public int HCost { get; set; }
        public int FCost { get; set; }
        public int XAxys { get; set; }
        public int YAxys { get; set; }
        public bool IsLastCave { get; set; }
        public int CaveNumber { get; set; }
        public Cave Parent { get; set; }

        public Cave(int x, int y)
        {
            this.XAxys = (int)x;
            this.YAxys = (int)y;
        }
        public void CalculateFCost()
        {
            // We calculate the F cost by adding the cave's G cost to the cave's H cost
            FCost = GCost + HCost;
        }
    }
}