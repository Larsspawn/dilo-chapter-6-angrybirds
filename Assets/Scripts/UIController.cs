using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject uiLevelCleared;
    public GameObject uiLevelFailed;
    public GameObject uiPaused;
    public Text scoreText;
    public Text scoreOnLevelClearedText;

    private GameController gameControl;
    private Scene currActiveScene;
    [SerializeField] bool isOnMenu; 

    private void Start()
    {
        gameControl = FindObjectOfType<GameController>();
        currActiveScene = SceneManager.GetActiveScene();

        if (isOnMenu)
            AudioManager.PlayMusicLoop(AudioManager.Sound.mainTheme);
    }

    public void UpdateScore(float value)
    {
        scoreText.text = ("SCORE : " + value.ToString());
        scoreOnLevelClearedText.text = ("SCORE : " + value.ToString());
    }

    public void SetVisibility(GameObject ui, bool state)
    {
        ui.SetActive(state);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(currActiveScene.name);
    }

    public void PauseGame(bool state)
    {
        gameControl.SetPause(state);
        uiPaused.SetActive(state);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
