using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector[] directors;
    private int currentDirectorIndex = 0;
    public TMP_Text skipText;
    public float requiredHoldTime = 3f;
    public float skipDelay = 1f;
    public float finishedTipDelay = 1f;

    public enum CutsceneState { Playing, Finished, AllComplete }
    private CutsceneState currentState = CutsceneState.Playing;

    private bool isSkipActive = false;
    private float skipTimer = 0f;
    private bool wasInputPressed = false;
    private float timelineStartTime = 0f;
    private float finishedTime = 0f;

    void Start()
    {
        if (directors == null || directors.Length == 0)
        {
            Debug.LogError("No PlayableDirectors assigned to the directors array!");
            return;
        }
        StartCurrentCutscene();
    }
    
    void OnTimelineStopped(PlayableDirector director)
    {
        currentState = CutsceneState.Finished;
        finishedTime = Time.time;
        HideSkipUI();
    }

    void Update()
    {
        if (currentState == CutsceneState.AllComplete) return;
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
                    StartSkipProcess();
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
                CancelSkipProcess();
            return;
        }

        skipTimer += Time.deltaTime;
        UpdateSkipUI();
        if (skipTimer >= requiredHoldTime)
            ExecuteSkip();
    }

    void StartSkipProcess()
    {
        isSkipActive = true;
        skipTimer = 0f;
        ShowSkipUI();
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
        
        if (currentDirectorIndex < directors.Length)
        {
            StartCurrentCutscene();
        }
        else
        {
            currentState = CutsceneState.AllComplete;
            SceneManager.LoadScene("commit");
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
            HideSkipUI();
        }
    }

    void UpdateFinishedTip()
    {
        if (currentState == CutsceneState.Finished)
        {
            if (Time.time - finishedTime >= finishedTipDelay && skipText != null)
            {
                skipText.gameObject.SetActive(true);
                skipText.text = "Press Enter to continue";
            }
        }
    }

    void ShowSkipUI()
    {
        if (skipText == null) return;
        skipText.gameObject.SetActive(true);
        skipText.text = "Skipping... hold to skip";
    }

    void UpdateSkipUI()
    {
        if (skipText == null) return;
        float remainingTime = Mathf.Clamp(requiredHoldTime - skipTimer, 0f, requiredHoldTime);
        skipText.text = $"Skipping in {Mathf.Ceil(remainingTime)}s...";
    }

    void HideSkipUI()
    {
        if (skipText != null)
            skipText.gameObject.SetActive(false);
    }
}