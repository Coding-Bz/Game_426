using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    [Header("Timeline Settings")]
    public PlayableDirector[] directors;
    private int currentDirectorIndex = 0;

    [Header("UI")]
    public TMP_Text skipText;

    [Header("Skip Settings")]
    public float requiredHoldTime = 3f; // Zeit, die man halten muss
    public float skipDelay = 1f; // Sekunden, in denen Skip am Anfang deaktiviert ist
    public float finishedTipDelay = 1f; // Zeit nach Ende, wann "Press Enter" angezeigt wird

    // States
    public enum CutsceneState
    {
        Playing,
        Finished,
        AllComplete
    }

    private CutsceneState currentState = CutsceneState.Playing;

    // Skip system
    private bool isSkipActive = false;
    private float skipTimer = 0f;

    // Input tracking
    private bool wasInputPressed = false;

    // Startzeit der aktuellen Timeline
    private float timelineStartTime = 0f;
    private float finishedTime = 0f; // Zeitpunkt, wann Timeline fertig wurde

    void Start()
    {
        InitializeUI();
        StartCurrentCutscene();
    }

    void Update()
    {
        if (currentState == CutsceneState.AllComplete)
            return;

        HandleInput();
        UpdateSkipSystem();
        UpdateFinishedTip();
    }

    void HandleInput()
    {
        bool inputPressed = IsSkipInputPressed();
        float timeSinceStart = Time.time - timelineStartTime;
        bool canSkip = timeSinceStart >= skipDelay;

        if (inputPressed)
        {
            if (currentState == CutsceneState.Finished)
            {
                NextCutscene();
                return;
            }

            if (currentState == CutsceneState.Playing)
            {
                if (!isSkipActive && canSkip)
                {
                    StartSkipProcess();
                }
                else if (isSkipActive)
                {
                    ContinueSkipProcess();
                }
            }
        }
        else if (!inputPressed && wasInputPressed && currentState == CutsceneState.Playing)
        {
            CancelSkipProcess();
        }

        wasInputPressed = inputPressed;
    }

    bool IsSkipInputPressed()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        return (keyboard != null && (keyboard.enterKey.isPressed || keyboard.spaceKey.isPressed))
               || (mouse != null && mouse.leftButton.isPressed);
    }

    void UpdateSkipSystem()
    {
        if (!isSkipActive || currentState != CutsceneState.Playing)
        {
            if (isSkipActive && currentState != CutsceneState.Playing)
            {
                CancelSkipProcess();
            }
            return;
        }

        skipTimer += Time.deltaTime;
        UpdateSkipUI();

        if (skipTimer >= requiredHoldTime)
        {
            ExecuteSkip();
        }
    }

    void StartSkipProcess()
    {
        isSkipActive = true;
        skipTimer = 0f;
        ShowSkipUI();
    }

    void ContinueSkipProcess()
    {
        // Timer läuft automatisch in UpdateSkipSystem()
    }

    void CancelSkipProcess()
    {
        isSkipActive = false;
        skipTimer = 0f;
        HideSkipUI();
    }

    void ExecuteSkip()
    {
        isSkipActive = false;
        skipTimer = 0f;
        HideSkipUI();

        NextCutscene();
    }

    void NextCutscene()
    {
        if (currentDirectorIndex < directors.Length && directors[currentDirectorIndex] != null)
        {
            directors[currentDirectorIndex].stopped -= OnTimelineStopped;
            directors[currentDirectorIndex].Stop();
        }

        currentDirectorIndex++;
        currentState = CutsceneState.Playing;

        if (currentDirectorIndex < directors.Length)
        {
            StartCurrentCutscene();
        }
        else
        {
            currentState = CutsceneState.AllComplete;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void StartCurrentCutscene()
    {
        if (currentDirectorIndex < directors.Length && directors[currentDirectorIndex] != null)
        {
            var currentDirector = directors[currentDirectorIndex];
            currentDirector.stopped += OnTimelineStopped;
            currentDirector.Play();
            currentState = CutsceneState.Playing;
            timelineStartTime = Time.time;
        }
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        if (directors[currentDirectorIndex] == director && currentState == CutsceneState.Playing)
        {
            currentState = CutsceneState.Finished;
            finishedTime = Time.time;
        }
    }

    void UpdateFinishedTip()
    {
        if (currentState == CutsceneState.Finished)
        {
            if (Time.time - finishedTime >= finishedTipDelay)
            {
                if (skipText != null)
                {
                    skipText.gameObject.SetActive(true);
                    skipText.text = "Press Enter to continue";
                }
            }
        }
    }

    // UI Methods
    void InitializeUI()
    {
        if (skipText != null)
        {
            skipText.gameObject.SetActive(false);
        }
    }

    void ShowSkipUI()
    {
        if (skipText == null) return;

        skipText.gameObject.SetActive(true);

        string message = currentState == CutsceneState.Playing ?
            "Skip cutscene? (Hold for 3s)" :
            "Next cutscene? (Hold for 3s)";

        skipText.text = message;
    }

    void UpdateSkipUI()
    {
        if (skipText == null) return;

        float remainingTime = Mathf.Clamp(requiredHoldTime - skipTimer, 0f, requiredHoldTime);
        string action = currentState == CutsceneState.Playing ? "Skipping" : "Next";
        skipText.text = $"{action} in {Mathf.Ceil(remainingTime)}s...";
    }

    void HideSkipUI()
    {
        if (skipText != null)
        {
            skipText.gameObject.SetActive(false);
        }
    }

    public void ForceNextCutscene()
    {
        NextCutscene();
    }

    public CutsceneState GetCurrentState()
    {
        return currentState;
    }
}
