using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    [System.Serializable]
    public class WeightedPart
    {
        public GameObject prefab;
        public int weight;

        public WeightedPart(GameObject prefab, int weight)
        {
            this.prefab = prefab;
            this.weight = weight;
        }
    }

    [SerializeField]
    private GameObject imageBorder;

    [SerializeField]
    private List<WeightedPart> initialParts; // 用于在编辑器中设置初始部件及权重

    private float partSize;

    private List<WeightedPart> partList = new List<WeightedPart>();

    private GridPosition[,] grid;

    private int dim;

    private void Start()
    {
        dim = Data.dim;

        // 初始化 partList，包括旋转的实例
        foreach (var part in initialParts)
        {
            partList.Add(part);

            for (int rotation = 90; rotation < 360; rotation += 90)
            {
                GameObject rotatedPart = Instantiate(part.prefab);
                rotatedPart.transform.Rotate(0, 0, rotation);
                rotatedPart.GetComponent<IColorDefinition>().Rotate(rotation);
                partList.Add(new WeightedPart(rotatedPart, part.weight));
                rotatedPart.SetActive(false); // 隐藏旋转的实例
            }
        }

        grid = new GridPosition[dim, dim];
        partSize = imageBorder.transform.localScale.x / dim;

        float startX = -imageBorder.transform.localScale.x / 2f + partSize / 2f;
        float startY = imageBorder.transform.localScale.y / 2f - partSize / 2f;

        float x = startX;
        float y = startY;

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                grid[i, j] = new GridPosition(x, y, partSize, j, i, partList);
                x += partSize;
            }
            y -= partSize;
            x = startX;
        }

        Random.InitState(System.DateTime.Now.Millisecond);
    }

    private void Update()
    {
        int min = int.MaxValue;
        List<GridPosition> nextPositions = new List<GridPosition>();
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (grid[i, j].possibleParts.Count <= min && !grid[i, j].filled)
                {
                    if (grid[i, j].possibleParts.Count < min)
                    {
                        nextPositions.Clear();
                        min = grid[i, j].possibleParts.Count;
                    }
                    nextPositions.Add(grid[i, j]);
                }
            }
        }

        if (nextPositions.Count > 0)
        {
            GridPosition nextPosition = nextPositions[Random.Range(0, nextPositions.Count)];
            if (nextPosition != null)
            {
                nextPosition.filled = true;
                GameObject next = Instantiate(GetWeightedRandomPart(nextPosition.possibleParts)); // 权重随机选择

                next.SetActive(true);

                next.GetComponent<IColorDefinition>().Rotate((int)next.transform.rotation.eulerAngles.z);

                next.transform.localPosition = new Vector3(nextPosition.x, nextPosition.y, 0);
                next.transform.localScale = new Vector3(partSize, partSize, 1);
                nextPosition.colorDefinitions = next.GetComponent<IColorDefinition>().GetPossibleNeighbors();

                UpdatePossibleNeighbors(nextPosition);
            }
        }
    }

    private GameObject GetWeightedRandomPart(List<WeightedPart> possibleParts)
    {
        int totalWeight = 0;

        foreach (var part in possibleParts)
        {
            totalWeight += part.weight;
        }

        int randomValue = Random.Range(0, totalWeight);

        foreach (var part in possibleParts)
        {
            if (randomValue < part.weight)
            {
                return part.prefab;
            }
            randomValue -= part.weight;
        }

        return possibleParts[0].prefab; // 默认返回第一个
    }

    private void UpdatePossibleNeighbors(GridPosition position)
    {
        List<WeightedPart> newPossibleNeighbors = new List<WeightedPart>();

        if (position.gridY - 1 >= 0 && !grid[position.gridY - 1, position.gridX].filled)
        {
            foreach (var part in partList)
            {
                if (!position.colorDefinitions[0].Equals(part.prefab.GetComponent<IColorDefinition>().GetPossibleNeighbors()[2]))
                {
                    if (!newPossibleNeighbors.Contains(part))
                        newPossibleNeighbors.Add(part);
                }
            }

            grid[position.gridY - 1, position.gridX].possibleParts.RemoveAll(p => newPossibleNeighbors.Contains(p));
        }

        newPossibleNeighbors.Clear();
        if (position.gridY + 1 < dim && !grid[position.gridY + 1, position.gridX].filled)
        {
            foreach (var part in partList)
            {
                if (!position.colorDefinitions[2].Equals(part.prefab.GetComponent<IColorDefinition>().GetPossibleNeighbors()[0]))
                {
                    if (!newPossibleNeighbors.Contains(part))
                        newPossibleNeighbors.Add(part);
                }
            }

            grid[position.gridY + 1, position.gridX].possibleParts.RemoveAll(p => newPossibleNeighbors.Contains(p));
        }

        newPossibleNeighbors.Clear();
        if (position.gridX + 1 < dim && !grid[position.gridY, position.gridX + 1].filled)
        {
            foreach (var part in partList)
            {
                if (!position.colorDefinitions[1].Equals(part.prefab.GetComponent<IColorDefinition>().GetPossibleNeighbors()[3]))
                {
                    if (!newPossibleNeighbors.Contains(part))
                        newPossibleNeighbors.Add(part);
                }
            }

            grid[position.gridY, position.gridX + 1].possibleParts.RemoveAll(p => newPossibleNeighbors.Contains(p));
        }

        newPossibleNeighbors.Clear();
        if (position.gridX - 1 >= 0 && !grid[position.gridY, position.gridX - 1].filled)
        {
            foreach (var part in partList)
            {
                if (!position.colorDefinitions[3].Equals(part.prefab.GetComponent<IColorDefinition>().GetPossibleNeighbors()[1]))
                {
                    if (!newPossibleNeighbors.Contains(part))
                        newPossibleNeighbors.Add(part);
                }
            }

            grid[position.gridY, position.gridX - 1].possibleParts.RemoveAll(p => newPossibleNeighbors.Contains(p));
        }
    }
}
