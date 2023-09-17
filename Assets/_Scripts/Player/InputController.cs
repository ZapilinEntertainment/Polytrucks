using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class InputController : SessionObject
	{
        private PlayerController _player;
        private void Start()
        {
            _player = SessionObjectsContainer.PlayerController;
        }

        public void MoveCommand(Vector2 dir)
        {
            if (GameSessionActive) _player.Move(dir);
        }
    }
}
