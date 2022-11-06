using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Polytrucks
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _camLookPoint;

        private TruckController _truck;
        private Joystick _joystick;
        private Transform _cameraTransform;

        private GUIStyle black;

        public float SpeedPc => _truck?.SpeedPc ?? 0f;

        private void Start()
        {
            black = new GUIStyle(EditorStyles.label);
            black.normal.textColor = Color.black;

            _truck = GetComponentInChildren<TruckController>();
            _joystick = FindObjectOfType<Joystick>();
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            Vector3 move = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
            Vector3 fwd = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up);
            if (fwd.sqrMagnitude == 0f) fwd = Vector3.ProjectOnPlane(_cameraTransform.up, Vector3.up);
           // var rotation = Quaternion.FromToRotation(fwd.normalized, Vector3.forward);
            _truck.Move( new Vector2(move.x, move.z));

            _camLookPoint.position = _truck.transform.TransformPoint(Vector3.forward * 5f * SpeedPc);
        }

        private void OnGUI()
        {
            
            GUILayout.Label(new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical).ToString(), black);
        }
    }
}
