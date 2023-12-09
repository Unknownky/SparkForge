using UnityEngine;

public class Level4InitFix : MonoBehaviour
{
    public GameObject player;

    public GameObject shadowPlayer;


    private void Awake() {
        player.GetComponent<PlayerController>().isUnmoveable = false;    // 玩家是否可以移动
        shadowPlayer.GetComponent<PlayerController>().isUnmoveable = true;    // 玩家是否可以移动
    }

    public void ChangeControl()
    {
        PlayerController playerController = GameObject.Find("player").GetComponent<PlayerController>();
        PlayerController shadowPlayerController = GameObject.Find("shadowplayer").GetComponent<PlayerController>();
        playerController.isUnmoveable = !playerController.isUnmoveable;//更改玩家的可移动状态
        shadowPlayerController.isUnmoveable = !shadowPlayerController.isUnmoveable;//更改影子的可移动状态
#if UNITY_EDITOR
        Debug.Log("反转控制");
#endif
    }
}
