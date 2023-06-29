using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    string jsonPathProject;

    string jsonPathPersistant;

    string binaryPath;


    public bool isSavingToJson;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        jsonPathPersistant = Application.persistentDataPath + "SaveGame.json";
        binaryPath = Application.persistentDataPath + "/save_game.bin";
    }

    public void SaveGame()
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        SavingTypeSwitch(data);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[2];
        playerStats[0] = PlayerStats.Instance.currentHealth;
        playerStats[1] = PlayerStats.Instance.currentMana;

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerStats.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerStats.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerStats.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerStats.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerStats.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerStats.Instance.playerBody.transform.rotation.z;

        return new PlayerData(playerStats, playerPosAndRot);
    }

    public void SavingTypeSwitch(AllGameData gameData)
    {
        if (isSavingToJson)
        {
            SaveGameDataToJsonFiles(gameData);
        }
        else
        {
            SaveGameDataToBinaryFiles(gameData);
        }
    }

    public AllGameData LoadingTypeSwitch()
    {
        if (isSavingToJson)
        {
            AllGameData gameData = LoadGameDataFromJsonFile();
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
    }

    public void LoadGame()
    {
        SetPlayerData(LoadingTypeSwitch().playerData);
    }


    private void SetPlayerData(PlayerData playerData)
    {
        PlayerStats.Instance.currentHealth = playerData.playerStats[0];
        PlayerStats.Instance.currentMana = playerData.playerStats[1];


        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        PlayerStats.Instance.playerBody.transform.position = loadedPosition;

        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

        PlayerStats.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);


    }

    public void StartLoadedGame()
    {
        SceneManager.LoadScene("Jogo");

        StartCoroutine(DelayedLoading());
    }

    private IEnumerator DelayedLoading()
    {
        yield return new WaitForSeconds(1f);
        LoadGame();

    }

    #region || ----- Binary Testing ----- ||
    public void SaveGameDataToBinaryFiles(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(binaryPath, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Dados guardados: " + binaryPath);
    }

    public AllGameData LoadGameDataFromBinaryFile()
    {
        if (File.Exists(binaryPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Dados carregados de " + binaryPath);

            return data;
        }
        else
        {
            return null;
        }
    }

    #endregion


    #region || ----- Json ----- ||

    public void SaveGameDataToJsonFiles(AllGameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);

        string encrypted = EncryptionDecryption(json);

        using (StreamWriter writer = new StreamWriter(jsonPathProject))
        {
            writer.Write(encrypted);

            print("Jogo guardado num ficheiro Json em " + jsonPathProject);
        };
    }

    public AllGameData LoadGameDataFromJsonFile()
    {
        using (StreamReader reader = new StreamReader(jsonPathProject))
        {
            string json = reader.ReadToEnd();

            string decrypted = EncryptionDecryption(json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted);
            return data;
        };
    }

    #endregion


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


    #region || ----- Encryption ----- ||

    public string EncryptionDecryption(string jsonString)
    {
        string keyword = "1234567";

        string result = "";

        for (int i = 0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }

        return result;

    }






    #endregion
}
