using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    private int playTime = 0;
    private bool isRunning = false;
    public static Action OnValueChange = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    float timecount = 0;
    void Update()
    {
        if (isRunning)
        {
            timecount += Time.deltaTime;
            if (timecount >= 1)
            {
                timecount = 0;
                Tick();
            }
        }
    }
    void Tick()
    {
        playTime++;
        OnValueChange?.Invoke();
    }
    public void StartTimer()
    {
        Debug.Log("TimerStarted");
        isRunning = true;
    }

    public void StopTimer()
    {
        Debug.Log("TimeStopped");
        isRunning = false;
    }

    public float GetPlayTime()
    {
        return playTime;
    }

    public void ResetTimer()
    {
        playTime = 0;
    }

    public string getTimeNow()
    {
        return System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }

}