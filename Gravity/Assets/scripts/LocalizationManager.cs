using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager instance;
    Dictionary<string, ConText[]> Lconversations;
    bool ready = false;
    public string defaultFileName;
    [TextArea(3,5)]
    public string WebGlText;

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
        LoadLocalizedText(defaultFileName);
	}
	
	public void LoadLocalizedText(string fileName) {
        Lconversations = new Dictionary<string, ConText[]>();

        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        //#if UNITYWEBGL
        //Resources.Load(fileName) as TextAsset;
        string dataAsJson;
        //#endif
        if (File.Exists(filePath))
        {
            dataAsJson = File.ReadAllText(filePath);
        }
        else
        {
            print("hello");
            dataAsJson = WebGlText;
        }
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.conversations.Length; i++)
            {
                Lconversations.Add(loadedData.conversations[i].key, loadedData.conversations[i].text);
            }
            Debug.Log("data loaded, dictionary contains " + Lconversations.Count + " entries");
        //}
        //else
        //{
           // print("fail");
        //}
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
