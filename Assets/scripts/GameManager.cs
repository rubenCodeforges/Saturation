using System.Collections;
using System.Collections.Generic;
using Http;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject levelCompleteMenu;

    private void Awake()
    {

    }

    public void endLevel()
    {
        levelCompleteMenu.SetActive(true);
    }
    
    public void endGame()
    {
        gameOverMenu.SetActive(true);
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