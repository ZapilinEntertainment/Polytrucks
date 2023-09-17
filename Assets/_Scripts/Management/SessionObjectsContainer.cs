using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class SessionObjectsContainer : MonoBehaviour
	{
		[SerializeField] private SessionMaster _gameManager;
		[SerializeField] private LevelManager _levelManager;
		[SerializeField] private PlayerController _player;
		[SerializeField] private ResourcesManager _resourcesManager;
		[SerializeField] private UIManager _uiManager;
		[SerializeField] private CameraController _cameraController;
		[Space]
		[SerializeField] private GameSettings _gameSettings;

		public static SessionMaster GameManager => s_current?._gameManager;
		public static LevelManager LevelManager => s_current?._levelManager;
		public static PlayerController PlayerController => s_current?._player;
		public static ResourcesManager ResourcesManager => s_current?._resourcesManager;
		public static UIManager UIManager => s_current?._uiManager;
		public static CameraController CameraController => s_current?._cameraController;	
		public static GameSettings GameSettings
		{
			get
			{
				if (s_current != null) return s_current._gameSettings;
				else return new GameSettings(); 		
			}
		}

		private static SessionObjectsContainer s_current;
		public static bool IsPresented => s_current != null;

        private void Awake()
        {
			s_current = this;
			if (_gameSettings == null)
			{
				_gameSettings = new GameSettings();
				Debug.Log("no game settings in container!");
			}
        }
    }
}
