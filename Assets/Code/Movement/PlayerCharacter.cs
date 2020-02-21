using System;
using Assets.Code.GridSystems;
using Code.Combat;
using Code.GridSystems;
using Code.UI;
using UnityEngine;

namespace Code.Movement
{
    public class PlayerCharacter : Unit
    {
        private void Update()
        {
            if (!IsAuthForTurn || WaitingForCombatSelection) return;
            if (!Moving)
            {
                FindSelectableTiles();
                ListenMousePositon();
            }
            else
            {
                PerformMovement();
            }
        }

        private void ListenMousePositon()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo, 100, LayerMask.GetMask("Grid")))
                {
                    var grid = hitInfo.collider.GetComponent<GridVisualizer>().TargetGrid;
                    if (grid.GridStatus == GridStatus.Selectable)
                    {
                        CalculatePath(grid);
                    }
                }
            }
        }

        protected override void AuthForCombatChoices()
        {
            WaitingForCombatSelection = true;
            EventDispatcher.OnAttackCommandSelected += OnAttackSelected;
            EventDispatcher.OnUnitRequestsCombatOptions?.Invoke(this);
        }

        private void OnAttackSelected(Command command,Vector3 targetPosition)
        {
            if (command is AttackCommand attackCommand)
            {
                attackCommand.InjectData(new AttackData(animator,transform,targetPosition));
                command = attackCommand;
            }

            StartCoroutine(command.Execute(OnAttackPerformed));
        }

        private void OnAttackPerformed()
        {
            EventDispatcher.OnAttackCommandSelected -= OnAttackSelected;
            FinishTurn();
        }
    }
}