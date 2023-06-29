using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuPausa;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelSalvar;

    private bool menuAberto = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuAberto)
                AbrirMenu();
            else
                FecharMenu();
                FecharOpcoes();
                FecharSalvar();
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
        painelMenuPausa.SetActive(false);
        menuAberto = false;

        if (CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
    }

    public void FecharOpcoes()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);
        painelOpcoes.SetActive(false);
    }

    public void AbrirSalvar()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);
        painelSalvar.SetActive(true);
    }

    public void FecharSalvar()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.menuButton);
        painelSalvar.SetActive(false);
    }
}