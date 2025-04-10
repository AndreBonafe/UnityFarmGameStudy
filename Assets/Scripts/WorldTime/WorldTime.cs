using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assembly_CSharp.Assets.Scripts.WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        [SerializeField]
        private int _currentDay = 1;
        public event EventHandler<TimeSpan> WorldTimeChanged;
        public event EventHandler<int> DayChanged;
        [SerializeField]
        private float _dayLength; //in seconds
        private TimeSpan _currentTime = TimeSpan.FromHours(6);
        private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;
        [SerializeField]
        private Player _player;

        public int CurrentDay { get => _currentDay; }
        private bool stop;

        private void Start() {
            StartCoroutine(AddMinute());
        }
        private IEnumerator AddMinute()
        {
            if (!stop) {
                var nextTime = _currentTime + TimeSpan.FromMinutes(1);
                if (nextTime.Hours == 02) {
                    _currentTime = TimeSpan.FromMinutes(0) + TimeSpan.FromHours(6);
                    _currentDay += 1;
                    DayChanged?.Invoke(this, _currentDay);
                    _player.PassOut();
                } else {
                    _currentTime += TimeSpan.FromMinutes(1);
                }
                WorldTimeChanged?.Invoke(this, _currentTime);
                yield return new WaitForSeconds(_minuteLength);
                StartCoroutine(AddMinute());
                // Debug.Log(_currentTime.ToString());
            }
        }

        public void Stop()
        {
            stop = true;
        }

        public void Resume()
        {
            stop = false;
            StartCoroutine(AddMinute());
        }

        public void SetTime(int hours, int minutes)
        {
            _currentTime = TimeSpan.FromMinutes(minutes) + TimeSpan.FromHours(hours);
        }
    }
}
