using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bar : MonoBehaviour {

    public float Min;
    public float Max;
    public RectTransform rect;
	
	public void changeValue (float Value) {
        rect.anchorMax = new Vector2(Mathf.Lerp(Min,Max,Value),rect.anchorMax.y);
		
	}
}
