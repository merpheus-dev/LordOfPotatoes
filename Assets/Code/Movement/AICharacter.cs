using System;
using System.Linq;
using Code.TurnSystems;
using UnityEngine;
using UnityEngine.AI;
using Grid = Code.GridSystems.Grid;

namespace Code.Movement
{
    public class AICharacter : MovableCharacter
    {
        [SerializeField] private TurnManager _turnManager;
        private MovableCharacter target;

        private void Update()
        {
            if (!isAuthForTurn) return;
            if (!Moving)
            {
                FindSelectableTiles();
                FindNearestTarget();
                FindPathToTarget();
            }
            else
            {
                PerformMovement();
            }
        }

        private void FindNearestTarget()
        {
            target = _turnManager.GetPlayerTeamMembers()
                .OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
        }

        private void FindPathToTarget()
        {
            var nearestGrids =
                gridManager.GetNearestGridsToTargetGrid(gridManager.GetGridFromPosition(target.transform.position));
            var shortestPathLength = Mathf.Infinity;
            Grid nearest = null;
            foreach (var nearestGrid in nearestGrids)
            {
                var length = Vector2.Distance(new Vector2(nearestGrid.X, nearestGrid.Y),
                    new Vector2(CurrentGrid.X, CurrentGrid.Y));
                if (shortestPathLength < length) continue;
                shortestPathLength = length;
                nearest = nearestGrid;
            }

            var nearestSelectable = SelectableGrid.OrderBy(x =>
                Vector2.Distance(new Vector2(x.X, x.Y), new Vector2(nearest.X, nearest.Y))).FirstOrDefault();
            CalculatePath(nearestSelectable);
        }
    }
}