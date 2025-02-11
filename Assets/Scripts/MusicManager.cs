using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;

    public AudioClip bgm;

    void Start()
    {
        musicSource.clip = bgm;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    void Update(){
        if(GameManager.instance != null){
            if(GameManager.instance.pauseMenu.activeSelf){
                if(musicSource.volume != 0.2f){
                    musicSource.volume = 0.2f;
                }
            }else{
                if(musicSource.volume != 0.5f){
                    musicSource.volume = 0.5f;
                }
            }
        }
    }
}
