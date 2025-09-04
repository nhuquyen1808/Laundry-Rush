using System;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class ManagerSound : MonoBehaviour
    {
        public static ManagerSound Instance;
        [SerializeField] AudioSource audioSourceMusic;
        [SerializeField] List<AudioSource> listAudioSource;

        public AudioClip aClick;

        ManagerTimer managerTimer = new ManagerTimer();

        void Awake()
        {
            Instance = this;
            /*if (!GlobalData.Sound)
            {
                StopMusic();
                StopSound();
            }*/
        }

        public static void PlaySound(AudioClip audioClip)
        {
            if (Instance == null) return;
            for (int i = 0; i < Instance.listAudioSource.Count; ++i)
            {
                if (!Instance.listAudioSource[i].isPlaying)
                {
                    Instance.listAudioSource[i].clip = audioClip;
                    Instance.listAudioSource[i].loop = false;
                    Instance.listAudioSource[i].Play();
                    return;
                }
            }

            var audioSource = Instance.listAudioSource.GetClone();
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.Play();
        }

        public static void PlayMusic(AudioClip audioClip)
        {
            if (Instance == null) return;
            Instance.audioSourceMusic.clip = audioClip;
            Instance.audioSourceMusic.loop = true;
            Instance.audioSourceMusic.Play();
        }

        const float FadeTime = 1f;

        public static void StopMusic()
        {
            if (Instance == null) return;
            Instance.managerTimer.Set("MUSIC", FadeTime, null, (remain, total) =>
            {
                if (Instance.audioSourceMusic.volume > remain / total * 0.4f)
                    Instance.audioSourceMusic.volume = remain / total * 0.4f;
            }, 0);
        }

        public static void StopSound()
        {
            if (Instance == null) return;
            Instance.managerTimer.Set("SOUND", FadeTime, null, (remain, total) =>
            {
                foreach (AudioSource audioSource in Instance.listAudioSource)
                    if (audioSource.volume > remain / total)
                        audioSource.volume = remain / total;
            });
        }

        public static void ResumeMusic()
        {
            if (Instance == null) return;
            Instance.managerTimer.Set("MUSIC", FadeTime, null, (remain, total) =>
            {
                if (Instance.audioSourceMusic.volume < (total - remain) / total * 0.4f)
                    Instance.audioSourceMusic.volume = (total - remain) / total * 0.4f;
            });
        }

        public static void ResumeSound()
        {
            if (Instance == null) return;
            Instance.managerTimer.Set("SOUND", FadeTime, null, (remain, total) =>
            {
                foreach (AudioSource audioSource in Instance.listAudioSource)
                    if (audioSource.volume < (total - remain) / total)
                        audioSource.volume = (total - remain) / total;
            });
        }
    }
}