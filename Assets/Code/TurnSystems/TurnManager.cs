using System;
using System.Collections.Generic;
using System.Linq;
using Code.Movement;
using Code.UI;
using UnityEngine;

namespace Code.TurnSystems
{
    public class TurnManager : MonoBehaviour
    {
        //TODO: Remove this && Auto genereate team members from Resource.Load
        [SerializeField] private Unit[] teamAMembers;
        [SerializeField] private Unit[] teamBMembers;
        
        private Team teamA;
        private Team teamB;
        private Team activeTeam;
        
        private Queue<Unit> turnables = new Queue<Unit>();

        public IEnumerable<Unit> GetPlayerTeamMembers()
        {
            return teamA;
        }

        public Unit GetCurrentlyAuthUnit()
        {
            return turnables.Peek();
        }

        public Team GetCurrentlyAuthTeam()
        {
            return activeTeam;
        }

        public Team GetOppositeTeam()
        {
            return activeTeam == teamA ? teamB : teamA;
        }

        public void DeactivateUnit(Unit unit)
        {
           teamA.RemoveUnit(unit);
           teamB.RemoveUnit(unit);
           var currentTurnList = turnables.ToList();
           currentTurnList.Remove(unit);
           turnables.Clear();
           foreach (var current in currentTurnList)
           {
               turnables.Enqueue(current);
           }
        }
        
        private void Awake()
        {
            GenereateTeams();
            activeTeam = teamA;
            PopulateTeamQueue();
            TakeTurn();
        }

        //TODO:Auto genereate team members from Resource.Load
        private void GenereateTeams()
        {
            CreateTeam(ref teamA,teamAMembers);
            CreateTeam(ref teamB,teamBMembers);
        }

        private void CreateTeam(ref Team team, Unit[] members)
        {
            team = new Team();
            foreach (var member in members)
            {
               team.AddUnit(member); 
            }
        }

        public void TakeTurn()
        {
            if (!teamA.Any() || !teamB.Any())
            {
                EventDispatcher.OnGameOver?.Invoke(teamA.Any());
                return;
            }
            var authorizedMember = GetCurrentlyAuthUnit();
            authorizedMember.TakeTurn(OnTurnComplete);
            EventDispatcher.OnTurnAuthChanged?.Invoke(authorizedMember);
        }

        private void OnTurnComplete()
        {
            turnables.Dequeue();
            if(turnables.Count>0)
                TakeTurn();
            else
            {
                FlipActiveTeam();
                PopulateTeamQueue();
                TakeTurn();
            }
        }

        private void FlipActiveTeam()
        {
            if (activeTeam == teamA)
                activeTeam = teamB;
            else
            {
                activeTeam = teamA;
            }
        }

        private void PopulateTeamQueue()
        {
            foreach (var unit in activeTeam)
            {
                turnables.Enqueue(unit);
            }
        }
    }
}