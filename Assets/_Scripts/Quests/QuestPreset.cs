using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    [Serializable]
    public class QuestPreset
    {
        [field:SerializeField] public QuestType QuestType{ get; private set; }
        [field: SerializeField] public VirtualCollectable DeliveringItem{ get; private set; }
        [field: SerializeField] public int RequiredCount{ get; private set; }
        [field: SerializeField] public DeliveryPoint DeliveryPoint { get; private set; }

        public bool TryStartQuest(PlayerController player, out QuestBase quest)
        {
            switch (QuestType)
            {
                case QuestType.Delivery:
                case QuestType.TimedDelivery:
                    {
                        if (TryLoadCargo())
                        {
                            quest = new DeliveryQuest(DeliveryPoint, DeliveringItem, RequiredCount);
                            return true;
                        }
                        else
                        {
                            quest = null;
                            return false;
                        }
                    }
                case QuestType.Supply:
                    {
                        quest = new SupplyQuest(DeliveryPoint, DeliveringItem, RequiredCount);
                        return true;
                    }
                default:
                    {
                        quest = null;
                        return false;
                    }
            }

            bool TryLoadCargo() => player.TryLoadCargo(DeliveringItem, RequiredCount); 
        }
    }
}
