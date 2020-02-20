using System.Linq;
using Code.Movement;
using UnityEngine;

namespace Code.Combat.AttackActors
{
    public sealed class LargeAttackActor : AttackActor<LargeAttackCommand>
    {
        [SerializeField] private float damage = 30;
        public override void PerformAttack(LargeAttackCommand attackCommand)
        {
            var ray = new Ray(transform.position,attackCommand.Heading);
            var castResult = Physics.RaycastAll(ray, attackCommand.Range, LayerMask.GetMask("Units"));
            var effectedUnits = castResult.Select(x => x.collider.GetComponent<Unit>()).Except(attackCommand.Owner);
            foreach (var effectedUnit in effectedUnits)
            {
                effectedUnit.ReceiveDamage(damage);
            }
        }
    }
}