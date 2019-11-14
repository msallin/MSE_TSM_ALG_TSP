﻿using System.Collections.Generic;
using System.Linq;
using TspLib.Metaheuristics.Interfaces;

namespace TspLib.Metaheuristics
{
    public class GreedyInsertion : ISolver
    {
        public string Id => "GreedyInsertion";

        public IEnumerable<Point> Solve(IEnumerable<Point> pointsInput)
        {
            var points = pointsInput
                .OrderBy(p => p.Id)
                .ToArray();
            
            //Array with the indices of the next nodes
            int[] nextIndices = new int[points.Count()];

            //Initial partial tour 0 -> 1 -> 0
            nextIndices[0] = 1;

            //Find the best position to insert for each remaining point
            for (int i = 2; i < points.Count(); i++)
            {
                double lowestDistanceIncrease = double.PositiveInfinity;
                int lowestDistanceIncreaseIdx = -1;

                for (int j = 0; j < i; j++)
                {
                    //Increased cost of tour if point i is inserted in place j
                    double distanceIncrease =
                        points[j].CalculateDistanceTo(points[i]) 
                        + points[i].CalculateDistanceTo(points[nextIndices[j]]) 
                        - points[j].CalculateDistanceTo(points[nextIndices[j]]);

                    if (distanceIncrease < lowestDistanceIncrease)
                    {
                        lowestDistanceIncrease = distanceIncrease;
                        lowestDistanceIncreaseIdx = j;
                    }
                }

                nextIndices[i] = nextIndices[lowestDistanceIncreaseIdx];
                nextIndices[lowestDistanceIncreaseIdx] = i;
            }

            //Walk along next indices to build solution.
            var solution = new List<Point>();
            int index = 0;
            for (int i = 0; i < points.Count(); i++)
            {
                solution.Add(points[index]);
                index = nextIndices[index];
            }

            return solution;
        }
    }
}
