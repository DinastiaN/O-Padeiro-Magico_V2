using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuJogo : MonoBehaviour
{
    public void NovoJogo()
    {
        SceneManager.LoadScene("Jogo");
    }

    public void Sair()
    {
        Debug.Log("Fechaste o jogo.");
        Application.Quit();
    }




}
