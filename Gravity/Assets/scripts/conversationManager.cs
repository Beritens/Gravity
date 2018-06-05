using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class conversationManager : MonoBehaviour {

    public static conversationManager instance;
    int currentIndex = -1;
    string currentKey = "";
    bool inConversation = false;
    _UnityEventStringArr[] currentMethods;
    bool isskipable;

    public Animator conPanelAnim;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI text;
    public Image image;
    public float textSpeed;
    [System.Serializable]
    public struct NamedImage
    {
        public string key;
        public Sprite image;
    }
    public NamedImage[] images;
    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    IEnumerator coroutine;

    void Start () {
        for(int i = 0; i < images.Length; i++)
        {
            sprites.Add(images[i].key, images[i].image);
        }
        instance = this;
	}
	
	public void displayText(string key, int index, int MinIndex, int MaxIndex, _UnityEventStringArr[] methods)
    {
        
        if ((key != currentKey && inConversation) || currentIndex<MinIndex || currentIndex>MaxIndex)
        {
            return;
            
        }
        inConversation = true;
        conPanelAnim.SetBool("isOpen", true);
        currentIndex = index;
        currentKey = key;
        currentMethods = methods;
        ConText[] conver = LocalizationManager.instance.GetConText(key);
        ConText message;
        if(conver != null && conver.Length >= index)
            message = conver[index];
        
        else
        {
            message = new ConText();
            message.doSomething = false;
            message.end = true;
            message.name = "<color=\"red\" > error";
            message.parameters = new string[0];
            message.skipable = true;
            message.spriteKey = "error";
            message.theText = "<color=\"red\" > error";
        }
        isskipable = message.skipable;
        //print(message.name + " says: " + message.theText);
        image.sprite = sprites[message.spriteKey];
        nameText.text = message.name;
        text.text = "";
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = typetext(message.theText, text, textSpeed);
        StartCoroutine(coroutine);
        if (message.doSomething)
        {
            methods[message.method].Invoke(message.parameters);
        }
        if (message.end == true)
        {
            inConversation = false;
            
            currentIndex = -1;
            currentKey = "";
        }
    }
    public void printOptions(string[] options)
    {
        for(int i = 0; i< options.Length; i += 2)
        {
            print("option " + i/2 + ": " + options[i] + "     method: " + options[i+1]);
        }
    }
    public void skip(int howMuch)
    {
        if (inConversation && isskipable)
            displayText(currentKey, currentIndex + howMuch, 0, currentIndex + howMuch + 1, currentMethods);
        else
        {
            conPanelAnim.SetBool("isOpen", false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp("e"))
        {
            skip(1);
        }
    }
    IEnumerator typetext(string text, TextMeshProUGUI textField, float speed)
    {
        bool ok = true;
        foreach(char letter in text)
        {
            textField.text += letter;
            if(letter == '<')
            {
                ok = false;
            }
            if(letter == '>')
            {
                ok = true;
            }
            if (ok)
                yield return new WaitForSeconds(speed);
        }
    }
}
