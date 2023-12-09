using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Slider volumeSlider;
    public void StartGame()
    {
        SceneManager.LoadScene("Cg01Test");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnSliderValueChanged()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }


}
