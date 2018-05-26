using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gate : MonoBehaviour {

	public Vector2 newPosition;
    public float time = 1;
    Vector2 StartPos;
    float t = 0;
    private void Start()
    {
        StartPos = transform.localPosition;
    }
    public void gateOpen () {
        StartCoroutine(open());
	}
    public IEnumerator open()
    {
        while (t < 1)
        {
            t += Time.deltaTime / time;
            transform.localPosition = Vector2.Lerp(StartPos, StartPos + newPosition, t);
            yield return null;
        }
    }
}
