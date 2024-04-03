using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {

    public enum UIPanel : byte { None = 0, Garage, ActionPanel, Total }
    public sealed class PanelsManager
	{
		private readonly UIInstaller.ElementsResolver _elementsResolver;
		private readonly SignalBus _signalBus;
		private GaragePanel _garagePanel = null;
		private ActionPanel _actionPanel = null;

		public PanelsManager(UIInstaller.ElementsResolver elementsResolver, SignalBus signalBus)
		{
			_elementsResolver = elementsResolver;
			_signalBus = signalBus;


            signalBus.Subscribe((GarageOpenedSignal signal) => OpenGaragePanel(signal.Garage));
            signalBus.Subscribe<GarageClosedSignal>(() => ClosePanel(UIPanel.Garage)); 
        }
		public void OpenGaragePanel(Garage garage)
		{
			if (_garagePanel == null)
			{
				_garagePanel = _elementsResolver.GaragePanel;	
			}
			_garagePanel.Open(garage);
		}
		public void ClosePanel(UIPanel panel)
		{
			if (panel == UIPanel.Garage)
			{
				if (_garagePanel != null && _garagePanel.IsActive)	_garagePanel.Close();
			}
		}


		public int OpenActionPanel(ActionContainer container)
		{
			if (_actionPanel == null)
			{
				_actionPanel = _elementsResolver.ActionPanel;
			}
			return _actionPanel.Show(container);			
		}
		public void CloseActionPanel(int actionID) => _actionPanel.Hide(actionID);
	}
}
