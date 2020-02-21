using System.Linq;
using Code.Movement;
using Subtegral.AudioUtility;
using UnityEngine;

namespace Code.Combat.AttackActors
{
    public sealed class RangeAttackActor : AttackActor<RangeAttackCommand>
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float effectRange = 2f;
        [SerializeField] private AudioClip sfx;
        public override void PerformAttack(RangeAttackCommand attackCommand)
        {
            var collider = Physics.OverlapSphere(transform.position, effectRange, LayerMask.GetMask("Units"));
            var effectedOpponentUnits = collider.Select(x => x.GetComponent<Unit>()).Except(attackCommand.Owner);
            foreach (var effectedOpponentUnit in effectedOpponentUnits)
            {
                effectedOpponentUnit.ReceiveDamage(damage);
            }
            AudioManager.GetInstance().SetPoolSize(3).PlayOneShot(sfx);
        }
    }
}