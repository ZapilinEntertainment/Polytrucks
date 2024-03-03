using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public enum TutorialAdviceID : byte
    {
        GasAdvice, SteerAdvice, ReverseAdvice, CollectAdvice,
        SellAdvice, BuildingRestoreAdvice, ExploreAdvice,
        Count
    }
    public static class TutorialAdviceIDExtension
    {
        public static IntCompleteMask GetEmptyMask() => new IntCompleteMask((int)TutorialAdviceID.Count);
        public static IntCompleteMask GetFullMask() => new IntCompleteMask();
    }
    public abstract class TutorialAdvice : MonoBehaviour, IDynamicLocalizer
	{
        [field: SerializeField] protected TMPro.TMP_Text AdviceLabel { get; private set; }
        protected Localization Localization { get; private set; }
        public System.Action OnCompleteEvent;
        abstract public TutorialAdviceID AdviceID { get; }

        [Inject]
        public void Setup(Localization localization)
        {
            Localization = localization;
            Localization.Subscribe(this);
            OnLocaleChanged();
        }
        virtual public void OnComplete() {
			OnCompleteEvent?.Invoke();
            Localization?.Unsubscribe(this);
            Destroy(gameObject);
        }

        public void OnLocaleChanged()
        {
            AdviceLabel.text = Localization.GetLocalizedTutorialAdvice(AdviceID);
        }
        
    }
}
