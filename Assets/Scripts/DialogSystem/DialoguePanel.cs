using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    private Animator animator;  //对话框的动画控制器

    //显示对话框时进行的操作
    private void OnEnable() {
        animator = GetComponent<Animator>();
        //TODO
        Debug.Log("对话框显示");
        animator.Play("fadein");
    }

    //隐藏对话框时进行的操作
    private void OnDisable() {
        //TODO
        Debug.Log("对话框隐藏");
    }

}
