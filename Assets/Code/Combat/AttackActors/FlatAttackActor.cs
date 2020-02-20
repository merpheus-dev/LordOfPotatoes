using System.Linq;
using Code.Movement;
using UnityEngine;

namespace Code.Combat.AttackActors
{
    public sealed class FlatAttackActor : AttackActor<FlatAttackCommand>
    {
        [SerializeField] private float effectRange = 2f;
        [SerializeField] private float damage = 25f;

        public override void PerformAttack(FlatAttackCommand attackCommand)
        {
            var collider = Physics.OverlapSphere(transform.position, effectRange, LayerMask.GetMask("Units"));
            var effectedOpponentUnits = collider.Select(x => x.GetComponent<Unit>()).Except(attackCommand.Owner);
            foreach (var effectedOpponentUnit in effectedOpponentUnits)
            {
                effectedOpponentUnit.ReceiveDamage(damage);
            }
        }
    }
}