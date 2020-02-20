using System;
using System.Collections;
using System.Collections.Generic;
using Code.Movement;

namespace Code.TurnSystems
{
    [Serializable]
    public class Team : IEnumerable<Unit>
    {
        private List<Unit> units = new List<Unit>();

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
        }
        
        public IEnumerator<Unit> GetEnumerator()
        {
            return units.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}