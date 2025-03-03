using System;
using System.Threading.Tasks;
using UnityEngine;

public static class Timer
{
    public static async void StartTimer(int milliseconds, Action callback)
    {
        await Task.Delay(milliseconds);
        callback.Invoke();
    }

    public static async void StartTimer(int milliseconds, int interval, Action<int> timeCallback, Action callback)
    {
        while (milliseconds > 0)
        {
            
            milliseconds -= interval;
        }
    }
}
