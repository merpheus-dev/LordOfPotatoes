using System;
using System.Collections.Generic;
using System.Linq;
using Code.Movement;
using UnityEngine;

namespace Code.TurnSystems
{
    public class TurnManager : MonoBehaviour
    {
        //TODO: Remove this && Auto genereate team members from Resource.Load
        [SerializeField] private MovableCharacter[] teamAMembers;
        [SerializeField] private MovableCharacter[] teamBMembers;
        
        private Team teamA;
        private Team teamB;
        private Team activeTeam;
        
        private Queue<MovableCharacter> turnables = new Queue<MovableCharacter>();

        public IEnumerable<MovableCharacter> GetPlayerTeamMembers()
        {
            return teamA;
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

        private void CreateTeam(ref Team team, MovableCharacter[] members)
        {
            team = new Team();
            foreach (var member in members)
            {
               team.AddUnit(member); 
            }
        }

        public void TakeTurn()
        {
            var authorizedMember = turnables.Peek();
            authorizedMember.TakeTurn(OnTurnComplete);
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