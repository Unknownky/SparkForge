using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 该脚本用于控制时间轴的播放
/// </summary>
public class Director : MonoBehaviour
{
    private PlayableDirector director;

    private int speedRate = 2;
    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (director.state == PlayState.Paused)
        {
            director.Resume();
#if UNITY_EDITOR
            Debug.Log("Paused");
#endif
        }
        if (director.state == PlayState.Playing)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                speedRate += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                speedRate -= 1;
            }
            if (Input.GetKey(KeyCode.Space))
            {
#if UNITY_EDITOR
                Debug.Log(speedRate + "倍速播放");
#endif
                director.playableGraph.GetRootPlayable(0).SetSpeed(speedRate);
            }
            else
            {
                director.playableGraph.GetRootPlayable(0).SetSpeed(1);
            }
        }

    }
}
