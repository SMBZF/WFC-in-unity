using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal class GridPosition
    {
        public int gridX { get; private set; }
        public int gridY { get; private set; }

        public float x { get; private set; }
        public float y { get; private set; }
        public bool filled { get; set; } = false;
        public float scale { get; private set; }

        public List<string> colorDefinitions { get; set; }
        public List<WaveFunctionCollapse.WeightedPart> possibleParts { get; set; } // 修改类型

        public GridPosition(float x, float y, float scale, int gridX, int gridY, List<WaveFunctionCollapse.WeightedPart> possibleParts)
        {
            this.possibleParts = new List<WaveFunctionCollapse.WeightedPart>(possibleParts);
            this.x = x;
            this.y = y;
            this.scale = scale;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }
}
