using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }

    public Button voltarBTN;

    public Slider geralSlider;
    public GameObject geralValue;

    public Slider musicaSlider;
    public GameObject musicaValue;

    public Slider efeitosSlider;
    public GameObject efeitosValue;

    private void Start()
    {
        voltarBTN.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(musicaSlider.value, efeitosSlider.value, geralSlider.value);

            print("Salvar em Player Pref");
        });


        StartCoroutine(LoadAndApplySettings());

    }


    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();


        yield return new WaitForSeconds(0.1f);
    }


    private void LoadAndSetVolume()
    {
        SaveManager.VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();

        geralSlider.value = volumeSettings.geral;
        musicaSlider.value = volumeSettings.musica;
        efeitosSlider.value = volumeSettings.efeitos;

        print("Definições do Som carregadas.");
    }


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


    private void Update()
    {
        geralValue.GetComponent<TextMeshProUGUI>().text = "" + (geralSlider.value) + "";
        musicaValue.GetComponent<TextMeshProUGUI>().text = "" + (musicaSlider.value) + "";
        efeitosValue.GetComponent<TextMeshProUGUI>().text = "" + (efeitosSlider.value) + "";
    }





}