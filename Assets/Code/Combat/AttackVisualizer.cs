using System;
using Code.TurnSystems;
using Code.UI;
using DG.Tweening;
using UnityEngine;

namespace Code.Combat
{
    public class AttackVisualizer : MonoBehaviour
    {
        [SerializeField] private Transform cursorIndicator;
        [SerializeField] private Transform rangeIndicator;
        [SerializeField] private LineRenderer lineIndicator;
        [SerializeField] private float lineLength = 10f;
        [SerializeField] private TurnManager turnManager;

        
        private Transform _sphereVisualizer;
        private Transform _rangeVisualizer;

        private LineRenderer _largeAttackVisualizer;

        //TODO:Implement render modes based on 'command' s render method, which you should implement
        private void Awake()
        {
            SpawnVisualizers();
        }

        private void SpawnVisualizers()
        {
            _sphereVisualizer = Instantiate(cursorIndicator);
            _rangeVisualizer = Instantiate(rangeIndicator);
            _largeAttackVisualizer = Instantiate(lineIndicator);
            HideVisualizers();
        }

        public void VisualizeCommand(Command command, Vector3 targetPosition)
        {
            if (command is LargeAttackCommand)
            {
                var authUnitPosition = turnManager.GetCurrentlyAuthUnit().transform.position;
                authUnitPosition.y = 0;
                var heading = (targetPosition - authUnitPosition).normalized;
                heading.y = 0.1f;
                ShowLineIndicator(authUnitPosition, heading*lineLength);
            }
            else if (command is FlatAttackCommand)
                ShowOnMouseCursor(targetPosition);
        }

        private void ShowOnMouseCursor(Vector3 position)
        {
            _sphereVisualizer.gameObject.SetActive(true);
            _sphereVisualizer.position = position;
        }

        public void ShowLineIndicator(Vector3 pointA, Vector3 pointB)
        {
            _largeAttackVisualizer.gameObject.SetActive(true);
            _largeAttackVisualizer.SetPosition(0, pointA);
            _largeAttackVisualizer.SetPosition(1, pointB);
        }

        public void ShowRangeIndicator(Vector3 position, float radius)
        {
            _rangeVisualizer.gameObject.SetActive(true);
            _rangeVisualizer.position = position;
            _rangeVisualizer.localScale = new Vector3(radius * 2f, .1f, radius * 2f);
            _rangeVisualizer.GetComponent<MeshRenderer>().material.DOFade(.4f, .1f);
        }

        public void HideVisualizers()
        {
            _sphereVisualizer.gameObject.SetActive(false);
            _rangeVisualizer.gameObject.SetActive(false);
            _largeAttackVisualizer.gameObject.SetActive(false);
            _rangeVisualizer.GetComponent<MeshRenderer>().material.DOFade(0f, .1f);
        }
    }
}