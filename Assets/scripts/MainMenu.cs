using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Restart()
    {
        FindObjectOfType<GameManager>().restartGame();
    }
}