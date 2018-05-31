using UnityEngine;
[System.Serializable]
public class LocalizationData{
    public LocalizationConversations[] conversations;
	
}
[System.Serializable]
public class LocalizationConversations
{
    public string key;
    public ConText[] text;

}
[System.Serializable]
public class ConText
{
    public string name;
    [TextArea(3,10)]
    public string theText;

}
