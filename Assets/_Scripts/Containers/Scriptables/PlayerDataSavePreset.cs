using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/GameSettings/PlayerDataSavePreset")]
    public class PlayerDataSavePreset : ScriptableObject, IPlayerDataSave
    {
        [field: SerializeField] public TruckID PlayerTruckID { get; private set; }
        [field: SerializeField] public VirtualPoint RecoveryPoint { get; private set; }
    }
}
