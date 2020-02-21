using System;
using System.Collections.Generic;
using Assets.Code.GridSystems;
using Code.GridSystems;
using UnityEngine;
using Grid = Code.GridSystems.Grid;

namespace Code.Movement
{
    public abstract class Unit : GridBlocker,IDamageable
    {
        [SerializeField] protected GridManager gridManager;
        [SerializeField] private int radius = 5;
        [SerializeField] private float movementLerpTreshold = .02f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float health = 100;
        [SerializeField] protected Animator animator;
        public float Health { get; private set; }
        public Grid CurrentGrid { get; protected set; }
        protected List<Grid> SelectableGrid = new List<Grid>();
        protected bool Moving = false;
        protected bool IsAuthForTurn = false;
        protected bool WaitingForCombatSelection = false;
        private Stack<Grid> movementPath = new Stack<Grid>();
        private Action _onTurnComplete;
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Walk = Animator.StringToHash("Walk");

        protected abstract void AuthForCombatChoices();

        protected void Awake()
        {
            Health = health;
        }

        protected void FindSelectableTiles()
        {
            SelectableGrid.Clear();
            CurrentGrid = gridManager.GetGridFromPosition(transform.position);
            CurrentGrid.Walkable = false;
            CurrentGrid.GridStatus = GridStatus.Current;
            var process = new Queue<Grid>();
            process.Enqueue(CurrentGrid);
            CurrentGrid.Visited = true;

            while (process.Count > 0)
            {
                var grid = process.Dequeue();
                SelectableGrid.Add(grid);
                grid.GridStatus = GridStatus.Selectable;
                if (grid.Distance >= radius) continue;
                foreach (var adjent in grid.Adjents)
                {
                   if(adjent.Visited) continue;
                   adjent.Parent = grid;
                   adjent.Visited = true;
                   adjent.Distance = 1 + grid.Distance;
                   process.Enqueue(adjent);
                }
            }
        }

        public void CalculatePath(Grid target)
        {
            movementPath.Clear();
            target.GridStatus = GridStatus.Target;
            Moving = true;
            var nextNode = target;
            while (nextNode != null)
            {
                movementPath.Push(nextNode);
                nextNode = nextNode.Parent;
            }
        }

        public void PerformMovement()
        {
            if (movementPath.Count > 0)
            {
                var targetNode = movementPath.Peek();
                var targetPosition = gridManager.GetGridVisualizerFromGrid(targetNode).transform.position;
                if (Vector3.Distance(targetPosition, transform.position) > movementLerpTreshold)
                {
                    var heading = (targetPosition - transform.position).normalized;
                    transform.rotation = Quaternion.LookRotation(heading, Vector3.up);
                    transform.forward = heading;
                    transform.position += heading * speed * Time.deltaTime;
                }
                else
                {
                    transform.position = targetPosition;
                    movementPath.Pop();
                }
                animator.SetBool(Walk,true);
            }
            else
            {
                animator.SetBool(Walk,false);
                CurrentGrid.Walkable = true;
                gridManager.ClearGridCalculations();
                SelectableGrid.Clear();
                CurrentGrid = null;
                Moving = false;
                AuthForCombatChoices();
            }
        }

        public void TakeTurn(Action onTurnComplete)
        {
            this._onTurnComplete = onTurnComplete;
            IsAuthForTurn = true;
        }

        protected void FinishTurn()
        {
            IsAuthForTurn = false;
            WaitingForCombatSelection = false;
            _onTurnComplete?.Invoke();
        }

        public void ReceiveDamage(float hitPoint)
        {
            Health -= hitPoint;
            Health = Mathf.Clamp(Health,0, Health);
            animator.SetTrigger(Health > 0 ? Hit : Die);
        }
    }
}
