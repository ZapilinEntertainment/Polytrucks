using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    public enum QuestType : byte
    {
        Delivery, Supply, TimedDelivery, Tutorial,Storyline, TypesCount
    }

    [Serializable]
    public class QuestTypeDefinedValues<T> 
    {
        [field: SerializeField] public T Delivery { get; private set; }
        [field: SerializeField] public T Supply { get; private set; }
        [field: SerializeField] public T TimedDelivery { get; private set; }
        [field: SerializeField] public T Tutorial { get; private set; }
        [field: SerializeField] public T Storyline { get; private set; }
        public T this[QuestType rarity]
        {
            get
            {
                switch (rarity)
                {
                    case QuestType.Supply: return Supply;
                    case QuestType.TimedDelivery: return TimedDelivery;
                    case QuestType.Tutorial: return Tutorial;
                    case QuestType.Storyline: return Storyline;
                    default: return Delivery;
                }
            }
        }

        public Dictionary<QuestType, T> ToDictionary() => new ()
            {
            { QuestType.Delivery, Delivery }, {QuestType.Supply, Supply}, {QuestType.TimedDelivery, TimedDelivery}
            };

        public IEnumerator<T> GetEnumerator() => new QuestTypeEnumerator(this);

        public class QuestTypeEnumerator : IEnumerator<T>
        {
            private QuestType _currentIndex = 0; // на шаг впереди, возвращает на 1 значение меньше
            private readonly QuestTypeDefinedValues<T> _source;
            public T Current
            {
                get
                {
                    if (_currentIndex == 0) throw new ArgumentException();
                    else return _source[_currentIndex - 1];
                }
            }
            object IEnumerator.Current => Current;

            public QuestTypeEnumerator(QuestTypeDefinedValues<T> source) => _source = source;

            public bool MoveNext()
            {
                if (_currentIndex < QuestType.TypesCount)
                {
                    _currentIndex++;
                    return true;
                }
                else return false;
            }

            public void Reset()
            {
                _currentIndex = 0;
            }
            public void Dispose() { }
        }
    }
}
