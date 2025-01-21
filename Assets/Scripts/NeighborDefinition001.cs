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
        // �����ھӵ����ƣ�������Ȩ�أ�
        List<string> neighbors = new List<string>();
        foreach (var neighbor in possibleNeighborsWithWeights)
        {
            neighbors.Add(neighbor.neighbor);
        }
        return neighbors;
    }

    public List<NeighborWithWeight> GetPossibleNeighborsWithWeights()
    {
        // �����������ھӺ�Ȩ����Ϣ
        return possibleNeighborsWithWeights;
    }

    public void Rotate(int degree)
    {
        // ģ����ת�ھ��߼���������Ը�������ʵ�ָ����ӵ���ת
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
            possibleNeighborsWithWeights.Reverse(); // �򵥷�ת
        }
        // ���������ת�߼���270�� �ȣ�
    }
}
