using System.Collections;
using System.Timers;
using TMPro;
using UnityEngine;
using Timer = System.Timers.Timer;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _gameTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        float currentTime = _gameTime;
        
        while (currentTime > 0)
        {
            int minutes = (int)currentTime / 60;
            int seconds = (int)currentTime % 60;
            
            string formatedTime = $"{minutes:00}:{seconds:00}";
            _timerText.text = formatedTime;
            
            currentTime -= Time.deltaTime;
            
            
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
