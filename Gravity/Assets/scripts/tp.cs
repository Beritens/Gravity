using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tp : MonoBehaviour {
    public Transform player;
    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void teleport()
    {
        player.position = transform.position;
    }
}
