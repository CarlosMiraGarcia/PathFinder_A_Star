using System;
using System.Collections.Generic;
using System.Linq;

namespace pathfinder
{
    public class Path
    {
        // I decided to use HashSet<T> instead of List<T> due to the better performance of HashSet<T>
        // when size of the collection is bigger than 5.
        public HashSet<Cave> HashCave { get; set; }
        public HashSet<Cave> OpenList { get; set; }
        public HashSet<Cave> ClosedList { get; set; }
        public CavesGrid CavesGrid { get; set; }
        public string SolutionString { get; set; }
        public Path(string pathFile)
        {
            // We create an instance of the class CavesGrid, and we pass a string
            // with the path for the text file
            this.CavesGrid = new CavesGrid(pathFile);
            // Initialise the HashSet HashCave
            this.HashCave = new HashSet<Cave>();
            // Calls the method SearchPath to start searching for the shortest path. We pass the position of the first cave
            SolutionString = SearchPath(1, CavesGrid.NumberCaves);
        }
        public string SearchPath(int startingCave, int finishingCave)
        {
            // We iterate throughout all the caves and assign them a null parent, the cave number, calculate the F cost,
            // and add them to the HashSet HashCave.
            for (int i = 1; i <= finishingCave; i++)
            {
                Cave Cave = GetCaveObject(i);

                Cave.Parent = null;
                Cave.CalculateFCost();
                Cave.CaveNumber = i;
                HashCave.Add(Cave);
            }
            // We then set the starting cave as the first element in the HashSet, and the finishing cave
            // as the last element in the HashSet
            Cave fromCave = HashCave.ElementAt(startingCave - 1);
            Cave toCave = HashCave.ElementAt(finishingCave - 1);
            // We set the boolean to true to be able to identify the goal cave
            toCave.IsLastCave = true;
            // We set the G Cost of the starting cave to 0, since it has no cost to travel from start cave
            // to start cave. We also calculate the H cost and the F cost.
            fromCave.GCost = 0;
            fromCave.HCost = Euclidean(fromCave, toCave);
            fromCave.CalculateFCost();

            // We initialise the OpenList and add the initial cave to it. We also initialise
            // the ClosedList
            OpenList = new HashSet<Cave> { fromCave };
            ClosedList = new HashSet<Cave>();

            // Here is where the algorithm takes place.
            // While the OpenList is not empty we will keep running the algorithm. If the OpenList is empty,
            // that means we didn't find a valid path from start to end cave.
            while (OpenList.Count > 0)
            {
                // We set the currentCave as the one with lowest f cost from the OpenList.
                // If the cave with the lowest f cost is the last cave, that means we found a valid path
                // so we call the function CalculatePath passing the last cave parameter in order to
                // track the path backwards, from the last cave to the initial cave
                Cave currentCave = GetLowestFCostCave(OpenList);
                if (currentCave.IsLastCave == true)
                {
                    return CalculatePath(toCave);
                }

                // If the currentCave is not the last cave, we remove it from the OpenList and
                // add it to the ClosedList, this way we won't have to visit that node again, unless
                // it proves to have a lower f cost than the current cave.
                OpenList.Remove(currentCave);
                ClosedList.Add(currentCave);

                // We search for all the caves that have a connection with the currentCave
                foreach (Cave connectedCave in GetConnectionsHashSet(currentCave))
                {
                    // If the connectedCave is already in the ClosedList, we check if the f cost of the stored version
                    // has a lower or equal f cost, then we discard the currentCave. If no better version has been found, then
                    // we remove it from the ClosedList and set it as a parent of currentCave.
                    if (ClosedList.Contains(connectedCave))
                    {
                        continue;
                    }

                    // We set the travellingCost as the currentCave G cost plus the Euclidean heuristic from the currentCave to the connectedCave
                    double travellingCost = currentCave.GCost + Euclidean(currentCave, connectedCave);

                    // We check if the travellingCost is inferior to the G cost of the connectedCave. If true, we update the G Cost of the connectedCave
                    // to the travellingCost, calculate the H Cost and F Cost of the conectedCave, and set
                    // the currentCave as a pconnectedCave's parent.
                    if (travellingCost < connectedCave.GCost)
                    {
                        connectedCave.GCost = travellingCost;
                        connectedCave.HCost = Euclidean(connectedCave, toCave);
                        connectedCave.CalculateFCost();
                        connectedCave.Parent = currentCave;

                        // If the OpenList doesn't contain the connectedCave, we add it to the OpenList.
                        if (!OpenList.Contains(connectedCave))
                        {
                            OpenList.Add(connectedCave);
                        }
                    }
                }
            }
            // If the OpenList is empty, we return 0 as we didn't find a valid path.
            return "0";
        }
        public static double Euclidean(Cave fromCave, Cave toCave)
        {
            // This method calculates the Euclidean distance between two cavers applying the following heuristic function
            return Math.Sqrt(((toCave.XAxys - fromCave.XAxys) * (toCave.XAxys - fromCave.XAxys)) + ((toCave.YAxys - fromCave.YAxys) * (toCave.YAxys - fromCave.YAxys)));
        }
        public string CalculatePath(Cave toCave)
        {
            // In this method we calculate the shortest path by adding the end cave to a List, then adding that cave's parent to the list,
            // then the parent's parent to the list, and so on until we arrive to the starting cave.
            List<Cave> SolutionList = new List<Cave>();
            SolutionList.Add(toCave);
            
            Cave currentCave = toCave;
            // We check if the parent of the current cave is null. If it is not null, we add the parent to the list, and set the parent
            // as the current cave. We keep doing this until the parent is null, which means that we have reached the starting cave.
            while (currentCave.Parent != null)
            {
                SolutionList.Add(currentCave.Parent);
                currentCave = currentCave.Parent;
            }

            // We reverse the list to get a path from the starting cave to the end cave.
            SolutionList.Reverse();
            // We create a new list of strings
            List<string> solution = new List<string>();
            // We iterate through the SolutionList and convert to string the number of the cave corresponding
            // to each of the entries into the solution string
            foreach (Cave cave in SolutionList)
            {
                solution.Add(Convert.ToString(cave.CaveNumber));
            }
            
            // We join the strings from the list solution and add an space between them
            string solutionString = string.Join(" ", solution);
            // We return the solutionString string
            return solutionString;
        }
        private static Cave GetLowestFCostCave(HashSet<Cave> caveHash)
        {
            Cave lowestFCostCave = caveHash.ElementAt(0);

            // To calculate the lowest f cost for the caves element we need to go throughout
            // the HashSet elements and update lowestFCostCave every time we find a lower f cost
            for (int i = 1; i < caveHash.Count; i++)
            {
                if (caveHash.ElementAt(i).FCost < lowestFCostCave.FCost)
                {
                    lowestFCostCave = caveHash.ElementAt(i);
                }
            }
            // We return the cave with the lowest f cost
            return lowestFCostCave;
        }
        public Cave GetCaveObject(int positionCave)
        {
            // In order to get the cave object for certain coordinates, we call the
            // jagged array CaveCoordinates and request the value of both columns for certain row positionCave.
            Cave cave = new Cave((CavesGrid.CaveCoordinates[positionCave - 1][0]), (CavesGrid.CaveCoordinates[positionCave - 1][1])); return cave;
        }
        private HashSet<Cave> GetConnectionsHashSet(Cave currentCave)
        {
            HashSet<Cave> caveConnections = new HashSet<Cave>();

            // We iterate i number of times through the CaveConnection jagged array,
            // i being the number of caves. Doing this, we can find out what caves can currentCave
            // move to, and we add them to the caveConnection HashSet.
            for (int i = 0; i < CavesGrid.NumberCaves; i++)
            {
                if (CavesGrid.CaveConnections[i][currentCave.CaveNumber - 1] == 1)
                {
                    caveConnections.Add(HashCave.ElementAt(i));
                }
            }
            return caveConnections;
        }
    }
}