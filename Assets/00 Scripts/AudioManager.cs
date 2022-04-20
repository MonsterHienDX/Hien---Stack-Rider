using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip ballCollected;
    public AudioClip ballLoseByWall;
    public AudioClip ballBreak;
    public AudioClip coinCollected;
    public AudioClip congratulation;

    public static AudioManager instance;
    private AudioSource audioSource;
    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(AudioName audioName)
    {
        switch (audioName)
        {
            case AudioName.ballCollected:
                audioSource.clip = ballCollected;
                audioSource.Play();
                break;
            case AudioName.ballLoseByWall:
                audioSource.clip = ballLoseByWall;
                audioSource.Play();
                break;
            case AudioName.ballBreak:
                audioSource.clip = ballBreak;
                audioSource.Play();
                break;
            case AudioName.coinCollected:
                audioSource.clip = coinCollected;
                audioSource.Play();
                break;
            case AudioName.congratulation:
                audioSource.clip = congratulation;
                audioSource.Play();
                break;


            default:
                break;
        }
    }


}
