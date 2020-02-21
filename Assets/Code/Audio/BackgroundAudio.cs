using System;
using System.Security.Cryptography;
using DG.Tweening;
using Subtegral.AudioUtility;
using UnityEngine;

namespace Code.Audio
{
    public class BackgroundAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip menuAudioClip;
        [SerializeField] private AudioClip fightAudioClip;
        [SerializeField] private bool isMenuScene;

        private static AudioSource _sourceCache;

        private void Start()
        {
            if (!isMenuScene && _sourceCache != null)
            {
                _sourceCache.DOFade(0f, 2f)
                    .OnComplete(() =>
                    {
                        Destroy(_sourceCache.gameObject);
                        Play();
                    });
            }
            else if (isMenuScene)
            {
                Play();
            }
        }

        private void Play()
        {
            _sourceCache = new GameObject("[LoopAudio]").AddComponent<AudioSource>();
            _sourceCache.clip = isMenuScene ? menuAudioClip : fightAudioClip;
            _sourceCache.loop = true;
            _sourceCache.Play();
            DontDestroyOnLoad(_sourceCache.gameObject);
        }
    }
}