using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
        
    public void StartGame()
    {
        SceneManager.LoadScene("Cg01Test");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
