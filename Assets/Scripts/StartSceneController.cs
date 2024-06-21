using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class StartSceneController : MonoBehaviour
{

    public GameObject aboutPanel;
    public TMP_Text aboutText;
    public Button backButton;
    public Button startButton;
    public Button aboutButton;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        aboutPanel.SetActive(false);
        backButton.gameObject.SetActive(false);

        backButton.onClick.AddListener(OnBackButtonClicked);
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        aboutButton.onClick.AddListener(OnAboutButtonClicked);
        

    }

    public void OnBackButtonClicked()
    {   
        aboutPanel.SetActive(false);
        backButton.gameObject.SetActive(false);
        aboutText.text = "";
        startButton.gameObject.SetActive(true);
        aboutButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
    public void OnAboutButtonClicked()
    {
        aboutPanel.SetActive(true);
        backButton.gameObject.SetActive(true);
        aboutText.text = "This Unity project was created for the AI3618 course assignment. The creators are Zheng Zeyi, Ge Tong, Su Zhe, Xu Zhengyi, and Wang Chenrun. The project is inspired by the shooting range in the Resident Evil 4 Remake. Enjoy the game:D";
        startButton.gameObject.SetActive(false);
        aboutButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

    }



}
