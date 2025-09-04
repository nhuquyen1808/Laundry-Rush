using DevDuck;
using ntDev;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioSource> sourcesMusic = new List<AudioSource>();
    public List<AudioSource> sourcesMusicMiniGame = new List<AudioSource>();
    public List<AudioSource> sourcesSound = new List<AudioSource>();
    public List<AudioSource> sourcesVoice = new List<AudioSource>();
    public bool isSound, isMusic;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public async void PlayBGMSound(string str)
    {

     //   if (ManagerAsset.IsExist(str, typeof(AudioClip)))
        {
              AudioClip clip = await ManagerAsset.LoadAssetAsync<AudioClip>(str);
           // AudioClip clip = Resources.Load<AudioClip>($"SoundBase/{str}");
            for (int i = 0; i < sourcesMusic.Count; i++)
            {
                if (!sourcesMusic[i].isPlaying)
                {
                    sourcesMusic[i].clip = clip;
                 //   if (PlayerPrefs.GetInt("MUSIC") == 1)
                    {
                        sourcesMusic[i].Play();
                    }
                    break;
                }
            }
            GameObject obj = new GameObject();
            AudioSource aus = obj.AddComponent<AudioSource>();
            aus.gameObject.transform.SetParent(this.transform); 
            sourcesMusic.Add(aus);
            aus.clip = clip;
          //  aus.volume = 0.15f;
            aus.loop = true;
           // if (PlayerPrefs.GetInt("MUSIC") == 1)
            {
                aus.Play();

            }
        }
    }
    public async void PlaySound(string str)
    {

       // if (ManagerAsset.IsExist(str, typeof(AudioClip)))
        {
            AudioClip clip = await ManagerAsset.LoadAssetAsync<AudioClip>(str);
        //    AudioClip clip = Resources.Load<AudioClip>($"SoundBase/{str}");
            for (int i = 0; i < sourcesSound.Count; i++)
            {
                if (!sourcesSound[i].isPlaying)
                {
                    sourcesSound[i].clip = clip;
                //    if (PlayerPrefs.GetInt("SOUND") == 1)
                    {
                        sourcesSound[i].playOnAwake = false; sourcesSound[i].loop = false;
                        sourcesSound[i].PlayOneShot(clip);
                    }
                    return;
                }
            }
            GameObject obj = new GameObject();
            AudioSource aus = obj.AddComponent<AudioSource>();
            aus.gameObject.transform.SetParent(this.transform);
            sourcesSound.Add(aus);
            aus.clip = clip;
          //  aus.volume = 0.25f;
         //   if (PlayerPrefs.GetInt("SOUND") == 1)
            {
                aus.playOnAwake = false;
                aus.loop= false;
                aus.PlayOneShot(clip);

            }
        }
    }
    public void StopPlaySound()
    {
        for (int i = 0; i < sourcesSound.Count; i++)
        {
            //if (sourcesSound[i].isPlaying)
            {
                sourcesSound[i].enabled = false;
            }
        }
    }
    public void StopPlayVoice()
    {
        for (int i = 0; i < sourcesVoice.Count; i++)
        {
            //if (sourcesSound[i].isPlaying)
            {
                sourcesVoice[i].enabled = false;
            }
        }
    }
    public void StopPlayMusic()
    {
        for (int i = 0; i < sourcesMusic.Count; i++)
        {
            if (sourcesMusic[i].isPlaying)
            {
                sourcesMusic[i].enabled = false;
                sourcesMusic[i].Pause();
            }
        }
    }
    public void ContinuePlaySound()
    {
        if (sourcesMusic.Count > 0)
        {
            for (int i = 0; i < sourcesSound.Count; i++)
            {
                if (!sourcesSound[i].enabled)
                {
                    sourcesSound[i].enabled = true;

                }
                else
                {
                    return;
                }

            }
        }

    }

    public void ContinuePlayMusic()
    {
        if (sourcesMusic.Count > 0)
        {
            for (int i = 0; i < sourcesMusic.Count; i++)
            {
                // if (!sourcesMusic[i].isPlaying)
                {
                    sourcesMusic[i].enabled = true;
                    sourcesMusic[i].Play();
                }
            }
        }

    }

    public void StopPlayBGMSound()
    {
        for (int i = 0; i < sourcesMusic.Count; i++)
        {
            sourcesMusic[i].Stop();

        }
    }

}
