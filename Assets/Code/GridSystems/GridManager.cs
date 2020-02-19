using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.GridSystems;
using Code.Movement;
using UnityEngine;
using Grid = Code.GridSystems.Grid;

namespace Code.GridSystems
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private bool visualize = false;
        [SerializeField] private GridVisualizer visualizerPrefab;
        [SerializeField] private int gridSize = 10;
        private Grid[,] _grids;
        private List<GridVisualizer> _visualizers = new List<GridVisualizer>();
        private List<Grid> BlockedGrids
        {
            get
            {
                return FindObjectsOfType<GridBlocker>()
                    .Select(e => GetGridFromPosition(e.transform.position))
                    .ToList();
            }
        }
        public Grid GetGridFromPosition(Vector3 position)
        {
            var x = (int) position.x;
            var y = (int) position.z;
            if (_grids.GetLength(0) < x || x < 0 ||
                _grids.GetLength(1) < y || y < 0)
                throw new Exception("Position out of grid range!");

            return _grids[x, y];
        }

        public GridVisualizer GetGridVisualizerFromGrid(Grid grid)
        {
            return _visualizers.FirstOrDefault(x => x.TargetGrid == grid);
        }

        public void ClearGridCalculations()
        {
            foreach (var visualizer in _visualizers)
                visualizer.ClearStatus();
            
            CalculateAdjacent();
        }

        public IEnumerable<Grid> GetNearestGridsToTargetGrid(Grid target)
        {
            var distance = Mathf.Infinity;
            var set = new HashSet<Grid>();
            foreach (var grid in _grids)
            {
                if(grid==target) continue;
                var dist = Vector2.Distance(new Vector2(target.X, target.Y), new Vector2(grid.X, grid.Y));
                if (dist < distance)
                {
                    distance = dist;
                }
            }

            foreach (var grid in _grids)
            {
                if(grid==target) continue;
                var dist = Vector2.Distance(new Vector2(target.X, target.Y), new Vector2(grid.X, grid.Y));
                if (dist <= distance)
                    set.Add(grid);
            }

            return set.ToArray();
        }

        private void Awake()
        {
            GenerateGrid(gridSize);
            CalculateAdjacent();
            InitVisualizers();
        }

        private void GenerateGrid(int size)
        {
            _grids = new Grid[size, size];
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    _grids[i, j] = new Grid(i, j);
                }
            }
        }

        private void CalculateAdjacent()
        {
            for (var i = 0; i < _grids.GetLength(0); i++)
            {
                for (var j = 0; j < _grids.GetLength(1); j++)
                {
                    _grids[i, j].Adjents = GetAdjacent(i, j);
                }
            }
        }

        private List<Grid> GetAdjacent(int x, int y)
        {
            var left = Mathf.Max(x - 1, 0);
            var right = Mathf.Min(x + 1, _grids.GetLength(0) - 1);
            var up = Mathf.Min(y + 1, _grids.GetLength(1) - 1);
            var down = Mathf.Max(y - 1, 0);
            var possibleGrids = new[] {_grids[left, y], _grids[right, y], _grids[x, up], _grids[x, down]};
            return possibleGrids.Where(possibleGrid => possibleGrid.Walkable && !BlockedGrids.Contains(possibleGrid)).ToList();
        }

        private void InitVisualizers()
        {
            foreach (var grid in _grids)
            {
                var visualizer = Instantiate(visualizerPrefab, new Vector3(grid.X, 0, grid.Y), Quaternion.identity);
                _visualizers.Add(visualizer);
                visualizer.TargetGrid = grid;
            }
        }
    }
}