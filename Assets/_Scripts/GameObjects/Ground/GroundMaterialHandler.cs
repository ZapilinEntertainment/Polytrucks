using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ZE.Polytrucks {
	public sealed class GroundMaterialHandler
	{
        private bool _isGroundClear = false;

        private GroundMaterialContainer _materialContainer;
        private DeformableGroundSettings _settings;
        private DeformableGroundData _groundData;
        private readonly GroundMaterialsDepot _materialsDepot;
        private readonly IObjectPool<GroundMaterialHandler> _pool;
        private readonly Texture2D _deformMap;
        private readonly GroundQualitySettings _qualitySettings;
        

        private const string DEFORM_TEX_PROPERTY = "_DeformMap", HEIGHT_DELTA_PROPERTY = "_HeightDelta";

        public System.Action OnDisposeEvent; // when not enough objects in pool
        
        public GroundMaterialHandler(GroundMaterialsDepot groundMaterialsDepot, IObjectPool<GroundMaterialHandler> pool, GroundQualitySettings qualitySettings)
        {            
            _materialsDepot= groundMaterialsDepot;
            _pool = pool;
            _qualitySettings= qualitySettings;

            int resolution = _qualitySettings.DeformMapResolution;
            _groundData = new LiquidGroundData(resolution);
            
            _deformMap = new Texture2D(resolution, resolution, TextureFormat.Alpha8, false);
        }

        public void StartHandling(GroundType groundType, DeformableGroundSettings settings, Renderer renderer)
        {

            Material material;
            DeformedMaterialID id = new DeformedMaterialID(groundType);
            if (!_materialsDepot.TryReuseMaterial(id, out material))
            {
                material = renderer.material;                
            }

            _settings = settings;
            _groundData.Setup(_settings);
            material.SetTexture(DEFORM_TEX_PROPERTY, _deformMap);
            material.SetFloat(HEIGHT_DELTA_PROPERTY, settings.VisualHeight);

            _materialContainer = new GroundMaterialContainer(id, material);
            renderer.sharedMaterial = material;

            _isGroundClear = false;
        }

        /// <returns> true if handler can be disposed cause ground is clear again </returns>
        public bool UpdateGround(float deltaTime)
        {
             _groundData.Smooth(deltaTime * _settings.Fluidity);

            int changedCellsCount = _groundData.RestoreHeight((deltaTime / _qualitySettings.ClearTime) * _settings.RestoreSpeedCf);

            _deformMap.LoadRawTextureData(_groundData.GetTextureBytes());
            _deformMap.Apply();
            _isGroundClear = changedCellsCount == 0;
            return _isGroundClear;
        }

        public void Dispose()
        {
            _materialsDepot.CacheMaterial(_materialContainer);
            _pool.Release(this);
            OnDisposeEvent?.Invoke();
        }

        public void OnWheelTouched(Vector2 pos, float radius, float affectionValue = 1f)
        {
            int radiusInPixels = Mathf.RoundToInt(radius * _groundData.Resolution);
            if (radiusInPixels == 0) radiusInPixels = 1;
            if (Random.value > 0.5f) radiusInPixels++;
            _groundData.DrawTouch(pos, radiusInPixels, affectionValue * _settings.AffectionForceCf);
        }

        public class Pool
        {
            private readonly GroundQualitySettings _groundQualitySettings;
            private readonly GroundMaterialsDepot _materialsDepot;
            private readonly ObjectPool<GroundMaterialHandler> _pool;
            public Pool(GroundMaterialsDepot materialsDepot, GroundQualitySettings qualitySettings) { 
                _materialsDepot= materialsDepot;
                _groundQualitySettings= qualitySettings;
                _pool = new ObjectPool<GroundMaterialHandler>(createFunc: Create, collectionCheck: false, maxSize: 4);
            }

            private GroundMaterialHandler Create() => new GroundMaterialHandler(_materialsDepot, _pool, _groundQualitySettings);
            public GroundMaterialHandler GetHandler() => _pool.Get();

        }
    }
}
