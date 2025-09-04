using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// by nt.Dev93
namespace ntDev
{
    public class ProgressShow : MonoBehaviour
    {
        public double CurrentValue;
        [SerializeField] double targetValue;
        public double TargetValue
        {
            get => targetValue;
            set
            {
                if (targetValue == value) return;
                targetValue = value;
                d = (targetValue - CurrentValue) / step;
            }
        }

        [SerializeField] double step = 60;
        [SerializeField] double d = 0.01;
        Action<double> funcProgress;
        Action funcDone;

        public void Init(float c, Action<double> fP = null, Action fD = null)
        {
            CurrentValue = c;
            TargetValue = c;
            funcProgress = fP;
            funcDone = fD;
            fP?.Invoke(c);
        }

        void Update()
        {
            if (CurrentValue != TargetValue)
            {
                if (Math.Abs(TargetValue - CurrentValue) <= Math.Abs(d))
                {
                    CurrentValue = TargetValue;
                    funcProgress?.Invoke(CurrentValue);
                    funcDone?.Invoke();
                }
                else
                {
                    CurrentValue += d;
                    funcProgress?.Invoke(CurrentValue);
                }
            }
        }
    }
}