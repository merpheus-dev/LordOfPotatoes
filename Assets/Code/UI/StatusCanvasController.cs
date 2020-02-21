using System;
using Code.Combat;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    [RequireComponent(typeof(Canvas))]
    public sealed class StatusCanvasController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Image healthBarContainer;
        [SerializeField] private Image healthBar;
        
        private Canvas _targetCanvas;
        private Camera _targetCamera;
        private RectTransform _statusRect;

        private void Awake()
        {
            _targetCamera = Camera.main;
            _targetCanvas = GetComponent<Canvas>();
            _statusRect = statusText.GetComponent<RectTransform>();
            _targetCanvas.worldCamera = _targetCamera;
            healthBarContainer.DOFade(0f, 0f);
        }

        private void FixedUpdate()
        {
            var heading = (_targetCamera.transform.position - transform.position).normalized;
            var headingRotation = Quaternion.LookRotation(heading);
            transform.rotation = headingRotation;
        }

        private void LateUpdate()
        {
            if (healthBar.fillAmount > .5f)
                healthBar.color = Color.Lerp(Color.green, Color.yellow, .5f - ((healthBar.fillAmount - .5f) / .5f));
            else
                healthBar.color = Color.Lerp(Color.yellow, Color.red, (.5f - healthBar.fillAmount) / .5f);
        }

        public void DisplayActionTaken(Command command)
        {
            statusText.text = command.CommandDisplayName;
            DOTween.Sequence()
                .Append(statusText.DOFade(1f, .3f))
                .Append(_statusRect.DOAnchorPosY(3f, 3f))
                .Append(statusText.DOFade(0f, 1f))
                .Append(_statusRect.DOAnchorPosY(0f, 0f));
        }
        
        public void DisplayHealthBar(float currentHealth,float beginHealth)
        {
            DOTween.Sequence()
                .Append(healthBarContainer.DOFade(1f, .2f))
                .Append(healthBar.DOFillAmount(currentHealth / beginHealth, .2f))
                .AppendInterval(1f)
                .Append(healthBarContainer.DOFade(0f, .2f));
        }
        
    }
}
