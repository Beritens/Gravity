using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bar : MonoBehaviour {

    public float Min;
    public float Max;
    public bool vertical = false;
    public RectTransform rect;
	
	public void changeValue (float Value) {
        if(!vertical)
            rect.anchorMax = new Vector2(Mathf.Lerp(Min,Max,Value),rect.anchorMax.y);
        else
            rect.anchorMax = new Vector2(rect.anchorMax.x, Mathf.Lerp(Min, Max, Value));

    }
}
