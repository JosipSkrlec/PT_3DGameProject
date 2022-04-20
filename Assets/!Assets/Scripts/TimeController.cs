using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private int _maxTime = 200;
    [SerializeField] private TMP_Text _timeText;

    private float _timeCounter = 0.0f;

    private float _currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        _timeCounter += Time.deltaTime;

        _currentTime = _maxTime - _timeCounter;

        if (_currentTime <= 0.0f)
        {
            Debug.Log("Time is Left - Do something!");
            RestartTimeCounting();
            return;
        }

        _timeText.text = ((int)_maxTime - (int)_timeCounter).ToString();
    }

    public void RestartTimeCounting()
    {
        _timeCounter = 0.0f;
        _currentTime = 0.0f;

    }



}
