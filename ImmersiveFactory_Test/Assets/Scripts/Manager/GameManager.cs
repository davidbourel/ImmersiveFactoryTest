using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    [SerializeField] private Transform startPosition;
    [SerializeField] private GameObject player;

    private PlayerMovement playerMovement;
    private PlayerController playerController;
    private PlayerCamera playerCamera;

    private bool gameOnPause = true;
    private bool onStartMenu = true;

    private static GameManager instance;

    private bool couroutineIsRunning = false;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerController = player.GetComponent<PlayerController>();
        playerCamera = Camera.main.GetComponent<PlayerCamera>();

        SetupGameMenu();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SetupStartPosition(); 
        }
        if (Input.GetKeyDown(KeyCode.F) && !onStartMenu)
        {
            SetupPauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Space) && onStartMenu && !couroutineIsRunning)
        {
            onStartMenu = false;
            StartCoroutine(StartDelay());
        }

    }

    public void PlayerDie()
    {
        ScoreManager.Instance.CanCount = false;
        gameOnPause = true;
        EnableCursor(true);
        DisableControl();
        CanvasManager.Instance.EnableSettingMenu(true);
        Time.timeScale = 0;
        SetupGameMenu();
    }

    public void SetupGameMenu()
    {
        EnableCursor(false);
        SetupStartPosition();
        CanvasManager.Instance.EnableSettingMenu(false);
        CanvasManager.Instance.EnableTextToPlay(true);
        CanvasManager.Instance.EnableLeftWallImage(false);
        CanvasManager.Instance.EnableRightWallImage(false);
        onStartMenu = true;
    }

    public void EnableCursor(bool isVisible)
    {
        if(isVisible)
        {
            LockCursor(false);
            Cursor.visible = true;
        }
        else{
            LockCursor(true);
            Cursor.visible = false;
        }
    }

    public void SetupPauseMenu()
    {
        if(gameOnPause)
        {
            gameOnPause = false;
            EnableCursor(false);
            EnableControl();
            CanvasManager.Instance.EnableSettingMenu(false);
            ScoreManager.Instance.CanCount = true;
            Time.timeScale = 1;
        }
        else{
            gameOnPause = true;
            EnableCursor(true);
            DisableControl();
            CanvasManager.Instance.EnableSettingMenu(true);
            ScoreManager.Instance.CanCount = false;
            Time.timeScale = 0;
        }
    }

    public void SetupStartPosition()
    {
        playerController.ResetPlayerAfterDie(startPosition);
        playerMovement.ResetPlayerVelocity();
        EnableCursor(false);
        ScoreManager.Instance.ResetTimer(0);
    }

    public void DisableControl()
    {
        playerMovement.PlayerCanMove = false;
        playerCamera.CameraCanMove = false;
    }

    public void EnableControl()
    {
        playerMovement.PlayerCanMove = true;
        playerCamera.CameraCanMove = true;
    }

    public void LockCursor(bool islock)
    {
        if (islock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ChangeSensibility(float newSensibility)
    {
        playerCamera.MouseSensibility = newSensibility;
    }



    public void PlayerLoseLife(int life)
    {
        ScoreManager.Instance.UpdateLifePlayer(life);
    }

    public void EndGame()
    {
        ScoreManager.Instance.CanCount = false;
        gameOnPause = true;
        EnableCursor(true);
        DisableControl();
        CanvasManager.Instance.EnableSettingMenu(true);
        Time.timeScale = 0;
        CanvasManager.Instance.SetLastScore(ScoreManager.Instance.MainTimer);
        CanvasManager.Instance.SetLifeLastRun(playerController.CurrentLifeNumber);
        SetupGameMenu();
    }



    IEnumerator StartDelay()
    {
        couroutineIsRunning = true;

        CanvasManager.Instance.EnableTextToPlay(false);
        float pauseTime = Time.realtimeSinceStartup + 1f;
        while(Time.realtimeSinceStartup < pauseTime)
        {
            yield return 0;
        }
        SetupPauseMenu();
        ScoreManager.Instance.CanCount = true;

        couroutineIsRunning = false;
    }

}
