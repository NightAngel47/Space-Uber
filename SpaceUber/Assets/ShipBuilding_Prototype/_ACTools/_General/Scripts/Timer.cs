using System;
using UnityEngine;
using UnityEngine.Events;

namespace ACTools
{
    namespace General
    {
        [AddComponentMenu("ACTools/General/Timer")]
        [Serializable]
        public class Timer : MonoBehaviour
        {
            [SerializeField] private bool countOnStart = true;  // Should this timer begin counting when it Start() is called.

            [SerializeField] private float startingValue = 1;   // Value the timer starts at.
            public float StartingValue { get => startingValue; set => startingValue = value; }  // Property for startingValue.

            [Space]

            [ReadOnly]
            [SerializeField] private float value = 0;   // Value for the timer.
            public float Value { get => value; }        // Property for value.

            private float previousValueCeiling = 0f;// Variable for the PreviousValueFloor property.
            public float PreviousValueCeiling       // Holds the ceiling of the previous timer value.
            {
                get => previousValueCeiling;
                private set => previousValueCeiling = Mathf.Ceil(value);
            }

            public bool CurrentlyCounting { get; private set; } = false;    // Is this timer currently counting?

            [Header("Events")]

            public UnityEvent OnStart;
            public FloatEvent OnTick;
            public UnityEvent OnFinish;

            private void Start()
            {
                if (countOnStart)
                    StartTimer();
            }

            private void Update()
            {
                if (CurrentlyCounting)
                {
                    if (value >= 0)
                    {
                        value -= Time.deltaTime;
                        if (PreviousValueCeiling - 1 >= value)
                        {
                            PreviousValueCeiling = value;
                            OnTick.Invoke(PreviousValueCeiling);
                        }
                    }
                    else
                    {
                        CurrentlyCounting = false;
                        OnFinish.Invoke();
                    }
                }
            }

            /// <summary> Starts this timer. </summary>
            public void StartTimer()
            {
                ResetTimer();
                CurrentlyCounting = true;
                OnStart.Invoke();
            }

            /// <summary> Pauses this timer. </summary>
            public void PauseTimer()
            {
                CurrentlyCounting = false;
            }

            /// <summary> Continue counting for this timer. </summary>
            public void ContinueCounting()
            {
                CurrentlyCounting = true;
            }

            /// <summary> Reset this timer. </summary>
            public void ResetTimer()
            {
                value = startingValue;
                PreviousValueCeiling = value;
            }
        }

        [Serializable]
        public class FloatEvent : UnityEvent<float> { };
    }
}