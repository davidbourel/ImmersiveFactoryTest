using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    
    [SerializeField] private GameObject infoStartGame;
    [SerializeField] private SettingsMenu settingsMenu;

    [SerializeField] private GameObject HitPlayerImage;
    [SerializeField] private GameObject leftWallImage;
    [SerializeField] private GameObject RightWallImage;

    [SerializeField] private Text LastScoreText;
    [SerializeField] private Text BestScoreText;
    [SerializeField] private Text LifeLastRun;

    private static CanvasManager instance;

    public static CanvasManager Instance
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

    private void Start()
    {
        BestScoreText.text = PlayerPrefs.GetFloat("HighScore").ToString("F");
    }

    public void RemovePlayerPref(){
        PlayerPrefs.DeleteAll();
    }

    public void EnableLeftWallImage(bool value)
    {
        leftWallImage.SetActive(value);
    }

    public void EnableRightWallImage(bool value)
    {
        RightWallImage.SetActive(value);
    }

    public void EnableTextToPlay(bool value)
    {
        infoStartGame.SetActive(value);
    }

    public void EnableSettingMenu(bool value)
    {
        settingsMenu.gameObject.SetActive(value);
    }

    public void SetLifeLastRun(int life){
        LifeLastRun.text = life.ToString();
    }

    public void SetLastScore(float lastScore){
        LastScoreText.text = lastScore.ToString("F");
        SetBestScore(lastScore);
    }

    public void SetBestScore(float score)
    {
        float bestScore = PlayerPrefs.GetFloat("HighScore");
        if (score < bestScore || bestScore == 0)
        {
            PlayerPrefs.SetFloat("HighScore", score);
            BestScoreText.text = score.ToString("F");
        }
    }

    public void PlayerHit(bool value)
    {
        HitPlayerImage.SetActive(value);
    }
}
