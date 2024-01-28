using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripClock : MonoBehaviour
{

    [Tooltip("Time in seconds to determine successful end of the trip.")]
    [SerializeField] private float tripTime;
    [Tooltip("Set to true to tick down the remaining time, false otherwise.")]
    [SerializeField] private bool tickingTime;
    [Tooltip("Success menu to trigger when the timer elapsed.")]
    [SerializeField] private GameObject successMenu;

    private float timeLeft;

    void Start()
    {
        timeLeft = tripTime;
    }

    void Update()
    {
        if (tickingTime)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                tickingTime = false;
                TimeElapsed();
            }
        }
    }

    public void TimeElapsed()
    {
        successMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public float GetTimeLeft()
    {
        return timeLeft;
    }
}
