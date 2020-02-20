using System;
using System.Collections.Generic;
using System.Linq;
using Code.Combat;
using Code.Movement;
using Code.TurnSystems;
using Subtegral.WeightedRandom;
using UnityEngine;

namespace Code.AI
{
    public class AiCommandDriver : MonoBehaviour
    {
        private WeightedRandom<AttackCommand> _commandPool;
        private TurnManager _turnManager;

        private void Awake()
        {
            _turnManager = FindObjectOfType<TurnManager>();
        }

        //TODO:Add stamina and designate probability based on it.
        private void AddCommandsInAttackRange(AiCharacter aiCharacter, Unit target)
        {
            _commandPool = new WeightedRandom<AttackCommand>();
            var commandList = new AttackCommand[] {new FlatAttackCommand(), new RangeAttackCommand(), new LargeAttackCommand()};
            foreach (var attackCommand in commandList)
            {
                if (Vector3.Distance(aiCharacter.transform.position, target.transform.position) < attackCommand.Range)
                    _commandPool.Add(attackCommand, .5f);
            }
        }

        public Command MakeDecision(AiCharacter aiCharacter, Unit target)
        {
            AddCommandsInAttackRange(aiCharacter,target);
            if(_commandPool.Count==0) return new PassTurnCommand();
            var selectedCommand = _commandPool.Next();
            selectedCommand.Owner = _turnManager.GetCurrentlyAuthTeam();
            return selectedCommand;
        }

        public Unit PickTarget(AiCharacter aiCharacter)
        {
            var getOpposites = _turnManager.GetOppositeTeam();
            var hitList = new Dictionary<Unit, int>(); //Unit and its cost for targeting,higher is better.
            var enemies = getOpposites.OrderByDescending(x => x.Health);
            var enemiesByDistance =
                getOpposites.OrderBy(x => Vector3.Distance(x.transform.position, aiCharacter.transform.position));
            var minusCost = 0;
            foreach (var unit in enemies)
            {
                hitList.Add(unit, Mathf.Abs(enemies.Count() - minusCost));
                minusCost++;
            }

            minusCost = 0;
            foreach (var unit in enemiesByDistance)
            {
                hitList[unit] += Mathf.Abs(enemiesByDistance.Count() - minusCost);
                minusCost++;
            }

            return hitList.OrderBy(x => x.Value).First().Key;
        }
    }
}