using System;
using System.Collections;

namespace Code.Combat
{
    public class PassTurnCommand : Command
    {
        public override IEnumerator Execute(Action OnComplete)
        {
            OnComplete?.Invoke();
            yield break;
        }
    }
}