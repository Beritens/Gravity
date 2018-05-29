using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bar : MonoBehaviour {

    public Image theBar;
    public TextMeshProUGUI text;
    public bool onlyValue = false;
    public static bool numbersOn = true;
    public bool spriteStuff = false;
    public Sprite[] sprites;
    
	
	public void changeValue (float Value, float MaxValue) {
        if(!spriteStuff){
           theBar.fillAmount = Value/MaxValue; 
        }
        else{
            theBar.sprite = sprites[spriteThing(Value/MaxValue,sprites.Length)];
        }
        if (numbersOn)
        {
            if (onlyValue)
            {
                text.text = Value.ToString();
            }
            else
            {
                text.text = string.Format("{0:0.##}/{1:0.##}", Value, MaxValue);
            }
        }

    }
    int spriteThing(float value, int quantity){
        for(int i = 0; i<quantity; i++){
            if(value <= (float)(i +1)/ (float)quantity){
                return i;
            }
        }
        return 0;
    }
}
