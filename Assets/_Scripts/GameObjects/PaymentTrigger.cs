using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    [RequireComponent(typeof(Collider))]
	public sealed class PaymentTrigger : PlayerTrigger
	{
        [SerializeField] private int _moneyCost = 100;
        [SerializeField] private MonoBehaviour _activableScript;
        [SerializeField] private TMPro.TMP_Text _costLabel;
        [SerializeField] private string PayStringID = "Unlock";
        private bool _isShown = false;
        private int _showingLabelID = -1;
        private UIManager _uiManager;
        private Localization _localization;
        private IAccountDataAgent _accountAgent;

        [Inject]
        public void Inject(UIManager uiManager, Localization localization, IAccountDataAgent accountAgent)
        {
            _uiManager= uiManager;
            _localization = localization;
            _accountAgent= accountAgent;
        }

        private void Start()
        {
            if (_costLabel != null) _costLabel.text = _moneyCost.ToString();
            OnPlayerEnterEvent += OnPlayerEntered;
            OnPlayerExitEvent += OnPlayerLeaved;
        }

        public bool TryMakePayment()
        {
            if (_accountAgent.PlayerDataAgent.TrySpendMoney(_moneyCost))
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
            if (_costLabel != null) Destroy(_costLabel.gameObject);
            Destroy(gameObject);
        }

        private void OnPlayerEntered(PlayerController player)
        {
            if (_isShown) return;
            _isShown = true;            
            _showingLabelID = _uiManager.ShowActionPanel(
                    new ActionContainer(                    
                        worldPos: transform.position,
                        mainLabel:  _localization.GetStringID(PayStringID),
                        costLabel:  _moneyCost.ToString(),
                        rejectionLabel:  LocalizedString.NotEnoughMoney,
                        resultFunc:  TryMakePayment,
                        radius: Radius
                    )
                   );
        }
        private void OnPlayerLeaved()
        {
            if (_isShown) HideLabel();
        }

        private void HideLabel()
        {
            _uiManager.HideActionPanel(_showingLabelID);
            _showingLabelID = -1;
            _isShown = false;
        }
    }
}
