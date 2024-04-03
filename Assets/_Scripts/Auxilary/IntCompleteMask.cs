using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks
{
    public class IntCompleteMask
    {
        private int Value;
        public bool IsComplete => Value == 0;

        public IntCompleteMask()
        {
            Value = 0;
        }
        public IntCompleteMask(int flagsCount)
        {
            Value = 0;
            if (flagsCount != 0)
            {
                for (int i = 0; i < flagsCount; i++)
                {
                    Value |= (1 << i);
                }
            }
            else Value = 0;
        }
        public bool CompleteFlag(int flagIndex)
        {
            Value &= ~(1 << flagIndex);
            return IsComplete;
        }
        public bool IsFlagCompleted(int flagIndex)
        {
            return (Value & (1 << flagIndex)) == 0;
        }
    }

    public class MultiFlagsCondition
    {
        public bool IsComplete { get; set; } = false;
        private readonly bool _oneShot;
        private readonly IntCompleteMask _mask;
        private readonly System.Action OnCompleteEvent;

        public MultiFlagsCondition(int flagsCount, System.Action completeEvent, bool oneShot = true)
        {
            _mask = new IntCompleteMask(flagsCount);
            _oneShot = oneShot;
            OnCompleteEvent = completeEvent;
        }
        public void SetFlag(int x)
        {
            if (_mask.CompleteFlag(x))
            {
                if (!IsComplete || !_oneShot)
                {
                    IsComplete = true;
                    OnCompleteEvent?.Invoke();
                }
            }
        }
    }
}
