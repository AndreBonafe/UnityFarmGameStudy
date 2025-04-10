using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assembly_CSharp.Assets.Scripts.WorldTime
{
    [RequireComponent(typeof(TMP_Text))]
    public class WorldDaysDisplay : MonoBehaviour
    {
        [SerializeField]
        private WorldTime _worldTime;
        private TMP_Text _text;

        private void Awake() {
            _text = GetComponent<TMP_Text>();
            _worldTime.DayChanged += OnWorldDayChanged;
        }

        private void OnDestroy() {
            _worldTime.DayChanged -= OnWorldDayChanged;
        }

        private void OnWorldDayChanged(object sender, int  newDay)
        {
            _text.SetText(newDay.ToString());
        }
    }
}