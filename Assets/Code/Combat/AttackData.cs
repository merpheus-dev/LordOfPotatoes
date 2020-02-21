using System;
using UnityEngine;

namespace Code.Combat
{
    [Serializable]
    public struct AttackData
    {
        public Animator Animator { get; }
        public Transform ActorTransform { get; }

        public Vector3 TargetPosition { get; }
        public Quaternion TargetRotation { get; private set; }

        public AttackData(Animator animator, Transform actor, Vector3 targetPosition)
        {
            Animator = animator;
            ActorTransform = actor;
            TargetPosition = targetPosition;
            TargetRotation = Quaternion.identity;
            CalculateRotationFromHeadingVector();
        }

        private void CalculateRotationFromHeadingVector()
        {
            var heading = (TargetPosition - ActorTransform.position).normalized;
            TargetRotation = Quaternion.LookRotation(heading, Vector3.up);
        }
    }
}