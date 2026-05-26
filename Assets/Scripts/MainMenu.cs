using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string firstLevelName;

    [Tooltip("Optional title text to display 'Class Survival'.")]
    public TMP_Text gameTitleText;

    [SerializeField] private GameObject creditPanel;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject creditButton;

    void Start()
    {
        if (gameTitleText != null)
            gameTitleText.text = "Class Survival";
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Class Survival");
    }

    public void OpenCredit()
    {
        Debug.Log("Open Credit");

        // FIX: gọi thẳng — không cần Reflection vì cùng project
        if (ChangeBGM.Instance != null)
            ChangeBGM.Instance.PlayCredit();

        if (creditPanel != null) creditPanel.SetActive(true);
        SetButtonActive(false);
    }

    public void CloseCredit()
    {
        Debug.Log("Close Credit");

        if (ChangeBGM.Instance != null)
            ChangeBGM.Instance.PlayMainMenu();

        if (creditPanel != null) creditPanel.SetActive(false);
        SetButtonActive(true);
    }

    private void SetButtonActive(bool active)
    {
        if (startButton  != null) startButton.GetComponent<Button>().interactable  = active;
        if (quitButton   != null) quitButton.GetComponent<Button>().interactable   = active;
        if (creditButton != null) creditButton.GetComponent<Button>().interactable = active;
    }
}