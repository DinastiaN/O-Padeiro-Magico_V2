using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }

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

        DontDestroyOnLoad(gameObject);
    }

    public void SaveGameDataToBinaryFiles(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save_game.bin";
        FileStream stream = new FileStream(path, FileMode.Open);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Dados guardados" + Application.persistentDataPath + "/save_game.bin");
    }

    public AllGameData LoadGameDataFromBinaryFile()
    {
        string path = Application.persistentDataPath + "/save_game.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    [System.Serializable]
    public class VolumeSettings
    {
        public float musica;
        public float efeitos;
        public float geral;
    }

    public void SaveVolumeSettings(float _musica, float _efeitos, float _geral)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            musica = _musica,
            efeitos = _efeitos,
            geral = _geral
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();
    }

    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
    }
}
