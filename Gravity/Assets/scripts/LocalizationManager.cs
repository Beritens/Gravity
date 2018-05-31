using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager instance;
    Dictionary<string, ConText[]> Lconversations;
    bool ready = false;

	void Start () {
		if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}
	
	public void LoadLocalizedText(string fileName) {
        Lconversations = new Dictionary<string, ConText[]>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for(int i = 0; i< loadedData.conversations.Length; i++)
            {
                Lconversations.Add(loadedData.conversations[i].key, loadedData.conversations[i].text);
            }
            Debug.Log("data loaded, dictionary contains " + Lconversations.Count +" entries");
        }
        else
        {
            Debug.Log("fail");
        }
        ready = true;
	}
    public bool isReady()
    {
        return ready;
    }
    public ConText[] GetConText(string key)
    {
        ConText[] result;
        if (Lconversations.ContainsKey(key))
        {
            result = Lconversations[key];
        }
        else
        {
            result = null;
        }
        return result;
    }
}
