using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
        else if (Input.GetButtonDown("respawn"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}
}
