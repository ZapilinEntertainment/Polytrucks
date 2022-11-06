using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    [RequireComponent(typeof(Collider))]
    public class SellZone : MonoBehaviour
    {
        protected sealed class HandlingTruck
        {
            public readonly Collider Collider;
            public readonly TruckController Truck;

            public HandlingTruck(Collider col)
            {
                Collider = col;
                Truck = col.GetComponent<TruckController>();
            }
        }

        [SerializeField] protected ItemType[] _sellTypes;
        protected bool _isTrading = false;
        protected int _tradeMask;
        protected HandlingTruck _handlingTruck;
        public float TradeProgress { get; protected set; }

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
                    _isTrading = true;
                    TradeProgress = 0f;
                    _handlingTruck = new HandlingTruck(col);
                    IngamePopupCanvas.OnStartTrading(this);
                    print("start trading");
                }
            }
        }
        private void OnTriggerExit(Collider col)
        {
            if (_isTrading && col == _handlingTruck.Collider)
            {
                StopTrading();
                print("trading rejected");
            }
        }

        private void Update()
        {
            if (_isTrading)
            {
                TradeProgress = Mathf.MoveTowards(TradeProgress, 1f, Time.deltaTime);
                if (TradeProgress == 1f)
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
                            IngamePopupCanvas.OnMoneyCollected(transform.position, '+' + income.ToString());
                        }
                    }
                    StopTrading();
                    print("trading completd");
                }
            }
        }
        private void StopTrading()
        {
            _isTrading = false;
            _handlingTruck = null;
            TradeProgress = 0f;
            IngamePopupCanvas.OnStopTrading(this);
        }

        private void OnDestroy()
        {
            if (_isTrading)
            {
                _isTrading = false;
                IngamePopupCanvas.OnStopTrading(this);
            }
        }
    }
}
