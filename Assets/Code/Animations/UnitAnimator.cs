using System;
using UnityEngine;

namespace Code.Animations
{
    [RequireComponent(typeof(Animator))]
    public abstract class UnitAnimator : MonoBehaviour
    {
        [SerializeField] private float seedChangeTimeOut = 2f;

        protected static readonly int Seed = Animator.StringToHash("Seed");
        protected Animator Animator { get; private set; }
        
        protected void Awake()
        {
            Animator = GetComponent<Animator>();
            InvokeRepeating(nameof(ChangeAnimationSeed),seedChangeTimeOut,seedChangeTimeOut);
        }

        protected abstract void ChangeAnimationSeed();
    }
}
