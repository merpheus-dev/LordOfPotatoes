using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Code.Combat
{
    public abstract class DirectionalAttackCommand : AttackCommand
    {
        public override IEnumerator Execute(Action OnComplete)
        {
            var heading = (AttackData.TargetPosition - AttackData.ActorTransform.position).normalized;
            var headingRotation = Quaternion.LookRotation(heading, Vector3.up);
            AttackData.ActorTransform.DORotateQuaternion(headingRotation,.1f);
            return base.Execute(OnComplete);
        }
    }
}