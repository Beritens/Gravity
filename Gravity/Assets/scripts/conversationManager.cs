using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conversationManager : MonoBehaviour {

    public static conversationManager instance;
    int currentIndex = -1;
    string currentKey = "";
    bool inConversation = false;
    
	void Start () {
        instance = this;
	}
	
	public void displayText(string key, int index, int MinIndex, int MaxIndex)
    {
        if ((key != currentKey && inConversation) || currentIndex<MinIndex || (currentIndex>MaxIndex))
        {
            print("peaaaaaaaaaaaace");
            return;
            
        }
            
        currentIndex = index;
        currentKey = key;
        ConText message = LocalizationManager.instance.GetConText(key)[index];
        print(message.name + " says: " + message.theText);
        inConversation = true;
    }
}
