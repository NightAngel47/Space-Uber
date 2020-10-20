using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ACTools
{
    namespace General
    {
        [AddComponentMenu("ACTools/General/Stopwatch")]
        [Serializable]
        public class Stopwatch : MonoBehaviour
        {
            [SerializeField] private bool countOnStart = true;  // Should this stopwatch begin counting when it Start() is called.

            [Space]

            [ReadOnly]
            [SerializeField] private float value = 0;  // Value for the stopwatch.
            public float Value { get => value; }  // Property for value.

            private float previousValueFloor = 0f;  // Variable for the PreviousValueFloor property.
            public float PreviousValueFloor    // Holds the floor of the previous value.
            {
                get => previousValueFloor;
                private set => previousValueFloor = Mathf.Floor(value);
            }

            public bool CurrentlyCounting { get; private set; } = false;    // Is this stopwatch currently counting?

            [ReadOnly]
            [SerializeField]
            private List<float> laps = new List<float>();    // List of lap values.
            public float[] LapArray => laps.ToArray();   // Array to acces lap values.
            private float currentLapsTotal = 0;

            [Header("Events")]

            public UnityEvent OnStart;
            public FloatEvent OnTick;

            void Start()
            {
                if (countOnStart)
                    StartCounting();
            }

            void Update()
            {
                if (CurrentlyCounting)
                {
                    value += Time.deltaTime;
                    if (value - PreviousValueFloor >= 1)
                    {
                        PreviousValueFloor = value;
                        OnTick.Invoke(PreviousValueFloor);
                    }
                }
            }

            /// <summary> Starts the stopwatch. </summary>
            public void StartCounting()
            {
                CurrentlyCounting = true;
            }

            /// <summary> Stop the stopwatch. </summary>
            public void StopCounting()
            {
                CurrentlyCounting = false;
            }

            /// <summary> Starts the stopwatch without resetting the value. </summary>
            public void ContinueCounting()
            {
                CurrentlyCounting = true;
            }

            /// <summary> Resets the stopwatch value. </summary>
            public void ResetStopWatch()
            {
                value = 0f;
                PreviousValueFloor = 0f;
                ResetLapList();
            }

            /// <summary> Laps the stopwatch value. </summary>
            public void Lap()
            {
                if (laps.Count == 0)
                {
                    laps.Add(value);
                    currentLapsTotal += value;
                }
                else
                {
                    float finishedLap = value - currentLapsTotal;
                    laps.Add(finishedLap);
                    currentLapsTotal += finishedLap;
                }
            }

            /// <summary> Resets the lap list. </summary>
            public void ResetLapList()
            {
                laps = new List<float>();
                currentLapsTotal = 0;
            }
        }
    }
}