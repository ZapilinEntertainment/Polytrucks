using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ZE.Polytrucks {
#if UNITY_EDITOR
    public sealed class TrailerJointConfigEditor : Editor
	{
        [MenuItem("CONTEXT/ConfigurableJoint/SaveJoint")]
        static void SaveJoint(MenuCommand command)
        {
            ConfigurableJoint joint = (ConfigurableJoint)command.context;

            var config = ScriptableObject.CreateInstance<TrailerJointConfig>();
            config.SaveValuesFrom(joint);
            AssetDatabase.CreateAsset(config, $"Assets/{joint.name}Config.asset");
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
