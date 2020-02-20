using System;
using System.Collections;
using Code.Combat.AttackActors;
using UnityEngine;

namespace Code.Combat
{
    public sealed class RangeAttackCommand : AttackCommand
    {
        protected override int Key { get; } = Animator.StringToHash("RangeAttack");
        protected override string AttackPrefab { get; } = "RangeAttack";
        protected override void OnSpawn(GameObject actor)
        {
            actor.GetComponent<RangeAttackActor>().PerformAttack(this);
        }

        public override IEnumerator Execute(Action onComplete)
        {
            yield return base.Execute(onComplete);
            yield return new WaitForSeconds(2f);
            onComplete?.Invoke();
        }
    }
}