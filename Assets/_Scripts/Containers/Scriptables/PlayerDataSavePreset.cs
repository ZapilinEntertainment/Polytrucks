using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/GameSettings/PlayerDataSavePreset")]
    public class PlayerDataSavePreset : ScriptableObject
    {
        [field: SerializeField] public bool TutorialCompleted { get; private set; } = false;
        [field: SerializeField] public TruckID[] UnlockedTrucks = new[] { GameConstants.DefaultTruck };
        [field: SerializeField] public float IntegrityPercent { get; private set; }
        [field: SerializeField] public TruckID PlayerTruckID { get; private set; }
        [field: SerializeField] public VirtualPoint RecoveryPoint { get; private set; }
        [field: SerializeField] public List<VirtualCollectable> StorageContent { get; private set; }
        
    }
}
