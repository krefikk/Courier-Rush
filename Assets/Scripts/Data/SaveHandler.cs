using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveHandler
{
    private string saveFilePath;
    private string saveFileName;
    private string encryptionCodeWord = "courier";
    private bool encryption = true;

    public SaveHandler(string saveFilePath, string saveFileName) 
    {
        this.saveFilePath = saveFilePath;
        this.saveFileName = saveFileName;
    }

    public GameData Load() 
    {
        string fullPath = Path.Combine(saveFilePath, saveFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath)) 
        { // Returns null if there is no save file yet
            try
            {
                string loadStore = "";
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream)) { loadStore = reader.ReadToEnd(); }
                }
                if (encryption) { loadStore = EncryptDecrypt(loadStore); }
                loadedData = JsonUtility.FromJson<GameData>(loadStore);
            }
            catch (Exception e)
            {
                Debug.Log("Error occured while saving data: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data) 
    {
        // Used Path.Combine instead of saveFilePath + "/" + saveFileName because other OS's may have different syntaxs than Windows' syntax
        string fullPath = Path.Combine(saveFilePath, saveFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string saveStore = JsonUtility.ToJson(data, true);
            if (encryption) { saveStore = EncryptDecrypt(saveStore); }
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create)) 
            {
                using (StreamWriter writer = new StreamWriter(fileStream)) { writer.Write(saveStore); }
            }
        }
        catch (Exception e) 
        {
            Debug.Log("Error occured while saving data: " + fullPath + "\n" + e);
        }
    }

    public void DeleteSaveFile() 
    {
        File.Delete(Path.Combine(saveFilePath, saveFileName));
    }

    public string EncryptDecrypt(string data) 
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++) 
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
