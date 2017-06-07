using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliderScrambleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] grid = new int[,] //Grid starts in normal orientation with gap in lower left
            {
                {1,2,3},
                {4,5,6},
                {7,8,0},
            };

            System.Random rand = new System.Random(); //Generates random movement

            //These lines are just here for debugging
            //print(grid); 
            //Console.ReadLine();

            for(int moves = 0; moves < 10; moves++) //Any number of moves will work here. Mathematically, it can't get harder than 31 moves away from solved
            {
                for(int i = 0; i < 3; i++) // i - vertical index
                {
                    for(int j = 0; j < 3; j++) // j - horizontal index
                    {
                        if(grid[i,j] == 0) //If the given spot is the empty space
                        {
                            List<int> potentialDirections = getSwapDirections(i, j); //Find out which ways are available to move
                            int index = rand.Next(potentialDirections.Count); //index is its own variable to make debugging and logging easier
                            int direction = potentialDirections[index]; //Chooses a random direction

                            //Console.WriteLine("COUNT :" + potentialDirections.Count + " INDEX: " + index + " DIRECTION: " + direction); //Debugging line

                            int temp; //Used while swapping

                            switch (direction) //Swaps one tile at a time with the empty space
                            {
                                case 1: //UP
                                    temp = grid[i - 1, j];
                                    grid[i - 1, j] = 0;
                                    grid[i, j] = temp;
                                    break;
                                case 2: //DOWN
                                    temp = grid[i + 1, j];
                                    grid[i + 1, j] = 0;
                                    grid[i, j] = temp;
                                    break;
                                case 3: //LEFT
                                    temp = grid[i, j - 1];
                                    grid[i, j - 1] = 0;
                                    grid[i, j] = temp;
                                    break;
                                case 4: //RIGHT
                                    temp = grid[i, j + 1];
                                    grid[i, j + 1] = 0;
                                    grid[i, j] = temp;
                                    break;

                            }

                            //Uncomment if you want to watch the boards zoom by with each change
                            //Console.Write("\n");
                            //print(grid);
                        }
                    }
                }
            }

            print(grid);
            Console.ReadLine();

        }

        static void print(int[,] grid) //Everything here is self-explanatory
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(grid[i, j]);
                }
                Console.Write("\n");
            }
        }

        static List<int> getSwapDirections(int i, int j) //Returns a list of available directions to move
        {
            /*
            List will contain:
            1 - move up
            2 - move down
            3 - move left
            4 - move right
            */

            List<int> neighbors = new List<int>(0); //Create list containing nothing

            if (i != 0) { neighbors.Add(1); }
            if(i != 2) { neighbors.Add(2); }
            if(j != 0) { neighbors.Add(3); }
            if(j != 2) { neighbors.Add(4); }

            return neighbors;
        }
    }
}
