using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameOverReturn : MonoBehaviour
{
    public float delay = 10f;

    void Start()
    {
        StartCoroutine(AutoReturn());
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            GoToTitle();
    }

    IEnumerator AutoReturn()
    {
        yield return new WaitForSeconds(delay);
        GoToTitle();
    }

    void GoToTitle()
    {
        StopAllCoroutines();
if (SceneFader.Instance != null)
            SceneFader.Instance.FadeTo("TitleScene");
        else
            SceneManager.LoadScene("TitleScene");
    }
}
