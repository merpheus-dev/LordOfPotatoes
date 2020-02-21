using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Movement;
using DG.Tweening;
using Subtegral.AudioUtility;
using UnityEngine;

namespace Code.Combat.AttackActors
{
    public sealed class LargeAttackActor : AttackActor<LargeAttackCommand>
    {
        [SerializeField] private float damage = 30;
        [SerializeField] private Transform visualizer;
        [SerializeField] private AudioClip sfx;

        public override void PerformAttack(LargeAttackCommand attackCommand)
        {
            visualizer.DOMove(transform.position + transform.forward * attackCommand.Range, 1f)
                      .OnComplete(() => visualizer.gameObject.SetActive(false));
            
            var ray = new Ray(transform.position,attackCommand.Heading);
            var castResult = Physics.RaycastAll(ray, attackCommand.Range, LayerMask.GetMask("Units"));
            var effectedUnits = castResult.Select(x => x.collider.GetComponent<Unit>()).Except(attackCommand.Owner);
            StartCoroutine(ApplyDamageWithDelay(effectedUnits));
        }

        private IEnumerator ApplyDamageWithDelay(IEnumerable<Unit> effectedUnits)
        {
            var waitForSecondsCached= new WaitForSeconds(.3f);
            foreach (var effectedUnit in effectedUnits)
            {
                effectedUnit.ReceiveDamage(damage);
                yield return waitForSecondsCached;
            }
            AudioManager.GetInstance().SetPoolSize(3).PlayOneShot(sfx);
        }
    }
}