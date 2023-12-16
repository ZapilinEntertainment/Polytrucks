using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    [RequireComponent(typeof(Collider))]
	public sealed class PaymentTrigger : MonoBehaviour
	{
        [SerializeField] private int _moneyCost = 100;
        [SerializeField] private MonoBehaviour _activableScript;
        [SerializeField] private TMPro.TMP_Text _costLabel;
        private bool _isShown = false;
        private int _playerColliderId = -1, _showingLabelID = -1;
        private ColliderListSystem _colliderList;
        private UIManager _uiManager;
        private Localization _localization;
        private PlayerData _playerData;

        [Inject]
        public void Inject(ColliderListSystem colliderListSystem, UIManager uiManager, Localization localization, PlayerData playerData)
        {
            _colliderList = colliderListSystem;
            _uiManager= uiManager;
            _localization = localization;
            _playerData = playerData;
        }

        private void Start()
        {
            if (_costLabel != null) _costLabel.text = _moneyCost.ToString();
        }

        public bool TryMakePayment()
        {
            if (_playerData.TrySpendMoney(_moneyCost))
            {
                OnPaymentComplete();
                return true;
            }
            else return false;
        }
        public void OnPaymentComplete()
        {
            HideLabel();
            (_activableScript as IActivableMechanism).Activate();
            Destroy(_costLabel.gameObject);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isShown) return;
            int id = other.GetInstanceID();
            if (_colliderList.TryDefineAsPlayer(id, out var player))
            {
                _isShown = true;
                _playerColliderId = id;


                _showingLabelID = _uiManager.ShowPayPanel(
                    new ActionContainer()
                    {
                        WorldPos = transform.position,
                        MainLabel = _localization.GetLocalizedString(LocalizedString.Unlock),
                        CostLabel = _moneyCost.ToString(),
                        RejectionLabel = _localization.GetLocalizedString(LocalizedString.NotEnoughMoney),
                        ResultFunc = TryMakePayment
                    }
                   );
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(_isShown && other.GetInstanceID() == _playerColliderId)
            {
                _isShown = false;
                _playerColliderId = -1;

                HideLabel();
            }
        }
        private void HideLabel()
        {
            _uiManager.HidePayPanel(_showingLabelID);
            _showingLabelID = -1;
        }
    }
}
