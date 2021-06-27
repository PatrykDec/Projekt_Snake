using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;

public class PauseWindow : MonoBehaviour
{

    private static PauseWindow instance;

    private void Awake()
    {
        instance = this;

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        transform.Find("resumeBtn").GetComponent<Button_UI>().ClickFunc = () => GameHendler.ResumeGame();

        transform.Find("mainMenuBtn").GetComponent<Button_UI>().ClickFunc = () => SceneManager.LoadScene(0);

        Hide();
    
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowStatic()
    {
        instance.Show();
    }

    public static void HideStatic()
    {
        instance.Hide();
    }
}
