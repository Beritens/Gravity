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
    public string spriteKey;
    [TextArea(3,10)]
    public string theText;
    public bool skipable;
    public bool end;
    public bool doSomething;
    public int method;
    public string[] parameters;

}
