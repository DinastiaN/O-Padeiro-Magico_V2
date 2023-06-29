using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuJogo : MonoBehaviour
{
    public Button LoadGameBTN;


    private void Start()
    {
        LoadGameBTN.onClick.AddListener(() =>
        {
            SaveManager.Instance.StartLoadedGame();
        });
    }

    public void NovoJogo()
    {
        SceneManager.LoadScene("Jogo");
    }

    public void Sair()
    {
        Debug.Log("Fechaste o jogo.");
        Application.Quit();
    }

    public void Som()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);
        Debug.Log("O botão está a fazer barulho, I HOPE");
    }

}
