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
    bool typing = false;
    _UnityEventStringArr[] currentMethods;
    ConText currentMessage;

    public Animator conPanelAnim;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI text;
    public Image image;
    public float textSpeed;
    public GameObject skipThing;
    
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
        if(methods != null)
            currentMethods = methods;
        ConText[] conver = LocalizationManager.instance.GetConText(key);
        if(conver != null && conver.Length >= index)
            currentMessage = conver[index];
        
        else
        {
            currentMessage = new ConText();
            currentMessage.doSomething = false;
            currentMessage.end = true;
            currentMessage.name = "<color=\"red\" > error";
            currentMessage.parameters = new string[0];
            currentMessage.skipable = true;
            currentMessage.spriteKey = "error";
            currentMessage.theText = "<color=\"red\" > error";
        }
        //print(message.name + " says: " + message.theText);
        image.sprite = sprites[currentMessage.spriteKey];
        nameText.text = currentMessage.name;
        text.text = "";
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = typetext(currentMessage.theText, text, textSpeed);
        StartCoroutine(coroutine);
        
        if (currentMessage.end == true)
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
    public void skip(int howMuch, bool needsToBeSkipable)
    {
        if (inConversation && (currentMessage.skipable || !needsToBeSkipable))
            displayText(currentKey, currentIndex + howMuch, 0, currentIndex + howMuch + 1, null);
        else if(!inConversation)
        {
            skipThing.SetActive(false);
            conPanelAnim.SetBool("isOpen", false);
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("use"))
        {
            if (typing)
            {
                completeType();
            }
            else
                skip(1,true);
        }
    }
    IEnumerator typetext(string text, TextMeshProUGUI textField, float speed)
    {
        skipThing.SetActive(false);
        typing = true;
        bool ok = true;
        foreach(char letter in text)
        {
            textField.text += letter;
            if(letter == '<')
            {
                ok = false;
            }
            
            if (ok)
                yield return new WaitForSeconds(speed);
            if (letter == '>')
            {
                ok = true;
            }
        }
        if (currentMessage.doSomething)
        {
            currentMethods[currentMessage.method].Invoke(currentMessage.parameters);
        }
        typing = false;
        if (currentMessage.skipable)
        {
            skipThing.SetActive(true);
        }
    }
    void completeType()
    {
        StopCoroutine(coroutine);
        text.text = currentMessage.theText;
        if (currentMessage.skipable)
        {
            skipThing.SetActive(true);
        }
        if (currentMessage.doSomething)
        {
            currentMethods[currentMessage.method].Invoke(currentMessage.parameters);
        }
        typing = false;
    }
    
    public void skipAdvanced(string key, int howMuch, int MinIndex, int MaxIndex)
    {
        if (inConversation)
        {
            displayText(key, currentIndex + howMuch, MinIndex, MaxIndex, null);
        }
    }
    //UnityEvents can only use one argument :/
    public void skipForTrigger(int howMuch)
    {
        skip(howMuch, false);
    }
    public void skipOne(bool needsToBeSkipable)
    {
        skip(1, needsToBeSkipable);
    }
}
