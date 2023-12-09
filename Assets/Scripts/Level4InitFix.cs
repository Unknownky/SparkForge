using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4InitFix : MonoBehaviour
{
    public GameObject player;
    public GameObject shadowPlayer;

    private void Awake() {
        player.GetComponent<PlayerController>().isUnmoveable = false;
        shadowPlayer.GetComponent<PlayerController>().isUnmoveable = true;
    }


}
