using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlag : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "player1" || other.gameObject.name == "player2")
        {
            SceneManager.LoadScene("Winning");
        }
    }
}
