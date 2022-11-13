using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Polytrucks
{
    public sealed class Player : MonoBehaviour
    {
        [SerializeField] private Transform _camLookPoint;

        private TruckController _truck;
        private Joystick _joystick;
        private Transform _cameraTransform, _truckTransform;

        private GUIStyle black;

        public float SpeedPc => _truck?.SpeedPc ?? 0f;
        public Vector3 Position { get; private set; }

        private void Start()
        {
            black = new GUIStyle(EditorStyles.label);
            black.normal.textColor = Color.black;

            _truck = GetComponentInChildren<TruckController>();
            _truckTransform = _truck.transform;
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

            _camLookPoint.position = _truckTransform.TransformPoint(Vector3.forward * 5f * SpeedPc);
        }
        private void FixedUpdate()
        {
            Position = _truckTransform?.position ?? transform.position;
        }

        private void OnGUI()
        {
            
            GUILayout.Label(new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical).ToString(), black);
        }
    }
}
