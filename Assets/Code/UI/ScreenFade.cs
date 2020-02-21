using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    public sealed class ScreenFade : MonoBehaviour
    {
        [SerializeField] private Image fadeOverlay;
        [SerializeField] private bool startWithFade;
        #region Singleton

        private static ScreenFade _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        #endregion

        private void Start()
        {
            if (!startWithFade) return;
            fadeOverlay.gameObject.SetActive(true);
            DOTween.Sequence()
                .Append(fadeOverlay.DOFade(1f, .0f))
                .AppendInterval(1f)
                .Append(fadeOverlay.DOFade(0f, .3f))
                .AppendCallback(() => fadeOverlay.gameObject.SetActive(false));
        }
        
        public void FadeScreenAndLoadScene(string sceneName)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.DOFade(1f, .5f)
                .OnComplete(() =>
                {
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadScene(sceneName);
                });
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            DOTween.Sequence()
                .AppendInterval(1f)
                .Append(fadeOverlay.DOFade(0f, .3f))
                .AppendCallback(() => fadeOverlay.gameObject.SetActive(false));
        }
    }
}