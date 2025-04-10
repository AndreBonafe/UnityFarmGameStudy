using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assembly_CSharp.Assets.Scripts.WorldTime
{
    public class WorldTimeWatcher : MonoBehaviour
    {
        [SerializeField]
        private WorldTime _worldTime;

        [SerializeField]
        private List<Schedule> _schedule;

        private void Start()
        {
            _worldTime.WorldTimeChanged += CheckSchedule;
        }

        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= CheckSchedule;    
        }

        private void CheckSchedule(object sender, TimeSpan newTime)
        {
            var schedule =
                _schedule.FirstOrDefault(s =>
                    s.Hour == newTime.Hours &&
                    s.Minute == newTime.Minutes);
            schedule?._action.Invoke();
        }

        [Serializable]
        private class Schedule
        {
            public int Hour;
            public int Minute;
            public UnityEvent _action;
        }
    }
}
