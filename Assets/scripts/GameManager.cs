using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject levelCompleteMenu;
//    public GameObject JoystickUI;

    public void endLevel()
    {
        levelCompleteMenu.SetActive(true);
//        JoystickUI.SetActive(false);

        ScoreManager sm = FindObjectOfType<ScoreManager>();
        sm.ShowPlayerInput();
        sm.disableTimer();
    }

    public void endGame()
    {
        gameOverMenu.SetActive(true);
//        JoystickUI.SetActive(false);
        ScoreManager sm = FindObjectOfType<ScoreManager>();
        sm.ShowScores();
    }

    public void disablePlayerControls()
    {
        FindObjectOfType<CharacterControl>().setControl(false);
    }

    public void restartGame()
    {
        SceneManager.LoadScene("Level01");
        gameOverMenu.SetActive(false);
        levelCompleteMenu.SetActive(false);
    }
}