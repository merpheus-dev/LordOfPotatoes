using System;
using System.Collections;
using System.Linq;
using Code.TurnSystems;
using UnityEngine;
using UnityEngine.AI;
using Grid = Code.GridSystems.Grid;

namespace Code.Movement
{
    public class AiCharacter : Unit
    {
        [SerializeField] private TurnManager turnManager;
        private Unit _target;

        private void Update()
        {
            if (!IsAuthForTurn || WaitingForCombatSelection) return;
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
            _target = turnManager.GetPlayerTeamMembers()
                .OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
        }

        private void FindPathToTarget()
        {
            var nearestGrids =
                gridManager.GetNearestGridsToTargetGrid(gridManager.GetGridFromPosition(_target.transform.position));
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

        //TODO: Implement
        protected override void AuthForCombatChoices()
        {
            WaitingForCombatSelection = true;
            StartCoroutine(PerformAttack());
        }

        private IEnumerator PerformAttack()
        {
            yield return new WaitForSeconds(2f);
            WaitingForCombatSelection = false;
            FinishTurn();
        }
    }
}