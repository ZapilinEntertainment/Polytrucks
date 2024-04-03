using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class EndGameWindow : AppearWindow
	{
		//[SerializeField] private TMPro.TMP_Text _rewardLabel;
		//protected abstract int GetReward();

        override protected void i_Show()
        {
           // _rewardLabel.text = '+' + GetReward().ToString();
            base.i_Show();
        }

    }
}
