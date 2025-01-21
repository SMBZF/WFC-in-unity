using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborDefinition002 : MonoBehaviour, IColorDefinition
{
    private List<string> possibleNeighbors = new List<string> { "aba", "aba", "aaa", "aba" };

    public List<string> GetPossibleNeighbors()
    {
        return possibleNeighbors;
    }

    public void Rotate(int degree)
    {
        possibleNeighbors = new List<string> { "aba", "aba", "aaa", "aba" };

        switch (degree)
        {
            case 90:
                RotateNeighbors();
                break;

            case 180:
                RotateNeighbors();
                RotateNeighbors();
                break;

            case 270:
                RotateNeighbors();
                RotateNeighbors();
                RotateNeighbors();
                break;
        }
    }

    private void RotateNeighbors()
    {
        List<string> copy = new List<string>();
        for (int i = 0; i < possibleNeighbors.Count; i++)
        {
            if (i >= possibleNeighbors.Count - 1) copy.Add(possibleNeighbors[0]);
            else copy.Add(possibleNeighbors[i + 1]);
        }
        possibleNeighbors = copy;
    }
}
