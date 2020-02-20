using System;
using System.Collections;

namespace Code.Combat
{
    public abstract class Command
    {
        public abstract IEnumerator Execute(Action OnComplete);
    }
}
