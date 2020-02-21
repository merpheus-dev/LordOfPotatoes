using System;
using System.Collections;

namespace Code.Combat
{
    public class PassTurnCommand : Command
    {
        public override string CommandDisplayName { get; protected set; } = "PASS!";

        public override IEnumerator Execute(Action OnComplete)
        {
            OnComplete?.Invoke();
            yield break;
        }
    }
}