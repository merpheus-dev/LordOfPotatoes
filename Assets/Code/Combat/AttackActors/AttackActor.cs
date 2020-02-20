using System;
using UnityEngine;

namespace Code.Combat.AttackActors
{
    public abstract class AttackActor<T> : MonoBehaviour where T : AttackCommand
    {
        public abstract void PerformAttack(T attackCommand);
    }
}