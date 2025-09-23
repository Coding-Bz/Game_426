using UnityEngine;
using UnityEngine.SceneManagement; 

public class WelcomePage : MonoBehaviour
{
   public void PlayGame()
   {   
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       Debug.Log("Playing Game!"); 
   }
   public void QuitGame()
   {
       Application.Quit();
       Debug.Log("Quitting Game!"); 
       #if UNITY_EDITOR
           UnityEditor.EditorApplication.isPlaying = false;
       #endif
   }
}