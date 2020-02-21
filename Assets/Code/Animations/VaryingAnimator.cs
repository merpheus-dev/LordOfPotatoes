using System.Collections.Generic;
using UnityEngine;

namespace Code.Animations
{
    public sealed class VaryingAnimator : UnitAnimator
    {
        private Queue<int> seedQueue = new Queue<int>();

        private void Awake()
        {
            base.Awake();
            seedQueue.Enqueue(0);
            seedQueue.Enqueue(1);
            if(Random.Range(0,2)==1) ChangeAnimationSeed(); //Add randomization for more natural unit anim variety
        }
        protected override void ChangeAnimationSeed()
        {
            var nextInQueue = seedQueue.Dequeue();
            seedQueue.Enqueue(nextInQueue);
            Animator.SetInteger(Seed,seedQueue.Peek());
        }
    }
}