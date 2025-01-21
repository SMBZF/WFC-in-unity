using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class NeighborDefinition001 : MonoBehaviour, IColorDefinition
{
    [System.Serializable]
    public class NeighborWithWeight
    {
        public string neighbor;
        public int weight;

        public NeighborWithWeight(string neighbor, int weight)
        {
            this.neighbor = neighbor;
            this.weight = weight;
        }
    }

    [SerializeField]
    private List<NeighborWithWeight> possibleNeighborsWithWeights = new List<NeighborWithWeight>
    {
        new NeighborWithWeight("aba", 1),
        new NeighborWithWeight("aba", 1),
        new NeighborWithWeight("aba", 1),
        new NeighborWithWeight("aba", 1)
    };

    public List<string> GetPossibleNeighbors()
    {
        // 返回邻居的名称（不包含权重）
        List<string> neighbors = new List<string>();
        foreach (var neighbor in possibleNeighborsWithWeights)
        {
            neighbors.Add(neighbor.neighbor);
        }
        return neighbors;
    }

    public List<NeighborWithWeight> GetPossibleNeighborsWithWeights()
    {
        // 返回完整的邻居和权重信息
        return possibleNeighborsWithWeights;
    }

    public void Rotate(int degree)
    {
        // 模拟旋转邻居逻辑，这里可以根据需求实现更复杂的旋转
        if (degree == 90)
        {
            possibleNeighborsWithWeights = new List<NeighborWithWeight>
            {
                new NeighborWithWeight("aba", 1),
                new NeighborWithWeight("aba", 1),
                new NeighborWithWeight("aba", 1),
                new NeighborWithWeight("aba", 1)
            };
        }
        else if (degree == 180)
        {
            possibleNeighborsWithWeights.Reverse(); // 简单反转
        }
        // 添加其他旋转逻辑（270° 等）
    }
}
