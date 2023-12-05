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

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void Update() {
        if(director.state == PlayState.Paused)
        {
            director.Resume();
            #if UNITY_EDITOR
            Debug.Log("Paused");
            #endif
        }
    }
}
