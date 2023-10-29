using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class ConveyorBelt : MonoBehaviour, IItemProvider
	{
        private class TransferringItem
        {
            public readonly VirtualCollectable Info;
            public readonly CollectibleModel Model;

            public TransferringItem(VirtualCollectable info, CollectibleModel model)
            {
                Info= info;
                Model= model;
            }
        }

        [SerializeField] private float _transferTime = 1f;
		[SerializeField] private Vector3 _startPos, _endPos;
        private int _nextToken = 1;
        private ObjectsManager _objectsManager;
        private Dictionary<int, TransferringItem> _transferringModels = new Dictionary<int, TransferringItem>();
        public VirtualPoint StartPos => new VirtualPoint(transform.TransformPoint(_startPos), transform.rotation);
        public VirtualPoint EndPos => new VirtualPoint(transform.TransformPoint(_endPos), transform.rotation);
        public Action<VirtualCollectable> OnItemProvidedEvent { get; set; }

        [Inject]
        public void Inject(ObjectsManager objectsManager)
        {
            _objectsManager = objectsManager;
        }
        public void TransferItem(VirtualCollectable item)
        {
            var itemModel = _objectsManager.GetCollectibleModel(item);
            var itemTransform = itemModel.transform;
            int id = _nextToken++;
            _transferringModels.Add(id, new TransferringItem(item, itemModel));

            itemTransform.parent = transform;
            itemTransform.localPosition = _startPos;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.DOLocalMove(_endPos, _transferTime, true).OnComplete(() => OnItemTransferred(id));
        }
        private void OnItemTransferred(int id)
        {
            if (_transferringModels.TryGetValue(id, out var item))
            {
                _transferringModels.Remove(id);
                item.Model.Dispose();
                OnItemProvidedEvent?.Invoke(item.Info);
            }           
        }
        

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(StartPos.Position, 0.2f);
            Gizmos.DrawSphere(EndPos.Position, 0.2f);
        }
    }
}
