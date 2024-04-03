using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using UnityEngine.UI;

namespace ZE.Polytrucks {
	public class ActionContainer
	{
        protected readonly LocalizedString _mainLabel, _rejectionLabel;
        protected const float MAX_FAR = 50f;
        public readonly float Radius;
		public readonly string CostLabel;        
		public readonly Vector3 WorldPos;
		public readonly Func<bool> ResultFunc;
		public float SqrRadius => Radius * Radius;
        

        public ActionContainer(LocalizedString mainLabel, LocalizedString rejectionLabel, string costLabel, Func<bool> resultFunc, 
			Vector3 worldPos, float radius = MAX_FAR)
		{
			_mainLabel= mainLabel;
			_rejectionLabel= rejectionLabel;
			CostLabel= costLabel;
			ResultFunc= resultFunc;
			WorldPos = worldPos;
			Radius = radius;
		}

		public bool ActionCheck()
		{
			if (ResultFunc != null) return ResultFunc();
			else return false;
		}

		virtual public string GetMainLabel(Localization locale) => locale.GetLocalizedString(_mainLabel);
		public string GetRejectionLabel(Localization locale) => locale.GetLocalizedString(_rejectionLabel);
	} 
	public class TruckBuyActionContainer : ActionContainer
	{
		public readonly TruckID TruckID;
        public TruckBuyActionContainer(TruckID id,LocalizedString mainLabel, LocalizedString rejectionLabel, string costLabel, Func<bool> resultFunc,
            Vector3 worldPos, float radius = MAX_FAR) : base(mainLabel,rejectionLabel, costLabel, resultFunc, worldPos, radius)
		{
			TruckID= id;
		}

        public override string GetMainLabel(Localization locale)
        {
            return base.GetMainLabel(locale) +" (" + locale.GetTruckName(TruckID) + ")";
        }

    }
	public sealed class ActionPanel : MonoBehaviour, IDynamicLocalizer
	{
		[SerializeField] private TMPro.TMP_Text _mainLabel, _costLabel, _rejectionLabel;
		[SerializeField] private Image _icon;
		[SerializeField] private GameObject _labelObject;
		[SerializeField] private float _rejectionLabelTime = 2f;
		private bool _isShowing = false;
		private int _actionID = 0;
		private ActionContainer _currentActionContainer;
		private CameraController _cameraController;
		private Localization _localization;
		private PlayerController _player;

		[Inject]
		public void Inject(CameraController cameraController, Localization localization, PlayerController player)
		{
			_cameraController= cameraController;
			_localization= localization;			
			_player= player;
		}

        private void Start()
        {
           // _labelObject.SetActive(false);
			//_rejectionLabel.enabled = false;
            _localization.Subscribe(this);
        }
        public int Show(ActionContainer actionContainer)
		{
			_actionID++;
			_currentActionContainer = actionContainer;
			_costLabel.text = actionContainer.CostLabel;
			LocalizeStrings();
			PositionLabel();
			
			SetActivity(true);
            Update();
            return _actionID;
		}
		private void LocalizeStrings()
		{
			_mainLabel.text = _currentActionContainer.GetMainLabel(_localization);
			_rejectionLabel.text = _currentActionContainer.GetRejectionLabel(_localization);
		}
		private void PositionLabel() => transform.position = _cameraController.WorldToScreenPoint(_currentActionContainer.WorldPos);
        private void Update()
        {
            if (_isShowing)
			{
				float sqrDistance = Vector3.SqrMagnitude(_player.Position - _currentActionContainer.WorldPos);

                if (sqrDistance > _currentActionContainer.SqrRadius)
				{
					i_Hide();
				}
				else
				{
					PositionLabel();
					if (_rejectionLabel.enabled)
					{
						var color = _rejectionLabel.color;
						color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime / _rejectionLabelTime);
						_rejectionLabel.color = color;
						if (color.a == 0f) _rejectionLabel.enabled = false;
					}
				}
            }
        }
        public void Hide(int id)
		{
			if (_actionID == id) i_Hide();
		}
		private void i_Hide()
		{
            _actionID = -1;
            SetActivity(false);
        }
		public void SetActivity(bool x)
		{
			_isShowing = x;
			_rejectionLabel.enabled = false;
			_labelObject.SetActive(x);
		}

		public void BUTTON_Click()
		{
			if (_currentActionContainer.ActionCheck())
			{
				i_Hide();
				_rejectionLabel.enabled = false;
			}
			else
			{
				var color = _rejectionLabel.color;
				color.a = 1f;
				_rejectionLabel.color = color;
				_rejectionLabel.enabled = true;
			}
		}

        public void OnLocaleChanged()
		{
			if (_isShowing) LocalizeStrings();
		}
    }
}
