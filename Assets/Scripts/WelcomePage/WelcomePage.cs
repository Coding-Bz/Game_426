using UnityEngine;
using UnityEngine.SceneManagement; 

public class WelcomePage : MonoBehaviour
{
   public void PlayGame()
   {   
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }
   public void QuitGame()
   {
       Application.Quit();
       #if UNITY_EDITOR
           UnityEditor.EditorApplication.isPlaying = false;
       #endif
   }
}