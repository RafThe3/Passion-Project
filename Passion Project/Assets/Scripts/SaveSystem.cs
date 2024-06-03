using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private float textDuration = 1;

    //private PlayerExp playerExp;

    //private TextMeshProUGUI objectiveText;

    private static readonly string keyWord = "password";
    private string file;

    private void Awake()
    {
        file = $"{Application.persistentDataPath}/{name}.json";
    }

    private void Start()
    {
        progressText.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Delete();
        }
    }

    public void Save()
    {
        SaveData myData = new();

        myData.x = transform.position.x;
        myData.y = transform.position.y;
        myData.z = transform.position.z;

        //Important - DO NOT DELETE
        string myDataString = JsonUtility.ToJson(myData);
        myDataString = EncryptDecryptData(myDataString);
        File.WriteAllText(file, myDataString);
        //

        progressText.text = "Progress saved!";
        StartCoroutine(ShowProgressText(textDuration));
    }

    public void Load()
    {
        //Important - DO NOT DELETE
        if (File.Exists(file))
        {
            string jsonData = File.ReadAllText(file);
            jsonData = EncryptDecryptData(jsonData);
            SaveData myData = JsonUtility.FromJson<SaveData>(jsonData);
            //
            
            transform.position = new Vector3(myData.x, myData.y, myData.z);

            progressText.text = "Progress loaded!";
            StartCoroutine(ShowProgressText(textDuration));
        }
    }

    public string EncryptDecryptData(string data)
    {
        string result = string.Empty;

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ (keyWord[i % keyWord.Length]));
        }

        return result;
    }

    public void Delete()
    {
        File.Delete(file);
    }

    private IEnumerator ShowProgressText(float duration)
    {
        progressText.enabled = true;

        yield return new WaitForSeconds(duration);

        progressText.enabled = false;
    }
}

[Serializable]
public class SaveData
{
    public float x, y, z;
    public int currentExp, maxExp, currentLevel;
    public float currentHealth, maxHealth;
    public float areasConquered;
}
