using System;
using System.Collections;

namespace Code.Combat
{
    public abstract class Command
    {
        public abstract string CommandDisplayName { get; protected set; }
        public abstract IEnumerator Execute(Action OnComplete);
    }
}
