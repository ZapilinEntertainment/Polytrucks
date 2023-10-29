using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public abstract class StorageProvider : MonoBehaviour, IItemProvider
    {       
        abstract public void ReturnItem(VirtualCollectable item);
        abstract public void SubscribeToProvisionListChange(Action action);
        abstract public void UnsubscribeFromProvisionListChange(Action action);

        abstract public bool TryProvideItem(VirtualCollectable item);
        abstract public bool TryProvideItems(TradeContract contract, out List<VirtualCollectable> list);        
    }
}
