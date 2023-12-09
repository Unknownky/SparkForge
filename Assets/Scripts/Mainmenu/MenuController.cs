using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Slider volumeSlider;

    private void Awake() {
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1);
    }

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
        AudioListener.volume = volumeSlider.value;
    }


}
