using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance {get; set;}


    public AudioSource menuButton;
    public AudioSource walkTerrain;
    public AudioSource walkStone;
    public AudioSource fireball;
    public AudioSource abrirmenu;

    public AudioSource mainMenuM;
    public AudioSource MusicaComeco;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
}
