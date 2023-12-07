using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    [Serializable]
    public class QuestPreset
    {
        public QuestType QuestType;
        public VirtualCollectable DeliveringItem;
        public int RequiredCount;
        public DeliveryPoint DeliveryPoint;

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
