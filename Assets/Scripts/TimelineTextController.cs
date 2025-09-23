using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector[] directors;
    private int currentDirectorIndex = 0;

    void Start()
    {
        if (directors != null && directors.Length > 0 && directors[0] != null)
        {
            directors[0].Play();
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            NextCutscene();
        }
    }

    void NextCutscene()
    {
        if (directors == null) return;

        if (currentDirectorIndex < directors.Length && directors[currentDirectorIndex] != null)
        {
            directors[currentDirectorIndex].Stop();
        }

        currentDirectorIndex++;

        if (currentDirectorIndex < directors.Length && directors[currentDirectorIndex] != null)
        {
            directors[currentDirectorIndex].Play();
        }
        else
        {
            Debug.Log("Alle Cutscenes fertig!");
        }
    }
}
