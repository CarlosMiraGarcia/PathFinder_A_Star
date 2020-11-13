using System;
using System.IO;

namespace pathfinder
{
    public class CavesGrid
    {
        public int NumberCaves { get; set; }
        public int[][] CaveCoordinates { get; set; }
        public int[][] CaveConnections { get; set; }
        public int[] SplitNumbers { get; set; }

        public CavesGrid(string fileName)
        {
            // Read text from the input file
            var fileString = File.ReadAllText(@fileName + ".cav");

            // Find the number of caves
            FindingNumberOfCaves(fileString);

            // CalculateTotalSplits finds out how many different number inputs are in the string.
            // This is necessary in order to tell the method SplittingAndParsin how many
            // times it needs to iterate to split and parse all the numbers. Since coordinates are two times the number of caves,
            // and connections are equal to the number of caves squared, we need to add 1 which correspond to the number indicating
            // the number of caves
            var numberOfSplits = CalculateTotalSplits(NumberCaves);

            // We call the function SplittingAndParsing, providing the string and number of iterations
            SplitNumbers = SplittingAndParsing(fileString, numberOfSplits);
            // We initialise the arrays. The jagged arrays will have set the number of rows by the number of caves
            // To initialise the other arrays, we will use the same calculations as for splitting the string, depending whether
            // are coordinates or connections
            this.CaveCoordinates = new int[NumberCaves][];
            this.CaveConnections = new int[NumberCaves][];

            // Here we call the functions that create the coordinate string and connection string.
            // Then we will use those string arrays to populate both the connection and coordinate jagged arrays.
            //CreateCaveCoordinateString();
            //CreateCaveConectionsString();
            PrepareArrays();
        }

        public void FindingNumberOfCaves(string rawString)
        {
            // To outperform the string.Split and Int32.Parse method,I decided to implement it myself  .
            // In this function, we have to provide the raw string to find out the number of cavers

            // We iterate through the raw string, adding each number to the string NumberCaves
            // until we find a comma. Then we break the loop.
            for (int i = 0; i < rawString.Length; i++)
            {
                if (rawString[i] == ',')
                {
                    break;
                }
                // After each iteration, we multiply the current NumberCave by 10 so we can move the
                // current value to the tens, hundreds, and so on. Then we add it to the value of
                // the character found at i position on the rawString. The - '0' will give us the integer from the char
                NumberCaves = NumberCaves * 10 + (rawString[i] - '0');
            }
        }

        public int CalculateTotalSplits(int numberCaves)
        {
            // We need to find out how many different sets of integers are in the string.
            // We multiply by 2 the numberCaves value to obtain the number of coordinates, then we
            // square the numberCaves value to obtain the number of connections. The number 1 represents the set of integer with the value for
            // the number of caves, the first of the string.
            // We add the results to find out how many times we need to split the string.
            return (numberCaves * 2) + ((int)Math.Pow(numberCaves, 2)) + 1;
        }

        public int[] SplittingAndParsing(string rawString, int iterations)
        {
            // This method it is also created to outperform the string.Split and Int32.Parse method.
            // Providing the number of caves and iterations, we can split all the numbers and convert
            // them to int.

            int row = 0;
            int[] intArray = new int[iterations];

            // We iterate through the raw string, adding each number to the string NumberCaves,
            // Each time we find a comma, we increment the row value and use continue to avoid the
            // addition of the comma into the array
            for (int i = 0; i < rawString.Length; i++)
            {
                if (rawString[i] == ',')
                {
                    row++;
                    continue;
                }
                // After each iteration, we multiply the current value of intArray[row] by 10 so we can move the
                // current value to the tens, hundreds, and so on. Then we add it to the value of
                // the character found at i position on the rawString. The - '0' will give us the integer from the char
                intArray[row] = intArray[row] * 10 + (rawString[i] - '0');
            }
            // Once we iterate through the whole string, we return the array.
            return intArray;
        }

        private void PrepareArrays()
        {
            // This method calls the ToArrayMap, providing the array with all the set of integers,
            // the number of columns for the jagged array and the purpose of the array we are going to create.
            CaveCoordinates = ToArrayMap(SplitNumbers, 2, "coordinates");
            CaveConnections = ToArrayMap(SplitNumbers, NumberCaves, "connections");
        }

        public int[][] ToArrayMap(int[] splitArray, int columns, string typeMap)
        {
            int index = new int();
            // First, we set index to different values depending if we are going to calculate
            // the coordinates or connections.
            if (typeMap == "coordinates")
            {
                index = 1;
            }
            else if (typeMap == "connections")
            {
                index = 1 + (NumberCaves * 2);
            }

            int[][] caveArray = new int[NumberCaves][];

            // With a nested for loop, we iterate through all the rows and columns,
            // adding the appropriate integer from the splitArray array into the new jagged array.
            // The value index will indicate where about in the splitArray array we need to start.
            for (int i = 0; i < NumberCaves; i++)
            {
                int[] row = new int[columns];
                for (int j = 0; j < columns; j++)
                {
                    row[j] = splitArray[index];
                    index += 1;
                }
                caveArray[i] = row;
            }
            return caveArray;
        }
    }
}