using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public AudioSource clickSound;

    public void PlayGame()
    {
        if (clickSound != null) clickSound.Play();
        SceneFader.Instance.FadeTo("FishOrDieV2");
    }

    public void LoadTitleScreen()
    {
        if (clickSound != null) clickSound.Play();
        SceneFader.Instance.FadeTo("TitleScene");
    }
}