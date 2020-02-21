using System;
using Code.Movement;
using Code.UI;
using DG.Tweening;
using UnityEngine;

namespace Code.TurnSystems
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 directionalDistanceToTarget;
        private void Awake()
        {
            EventDispatcher.OnTurnAuthChanged += TurnAuthChanged;
            EventDispatcher.OnUnitMovementComplete += TurnAuthChanged;
        }

        private void TurnAuthChanged(Unit unit)
        {
            MoveToPosition(unit.transform.position);
        }

        private void MoveToPosition(Vector3 position)
        {
            position += directionalDistanceToTarget;
            var targetPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.DOMove(targetPosition, .5f).SetEase(Ease.Flash);
        }
    }
}
