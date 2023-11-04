using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {
	public class ConveyorBelt : MonoBehaviour, IItemReceiver
	{
        private class TransferringItem
        {
            public float TransferProgression = 0f;
            public readonly VirtualCollectable Info;
            public readonly CollectibleModel Model;

            public TransferringItem( VirtualCollectable info, CollectibleModel model)
            {
                Info= info;
                Model= model;
            }
        }

        [SerializeField] private int _maxTransferringItems = 4;
        [SerializeField] private float _transferTime = 1f, _receiveCooldown = 0.1f;
		[SerializeField] private Vector3 _startPos, _endPos;
        private int _transferringItemsCount= 0;        
        private IItemReceiver _itemReceiver;
        private ObjectsManager _objectsManager;
        private List<TransferringItem> _transferringItems = new List<TransferringItem>();
        private Action OnItemAddedToBeltEvent, OnItemRemovedFromBeltEvent;


        public VirtualPoint StartPos => new VirtualPoint(transform.TransformPoint(_startPos), transform.rotation);
        public VirtualPoint EndPos => new VirtualPoint(transform.TransformPoint(_endPos), transform.rotation);
        public Action<VirtualCollectable> OnItemProvidedEvent { get; set; }
        

        [Inject]
        public void Inject(ObjectsManager objectsManager)
        {
            _objectsManager = objectsManager;
        }

        public void AssignReceiver(IItemReceiver receiver) => _itemReceiver = receiver;
        public void StartTransferItem(VirtualCollectable item)
        {
            var itemModel = _objectsManager.GetCollectibleModel(item);
            var itemTransform = itemModel.transform;

            _transferringItems.Insert(0,new TransferringItem( item, itemModel));
            _transferringItemsCount = _transferringItems.Count;

            itemTransform.parent = transform;
            itemTransform.localPosition = _startPos;
            itemTransform.localRotation = Quaternion.identity;

            OnItemAddedToBeltEvent?.Invoke();
        }
        private void Update()
        {
            if (_transferringItemsCount != 0)
            {
                float maxProgression = 1f;
                float step = Time.deltaTime / _transferTime;
                for (int i = _transferringItemsCount - 1; i > -1; i--)
                {
                    var item = _transferringItems[i];
                    float progression = item.TransferProgression;
                    progression = Mathf.MoveTowards(progression, maxProgression, step);
                    if (progression == 1f && (_itemReceiver?.TryAddItem(item.Info) ?? false))
                    {
                        _transferringItems.RemoveAt(i);
                        _transferringItemsCount--;
                        item.Model.Dispose();
                        OnItemProvidedEvent?.Invoke(item.Info);
                    }
                    else
                    {
                        maxProgression -= 1f / (float)_maxTransferringItems;
                        item.TransferProgression = progression;
                        item.Model.transform.localPosition = Vector3.Lerp(_startPos, _endPos, progression);
                    }                    
                }
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(StartPos.Position, 0.2f);
            Gizmos.DrawSphere(EndPos.Position, 0.2f);
        }

        #region receiver
        public bool IsReadyToReceive => _transferringItemsCount < _maxTransferringItems;
        public int FreeSlotsCount => _maxTransferringItems - _transferringItemsCount;
        public bool TryAddItem(VirtualCollectable item)
        {
            if (IsReadyToReceive)
            {
                StartTransferItem(item);
                return true;
            }
            else return false;
        }
        public int AddItems(VirtualCollectable item, int count)
        {
            if (TryAddItem(item)) return count - 1;
            else return count;
        }
        public void AddItems(IList<VirtualCollectable> items, out BitArray result)
        {
            int count = items.Count, i = 0 ;
            result = new BitArray(count, false);
            if (TryAddItem(items[0])) result[0] = true;
        }

        public void SubscribeToItemAddEvent(Action action) => OnItemAddedToBeltEvent += action;
        public void UnsubscribeFromItemAddEvent(Action action) => OnItemAddedToBeltEvent-= action;
        public void SubscribeToItemRemoveEvent(Action action) => OnItemRemovedFromBeltEvent += action;
        public void UnsubscribeFromItemRemoveEvent(Action action) => OnItemRemovedFromBeltEvent-= action;

        
        #endregion
    }
}
