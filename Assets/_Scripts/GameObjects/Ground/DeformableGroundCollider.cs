using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class DeformableGroundCollider : DepthGroundCollider
	{       
        [SerializeField] protected Renderer _renderer;
        [SerializeField] protected DeformableGroundSettings _deformationSettings;
        protected bool _materialHandlerActive = false, _depthSettingsPresented = false;
        protected Vector3 _up;
        protected Material _cachedMaterial;
        protected GroundMaterialHandler _materialHandler;
        protected GroundMaterialHandler.Pool _handlersPool;

        [Inject]
        public void Inject(GroundMaterialHandler.Pool handlersPool)
        {
            _handlersPool= handlersPool;
        }

        protected override void Start()
        {
            base.Start();
            _cachedMaterial = _renderer.sharedMaterial;
            _up = transform.up;
            _depthSettingsPresented = _depthSettings != null;
        }
        private void Update()
        {
            if (_materialHandlerActive)
            {
                if (_materialHandler.UpdateGround(Time.deltaTime))
                {
                    i_ReleaseMaterialHandler(true);
                }
            }
        }

        protected override GroundCastInfo FormCastInfo(WheelCollisionInfo info)
        {
            Vector2 internalCoords = GetInternalCoordinates(info.Pos);

            if (_materialHandler == null)
            {
                RequestMaterialHandler();
                _materialHandler?.OnWheelTouched(internalCoords, info.WheelRadius / _squareSize);
            }
            else _materialHandler.OnWheelTouched(internalCoords, info.WheelRadius / _squareSize);

            return new GroundCastInfo(_passabilityParameters.Harshness,_passabilityParameters.Resistance,  _depthSettingsPresented ? _depthSettings.GetDepth(internalCoords) : 0f);
        }

        private void RequestMaterialHandler()
        {
            _materialHandler = _handlersPool.GetHandler();
            _materialHandlerActive = _materialHandler != null;
            if (_materialHandlerActive)
            {
                _materialHandler.StartHandling(_groundType, _deformationSettings, _renderer);
                _materialHandler.OnDisposeEvent += ReleaseMaterialHandler;
            }
        }
        private void ReleaseMaterialHandler() => i_ReleaseMaterialHandler(false);
        private void i_ReleaseMaterialHandler(bool callFromInside)
        {
            if (_materialHandler != null)
            {
                _materialHandler.OnDisposeEvent -= ReleaseMaterialHandler;
                if (callFromInside) _materialHandler.Dispose();
                _materialHandler = null;                
            }
            _materialHandlerActive = false;
            _renderer.sharedMaterial = _cachedMaterial;
        }
    }
}
