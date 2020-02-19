﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Movement;
using UnityEngine;

namespace Assets.Code.GridSystems
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private bool visualize = false;
        [SerializeField] private GridVisualizer visualizerPrefab;
        [SerializeField] private int gridSize = 10;
        private Grid[,] _grids;
        private List<GridVisualizer> _visualizers = new List<GridVisualizer>();
    
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
            var gridBlockers = FindObjectsOfType<GridBlocker>()
                .Select(e => GetGridFromPosition(e.transform.position))
                .ToList();
            
            var left = Mathf.Max(x - 1, 0);
            var right = Mathf.Min(x + 1, _grids.GetLength(0) - 1);
            var up = Mathf.Min(y + 1, _grids.GetLength(1) - 1);
            var down = Mathf.Max(y - 1, 0);
            var possibleGrids = new[] {_grids[left, y], _grids[right, y], _grids[x, up], _grids[x, down]};
            return possibleGrids.Where(possibleGrid => possibleGrid.Walkable && !gridBlockers.Contains(possibleGrid)).ToList();
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