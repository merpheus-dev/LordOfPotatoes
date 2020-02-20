using System;
using System.Collections;
using Code.Movement;
using Code.TurnSystems;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Combat
{
    public abstract class AttackCommand : Command
    {
        public Team Owner;
        public Vector3 Heading => (AttackData.TargetPosition - AttackData.ActorTransform.position).normalized;
        public virtual int Range { get; }
        protected virtual int Key { get; }
        protected AttackData AttackData;
        protected virtual string AttackPrefab { get; } = string.Empty;

        protected abstract void OnSpawn(GameObject actor);
        public void InjectData(AttackData attackData)
        {
            AttackData = attackData;
        }

        public override IEnumerator Execute(Action onComplete)
        {
            AttackData.Animator.SetTrigger(Key);
            var prefab = Resources.Load<GameObject>($"Attacks/{AttackPrefab}");
            var attackActor = Object.Instantiate(prefab, AttackData.TargetPosition, AttackData.TargetRotation);
            OnSpawn(attackActor);
            yield return null;
        }

    }
}