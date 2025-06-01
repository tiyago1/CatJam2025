using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts
{
    public enum SoundType
    {
        Success,
        Fail
    }

    public class SoundContoller : MonoBehaviour
    {
        [SerializeField] private AudioClip gameMusic;
        [SerializeField] private AudioSource clickSource;
        [SerializeField] private List<AudioClip> clips;

        public void PlaySoundVFX(AudioSource source, SoundType type)
        {
            source.PlayOneShot(clips[(int)type]);
        }

        public void PlayClickSound()
        {
            clickSource.Play();
        }
    }
}