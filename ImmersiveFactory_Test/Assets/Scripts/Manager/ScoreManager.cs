using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private Text scoreText;
    [SerializeField] private Text LifeText;

    private float mainTimer;

    private bool canCount = false;


    private static ScoreManager instance;

    public static ScoreManager Instance
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

    // Use this for initialization
    void Start()
    {
        mainTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCount)
        {
            mainTimer += Time.deltaTime;
            scoreText.text = mainTimer.ToString("F");
        }
    }

    public void UpdateLifePlayer(int life)
    {
        LifeText.text = life.ToString();
    }

    public void ResetTimer(float value)
    {
        mainTimer = value;
        scoreText.text = mainTimer.ToString("F");
    }

    public float MainTimer
    {
        get
        {
            return mainTimer;
        }

        set
        {
            mainTimer = value;
        }
    }

    public bool CanCount
    {
        get
        {
            return canCount;
        }

        set
        {
            canCount = value;
        }
    }

}
