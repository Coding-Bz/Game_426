using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public string winSceneName = "Winning";
    public string loseSceneName = "Loosing";

    private bool isGameOver = false;

    void Update()
    {
        if (!isGameOver && player1 == null && player2 == null)
        {
            LoseGame();
        }
    }

    public void WinGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            SceneManager.LoadScene(winSceneName);
        }
    }

    private void LoseGame()
    {
        isGameOver = true;
        SceneManager.LoadScene(loseSceneName);
    }
}