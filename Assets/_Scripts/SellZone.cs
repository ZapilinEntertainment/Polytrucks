using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    [RequireComponent(typeof(Collider))]
    public class SellZone : MonoBehaviour, IProgressionObject
    {
        protected sealed class HandlingTruck
        {
            public readonly Collider Collider;
            public readonly TruckController Truck;

            public bool IsEmpty => Truck?.GetStorage()?.IsEmpty ?? true;
            public bool IsStopped => (Truck?.SpeedPc ?? 0f) < 0.1f;

            public HandlingTruck(Collider col)
            {
                Collider = col;
                Truck = col.GetComponent<TruckController>();
            }
        }

        [SerializeField] protected ItemType[] _sellTypes;
        [SerializeField] protected float _tradeTime = 1f;
        protected bool _isTrading = false;
        protected float _tradeProgress;
        protected int _tradeMask;
        protected HandlingTruck _handlingTruck;
        protected Coroutine _waitForStopCoroutine;

        public bool IsProgressing => _isTrading;
        public float Progress => _tradeProgress;
        public Vector3 IndicatorPosition
        {
            get { if (!_isTrading || _handlingTruck == null) return transform.position; else return _handlingTruck.Truck.transform.position; }
        }

        private void Start()
        {
            _tradeMask = _sellTypes.FormMask();
        }
        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(GameConstants.TRUCK_TAG) && col.GetComponentInParent<Player>())
            {
                if (!_isTrading)
                {
                    _handlingTruck = new HandlingTruck(col);                    

                    if (!_handlingTruck.IsEmpty)
                    {
                        if (_handlingTruck.IsStopped) StartTrading();
                        else
                        {
                            if (_waitForStopCoroutine != null) StopCoroutine(_waitForStopCoroutine);
                            _waitForStopCoroutine = StartCoroutine(WaitForStartCoroutine());
                        }
                    }
                }
            }
        }
        private IEnumerator WaitForStartCoroutine()
        {
            yield return new WaitUntil(() => _handlingTruck?.IsStopped ?? true );            
            if (_handlingTruck != null) StartTrading();
        } 
        private void StartTrading()
        {
            _waitForStopCoroutine = null;
            _isTrading = true;
            _tradeProgress = 0f;
            UIManager.OnStartTrading(this);
        }
        private void OnTriggerExit(Collider col)
        {
            if (_isTrading && col == _handlingTruck.Collider)
            {
                StopTrading();
            }
        }

        private void Update()
        {
            if (_isTrading)
            {
                if (_handlingTruck != null)
                {
                    _tradeProgress = Mathf.MoveTowards(_tradeProgress, 1f, Time.deltaTime / _tradeTime);
                    if (_tradeProgress == 1f)
                    {
                        var sellList = _handlingTruck.Truck.GetStorage().SellItems(_tradeMask);
                        if (sellList.Length > 0)
                        {
                            int income = 0;
                            foreach (var item in sellList)
                            {
                                income += item.GetCost();
                            }
                            if (income != 0)
                            {
                                MoneyManager.Instance.AddMoney(income);
                                UIManager.OnMoneyCollected(IndicatorPosition, '+' + income.ToString());
                            }
                        }
                        StopTrading();
                    }
                }
                else StopTrading();
            }
        }
        private void StopTrading()
        {
            _isTrading = false;
            if (_waitForStopCoroutine != null)
            {
                StopCoroutine(_waitForStopCoroutine);
                _waitForStopCoroutine = null;
            }
            _handlingTruck = null;
            _tradeProgress = 0f;
            UIManager.OnStopTrading(this);
        }

        private void OnDestroy()
        {
            if (_isTrading)
            {
                _isTrading = false;
                UIManager.OnStopTrading(this);
            }
        }
    }
}
