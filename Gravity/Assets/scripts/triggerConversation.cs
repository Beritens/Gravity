﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class _UnityEventStringArr : UnityEvent<string[]> { }

public class triggerConversation : MonoBehaviour {

    public messageInfo[] messageInfo = new messageInfo[1];
    public bool usePlayerPrefs = false;
    public string PPname;
	public void trigger () {
        int MII = 0;
        if (usePlayerPrefs)
        {
            for(int i = 0; i<messageInfo.Length; i++)
            {
                if(PlayerPrefs.GetInt(PPname)< messageInfo[i + 1].PPvalue)
                {
                    MII = i;
                    PlayerPrefs.SetInt(PPname, PlayerPrefs.GetInt(PPname) + messageInfo[i].PPvalue);
                }
            }
        }
        _UnityEventStringArr[] methodThing = null;
        if (messageInfo[MII].method.Length != 0)
        {
            methodThing = messageInfo[MII].method;
        }
        conversationManager.instance.displayText(messageInfo[MII].key, messageInfo[MII].index, messageInfo[MII].minIndex, messageInfo[MII].maxIndex, methodThing);
	}
    public void triggerSkip()
    {
        int MII = 0;
        if (usePlayerPrefs)
        {
            for (int i = 0; i < messageInfo.Length; i++)
            {
                if (PlayerPrefs.GetInt(PPname) < messageInfo[i + 1].PPvalue)
                {
                    MII = i;
                    PlayerPrefs.SetInt(PPname, PlayerPrefs.GetInt(PPname) + messageInfo[i].PPvalue);
                }
            }
        }
        conversationManager.instance.skipAdvanced(messageInfo[MII].key, messageInfo[MII].index, messageInfo[MII].minIndex, messageInfo[MII].maxIndex);
    }
}

[System.Serializable]
public class messageInfo
{
    public string key;
    [Tooltip("bei skip wird es um so viel geskipt")]
    public int index;
    [Tooltip("-1 bei conversations start")]
    public int minIndex = 0;
    public int maxIndex = 0;
    public int PPPlus = 0;
    public int PPvalue = 0;
    public _UnityEventStringArr[] method;

}
