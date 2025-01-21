using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborDefinition005 : MonoBehaviour, IColorDefinition
{
    private List<string> possibleNeighbors = new List<string> { "aaa", "aaa", "aaa", "aaa" };

    public List<string> GetPossibleNeighbors()
    {
        return possibleNeighbors;
    }

    public void Rotate(int degree)
    {
        possibleNeighbors = new List<string> { "aaa", "aaa", "aaa", "aaa" };
    }
}
