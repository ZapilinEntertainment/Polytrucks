using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks.TestModule {
	public class TestModuleContainer : MonoBehaviour
	{
		[field: SerializeField] public bool UseTestKeys { get; private set; } = true;
		[field: SerializeField] public PlayerDataSavePreset SavePreset { get; private set; }
	}
}
