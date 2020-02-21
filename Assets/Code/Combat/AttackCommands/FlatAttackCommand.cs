using System;
using System.Collections;
using Code.Combat.AttackActors;
using UnityEngine;

namespace Code.Combat
{
    public sealed class FlatAttackCommand : DirectionalAttackCommand
    {
        protected override string AttackPrefab { get; } = "FlatAttack";
        public override int Range { get; } = 2;
        protected override int Key { get; }  = Animator.StringToHash("FlatAttack");

        protected override void OnSpawn(GameObject actor)
        {
            actor.GetComponent<FlatAttackActor>().PerformAttack(this);
        }

        public override string CommandDisplayName { get; protected set; } = "SWORD ATTACK!";

        public override IEnumerator Execute(Action onComplete)
        {
            yield return base.Execute(onComplete);
            yield return new WaitForSeconds(2f);
            onComplete?.Invoke();
        }
    }
}