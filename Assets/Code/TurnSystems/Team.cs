using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Movement;

namespace Code.TurnSystems
{
    [Serializable]
    public class Team : IEnumerable<MovableCharacter>
    {
        private List<MovableCharacter> units = new List<MovableCharacter>();

        public void AddUnit(MovableCharacter unit)
        {
            units.Add(unit);
        }
        
        public IEnumerator<MovableCharacter> GetEnumerator()
        {
            return units.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}