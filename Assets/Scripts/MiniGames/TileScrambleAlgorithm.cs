using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScrambleAlgorithm : MonoBehaviour {

    /*
        Summary: Generates an initial layout for an 8-tile slider game.
        This is done by creating a 2D array with the numbers 0-8.
        0 is an empty space and 1-8 are all tiles.
        The scrambling occurs by using a pseudo-random number generator to pick "moves" for the empty space.
        By swapping the empty space with one of the tiles immediately next to it, the algorithm emulates sliding a tile.
        This eliminates any chance of generating an unsolveable grid.
            -Jack Potter, June 2017
    */ 

    public static int[,] Scramble()
    {

        //Setting this to true generates grids 1 move away from being solved for testing TileSlider.cs
        bool DEBUGGING = true;

        int[,] grid = new int[,] //Grid starts in normal orientation with gap in lower left
        {
                {1,2,3},
                {4,5,6},
                {7,8,0},
        };

        if(DEBUGGING) //While debugging, this returns a solved grid 
        {
            return grid;
        }

        System.Random rand = new System.Random(); //Generates random movement

        for (int moves = 0; moves < 100; moves++) //Any number of moves will work here. Mathematically, it can't get harder than 31 moves away from solved
        {
            for (int i = 0; i < 3; i++) // i - vertical index
            {
                for (int j = 0; j < 3; j++) // j - horizontal index
                {
                    if (grid[i, j] == 0) //If the given spot is the empty space
                    {
                        List<int> potentialDirections = getSwapDirections(i, j); //Find out which ways are available to move
                        int index = rand.Next(potentialDirections.Count); //index is its own variable to make debugging and logging easier
                        int direction = potentialDirections[index]; //Chooses a random direction

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
                    }
                }
            }
        }

        return grid;
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
        if (i != 2) { neighbors.Add(2); }
        if (j != 0) { neighbors.Add(3); }
        if (j != 2) { neighbors.Add(4); }

        return neighbors;
    }

}
