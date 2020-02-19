using Assets.Code.GridSystems;
using Code.GridSystems;
using UnityEngine;

namespace Code.Movement
{
    public class PlayerCharacter : MovableCharacter
    {
        private void Update()
        {
            if (!isAuthForTurn) return;
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
                if (Physics.Raycast(ray, out var hitInfo,100,LayerMask.GetMask("Grid")))
                {
                    var grid = hitInfo.collider.GetComponent<GridVisualizer>().TargetGrid;
                    if (grid.GridStatus==GridStatus.Selectable)
                    {
                        CalculatePath(grid);
                    }
                }
            }
        }
    }
}