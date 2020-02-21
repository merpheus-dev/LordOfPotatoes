using System;
using Code.Combat;
using Code.Movement;
using UnityEngine;

namespace Code.UI
{
    public static class EventDispatcher
    {
        public static Action<PlayerCharacter> OnUnitRequestsCombatOptions;
        public static Action<Command,Vector3> OnAttackCommandSelected;
        public static Action<bool> OnGameOver;
        public static Action<Unit> OnTurnAuthChanged;
        public static Action<Unit> OnUnitMovementComplete;
    }
}
