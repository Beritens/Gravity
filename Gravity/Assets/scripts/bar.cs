using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bar : MonoBehaviour {

    public Image theBar;
    public Text text;
    public bool onlyValue = false;
    public static bool numbersOn = true;
	
	public void changeValue (float Value, float MaxValue) {
        theBar.fillAmount = Value/MaxValue;
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
}
