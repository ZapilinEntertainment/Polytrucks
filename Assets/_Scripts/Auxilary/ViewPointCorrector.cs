using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ZE.Polytrucks {
	public sealed class ViewPointCorrector : MonoBehaviour
	{
        [SerializeField] private float _maxOffset = 10f;
        [SerializeField] private Vehicle _vehicle;
        private Vector3 _startPoint;
        private void Awake()
        {
            _startPoint = transform.localPosition;
        }
        private void OnEnable()
        {
            // Add WriteLogMessage as a delegate of the RenderPipelineManager.beginCameraRendering event
            RenderPipelineManager.beginCameraRendering += OnBeginRender;
        }

        // Unity calls this method automatically when it disables this component
        private void OnDisable()
        {
            // Remove WriteLogMessage as a delegate of the  RenderPipelineManager.beginCameraRendering event
            RenderPipelineManager.beginCameraRendering -= OnBeginRender;
        }

        private void OnBeginRender(ScriptableRenderContext context, Camera camera)
        {
            transform.localPosition = _startPoint + _vehicle.SpeedPc * _maxOffset * Vector3.forward;
        }
    }
}
