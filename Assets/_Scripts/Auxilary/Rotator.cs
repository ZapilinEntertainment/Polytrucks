using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationVector = Vector3.up * 90f;
        void Update()
        {
            transform.Rotate(_rotationVector * Time.deltaTime, Space.Self);
        }
    }
}
