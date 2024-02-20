using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks
{
    public class FieldLocalizer : MonoBehaviour, IDynamicLocalizer
    {
        [SerializeField] protected TMPro.TMP_Text _field;
        [SerializeField] protected string _localeKeyName;
        private Localization _localization;

        [Inject]
        public void Inject(Localization localization)
        {
            _localization = localization;
            localization.Subscribe(this);
            OnLocaleChanged();
        }

        public void OnLocaleChanged()
        {
            if (_localeKeyName != null && _localeKeyName != string.Empty)
            {
                _field.text = _localization.GetLocalizedString(_localeKeyName);
            }
        }
    }
}
