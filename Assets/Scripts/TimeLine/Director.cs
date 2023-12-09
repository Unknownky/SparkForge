using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
        StartCoroutine(LoadMainScene());
        
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

    private IEnumerator LoadMainScene(){
        yield return new WaitForSeconds(16f);
        SceneManager.LoadScene("Level_0-1");

    }
}
