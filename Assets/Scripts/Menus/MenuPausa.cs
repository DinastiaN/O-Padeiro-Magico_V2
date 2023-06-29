using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuPausa;
    [SerializeField] private GameObject painelOpcoes;

    private bool menuAberto = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuAberto)
                AbrirMenu();
            else
                FecharMenu();
        }
    }

    public void AbrirMenu()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.abrirmenu);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        painelMenuPausa.SetActive(true);
        menuAberto = true;
    }

    public void FecharMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        painelMenuPausa.SetActive(false);
        menuAberto = false;
    }

    public void SairJogo()
    {
        Debug.Log("Fechar o Jogo.");
        Application.Quit();
    }

    public void ResumirJogo()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);
        FecharMenu();
    }

    public void VoltarMenuInicial()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void AbrirOpcoes()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);
        painelOpcoes.SetActive(true);
        painelMenuPausa.SetActive(false);
    }

    public void FecharOpcoes()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);
        painelOpcoes.SetActive(false);
        painelMenuPausa.SetActive(true);
    }

    public void Salvar()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);



    }
}