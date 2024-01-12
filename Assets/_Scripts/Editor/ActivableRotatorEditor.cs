using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ZE.Polytrucks {

#if UNITY_EDITOR
    [CustomEditor(typeof(ActivableRotator))]
    public class LookAtPointEditor : Editor
    {
        [SerializeField] private bool EDITOR_showRotation = false;
        private Quaternion _cachedRotation;
        private SerializedProperty _activeProperty, _rotationVectorProperty;
        private Transform _transform;

        void OnEnable()
        {
            _activeProperty = serializedObject.FindProperty("_isActive");
            _rotationVectorProperty = serializedObject.FindProperty("_rotationVector");
            _transform = (target as MonoBehaviour).transform;
        }
        private void OnDisable()
        {
            if (EDITOR_showRotation) StopRotation();
        }
        private void OnSceneGUI()
        {
            if (EDITOR_showRotation && _transform != null)
            {
                _transform.Rotate(_rotationVectorProperty.vector3Value * Time.deltaTime, Space.Self);
            }
        }
        private void StopRotation()
        {
            EDITOR_showRotation = false;
            if (_transform != null) _transform.rotation = _cachedRotation;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_activeProperty);
            EditorGUILayout.PropertyField(_rotationVectorProperty);
            serializedObject.ApplyModifiedProperties();

            if (!EDITOR_showRotation)
            {
                if (GUILayout.Button("Play"))
                {
                    EDITOR_showRotation = true;
                    if (_transform != null) _cachedRotation = _transform.rotation;
                }
            }
            else
            {
                if (GUILayout.Button("Stop")) StopRotation();
            }
        }
    }
#endif
}
