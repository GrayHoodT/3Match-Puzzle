using System.Collections;
using UnityEngine;
using Framework;

namespace Anipang
{
    public class SFXPlayer : Poolable
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayAudio(AudioClip clip, float volume = 1f)
        {
            gameObject.SetActive(true);
            audioSource.clip = clip;
            audioSource.volume = Mathf.Clamp(volume, 0f, 1f);
            audioSource.Play();

            StartCoroutine(CheckAudioEnd());
        }

        private IEnumerator CheckAudioEnd()
        {
            while (audioSource.isPlaying)
                yield return null;

            ReturnToPool();
        }
    }
}
