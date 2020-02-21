using System;
using System.Collections;
using Code.Combat.AttackActors;
using UnityEngine;

namespace Code.Combat
{
    public sealed class LargeAttackCommand : DirectionalAttackCommand
    {
        public override int Range { get; } = 7;
        protected override int Key { get;} = Animator.StringToHash("LargeAttack");
        protected override string AttackPrefab { get; } = "LargeAttack";
        protected override void OnSpawn(GameObject actor)
        {
            actor.SetActive(true);
            actor.transform.rotation = Quaternion.LookRotation(Heading);
            actor.transform.position = AttackData.ActorTransform.position;
            //actor.transform.position += actor.transform.forward * 3f;
            actor.GetComponent<LargeAttackActor>().PerformAttack(this);
        }

        public override string CommandDisplayName { get; protected set; } = "SPELL CAST!";

        public override IEnumerator Execute(Action onComplete)
        {
            yield return base.Execute(onComplete);
            yield return new WaitForSeconds(2f);
            onComplete?.Invoke();
        }
    }
}