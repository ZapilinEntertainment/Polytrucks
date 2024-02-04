using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ZE.Polytrucks {
	public sealed class GroundMaterialHandler
	{
        private bool _isGroundClear = false;
        private float _fluidity = 1f;

        private GroundMaterialContainer _materialContainer;
        private readonly GroundMaterialsDepot _materialsDepot;
        private readonly IObjectPool<GroundMaterialHandler> _pool;
        private readonly Texture2D _deformMap;
        private readonly GroundQualitySettings _qualitySettings;
        private readonly byte[] _values;

        private const string DEFORM_TEX_PROPERTY = "_DeformMap", HEIGHT_DELTA_PROPERTY = "_HeightDelta";

        public System.Action OnDisposeEvent; // when not enough objects in pool
        
        public GroundMaterialHandler(GroundMaterialsDepot groundMaterialsDepot, IObjectPool<GroundMaterialHandler> pool, GroundQualitySettings qualitySettings)
        {            
            _materialsDepot= groundMaterialsDepot;
            _pool = pool;
            _qualitySettings= qualitySettings;

            int resolution = _qualitySettings.DeformMapResolution;
            _values = new byte[resolution * resolution];
            _deformMap = new Texture2D(resolution, resolution, TextureFormat.Alpha8, false);
        }

        public void StartHandling(GroundSettings settings, Renderer renderer)
        {
            Material material;
            DeformedMaterialID id = new DeformedMaterialID(settings.GroundType);
            if (!_materialsDepot.TryReuseMaterial(id, out material))
            {
                material = renderer.material;                
            }

            material.SetTexture(DEFORM_TEX_PROPERTY, _deformMap);
            material.SetFloat(HEIGHT_DELTA_PROPERTY, settings.HeightDelta);
            _fluidity = settings.Fluidity;

            _materialContainer = new GroundMaterialContainer(id, material);
            renderer.sharedMaterial = material;

            _isGroundClear = false;
        }

        /// <returns> true if handler can be disposed cause ground is clear again </returns>
        public bool UpdateGround(float deltaTime)
        {
            int cellsUsed = 0, resolution = _qualitySettings.DeformMapResolution;
            byte delta = (byte)(255 * (deltaTime / _qualitySettings.ClearTime)),
                fluidityDelta = (byte)(deltaTime * _fluidity);
            if (delta == 0) delta = 1;


            for (int i = 0; i < resolution; i += 2)
            {
                for (int j = 0; j < resolution; j += 2)
                {
                    byte a = _values[i * resolution + j],
                        b = _values[i * resolution + j + 1],
                        c = _values[(i + 1) * resolution + j],
                        d = _values[(i + 1) * resolution + j + 1];

                    byte minValue = a;
                    if (b < minValue) minValue = b;
                    if (c < minValue) minValue = c;
                    if (d < minValue) minValue = d;

                    _values[i * resolution + j] = MoveValue(a);
                    _values[i * resolution + j + 1] = MoveValue(b);
                    _values[(i + 1) * resolution + j] = MoveValue(c);
                    _values[(i + 1) * resolution + j + 1] = MoveValue(d);

                    byte MoveValue(byte val)
                    {
                        if (val > minValue) val -= fluidityDelta;
                        return val;
                    }
                }
            }

            /*
            for (int i = 0; i < resolution; i+=2)
            {
                for (int j = 0; j < resolution; j+=2)
                {
                    byte a = _values[i * resolution + j],
                        b = _values[i * resolution + j + 1],
                        c = _values[(i + 1) * resolution + j],
                        d = _values[(i + 1) * resolution + j + 1];

                    float middleSum = a + b + c + d;
                    byte middle = (byte)(middleSum / 4f);
                    _values[i * resolution + j] = MoveValue(a);
                    _values[i * resolution + j + 1] = MoveValue(b);
                    _values[(i + 1) * resolution + j] = MoveValue(c);
                    _values[(i + 1) * resolution + j + 1] = MoveValue(d);

                    byte MoveValue(byte val)
                    {
                        if (val != middle) if (val > middle) val -= fluidityDelta; else val += fluidityDelta;
                        return val;
                    }
                }
            }
            */

            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    byte value = _values[i * resolution + j];

                    if (value < delta)
                    {
                        value = 0;
                    }
                    else
                    {
                        value -= delta;
                        cellsUsed++;
                    }
                    _values[i * resolution + j] = value;
                }
            }

            _deformMap.LoadRawTextureData(_values);
            _deformMap.Apply();

            _isGroundClear = cellsUsed == 0;
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
            int resolution = _qualitySettings.DeformMapResolution;
            int radiusInPixels = Mathf.RoundToInt(radius * resolution);
            if (radiusInPixels == 0) radiusInPixels = 1;
            if (Random.value > 0.5f) radiusInPixels ++;
            int posX = Mathf.RoundToInt(pos.x * resolution), posY = Mathf.RoundToInt(pos.y * resolution);
            int startX = posX - radiusInPixels, startY = posY - radiusInPixels, 
                endX = startX + radiusInPixels, endY = startY + radiusInPixels;
            if (startX < 0) startX = 0; 
            if (startY < 0) startY = 0;
            if (endX > resolution - 1) endX= resolution - 1;
            if (endY > resolution - 1) endY= resolution - 1;
            int count = 0;
            Vector2 center = new Vector2(posX, posY);
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    int index = y * resolution + x;
                    float sumVal = _values[index] + affectionValue * 255f;
                    if (sumVal > 255) _values[index] = 255;
                    else _values[index] = (byte)sumVal;
                    count++;
                }
            }
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
