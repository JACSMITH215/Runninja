using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public AudioClip buttonSound;
    private bool buttonPressed;
    public Image fadePanel;
    public Image creditPanel;
    
    void Start(){
        fadePanel.gameObject.SetActive(false);
        fadePanel.color = new Color(0,0,0,0);
        creditPanel.gameObject.SetActive(false);
        buttonPressed = false;
    }

    void Update(){
        if(buttonPressed){
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = new Color(0,0,0,Mathf.Clamp(fadePanel.color.a+2f*Time.deltaTime,0,1f));
        }
    }

    public void StartGame(){
        if(buttonPressed) return;

        buttonPressed = true;

        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
        Invoke(nameof(StartScene), 1f);
    }

    public void StartScene(){
        SceneManager.LoadScene("Level1");
    }

    public void OpenCredits(){
        if(buttonPressed) return;

        creditPanel.gameObject.SetActive(true);
    }

    public void CloseCredits(){
        if(buttonPressed) return;

        creditPanel.gameObject.SetActive(false);
    }

    public void ExitGame(){
        if(buttonPressed) return;

        buttonPressed = true;

        #if UNITY_EDITOR
            //Solo se ejecuta cuando estamos en Unity
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE_WIN
            //Solo se ejecuta cuando estamos en eun aplicativo de windows (.exe)
            Application.Quit();
        #endif
    }
}
