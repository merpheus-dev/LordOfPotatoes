using System;
using Code.Combat;
using Code.Combat.AttackActors;
using Code.GridSystems;
using Code.Movement;
using Code.TurnSystems;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class CombatUi : MonoBehaviour, IUiEventListener
    {
        [SerializeField] private GameObject combatOptionContainer;
        [SerializeField] private Button[] combatOptions;
        [SerializeField] private GameObject gameResultGameObject;
        [SerializeField] private RectTransform gameResultContainer;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private AttackVisualizer attackVisualizer;
        [SerializeField] private TurnManager turnManager;
        private bool _listenMouseSelection = false;
        private Command _activeCommand;

        private void Awake()
        {
            RegisterEvents();
        }

        public void RegisterEvents()
        {
            EventDispatcher.OnUnitRequestsCombatOptions += ShowCombatUiForUnit;
            EventDispatcher.OnGameOver += ShowResultScreen;
        }

        private void ShowCombatUiForUnit(PlayerCharacter unit)
        {
            combatOptionContainer.SetActive(true);
            ChangeCombatButtonInteraction(true);
        }

        private void ShowResultScreen(bool playerWin)
        {
            resultText.text = playerWin ? "ORCS DEFEATED" : "YOU FAILED";
            gameResultGameObject.SetActive(true);
            gameResultContainer.DOAnchorPosY(0f, 1f);
        }
        
        public void SelectAttackType(int attackType)
        {
            ChangeCombatButtonInteraction(false);
            
            _activeCommand = CommandFactory.ConvertAttackTypeToCommand(attackType,
                                                                turnManager.GetCurrentlyAuthTeam());
            switch (_activeCommand)
            {
                case PassTurnCommand _:
                    EventDispatcher.OnAttackCommandSelected?.Invoke(_activeCommand,
                        Vector3.zero);
                    CloseCombatInterface();
                    return;
                case RangeAttackCommand _:
                    EventDispatcher.OnAttackCommandSelected?.Invoke(_activeCommand,turnManager.GetCurrentlyAuthUnit().transform.position);
                    CloseCombatInterface();
                    return;
            }

            _listenMouseSelection = true;
            if (_activeCommand is FlatAttackCommand)
                DisplayUnitAttackRange();
        }

        private void Update()
        {
            if (!_listenMouseSelection) return;
            if (!Raycasting(out var hitInfo) || !IsHitPointInUnitAttackRadius(hitInfo.point)) return;
            attackVisualizer.VisualizeCommand(_activeCommand, hitInfo.point);
            if (!Input.GetMouseButtonDown(0)) return;
            OnAttackTargetSetForCommandBroadcast(hitInfo.point);
        }

        private void DisplayUnitAttackRange()
        {
            var authUnit = turnManager.GetCurrentlyAuthUnit();
            attackVisualizer.ShowRangeIndicator(authUnit.transform.position, ((AttackCommand) _activeCommand).Range);
        }

        private bool Raycasting(out RaycastHit hit)
        {
            return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f,
                LayerMask.GetMask("Terrain"));
        }

        private bool IsHitPointInUnitAttackRadius(Vector3 worldPosition)
        {
            var authUnit = turnManager.GetCurrentlyAuthUnit();
            var distance = Vector3.Distance(worldPosition, authUnit.transform.position);
            return distance <= ((AttackCommand) _activeCommand).Range;
        }

        private void OnAttackTargetSetForCommandBroadcast(Vector3 attackPosition)
        {
            EventDispatcher.OnAttackCommandSelected?.Invoke(_activeCommand, attackPosition);
            _listenMouseSelection = false;
            _activeCommand = null;
            CloseCombatInterface();
        }

        private void CloseCombatInterface()
        {
            combatOptionContainer.SetActive(false);
            attackVisualizer.HideVisualizers();
        }

        private void ChangeCombatButtonInteraction(bool interactionsAllowed)
        {
            foreach (var combatOption in combatOptions)
                combatOption.interactable = interactionsAllowed;
        }
    }
}