using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class DeformableGroundData
	{
        protected readonly NormalizedHeightsArray Heights;
        public readonly int Resolution;        
        virtual public GroundDataWorkMode Workmode => GroundDataWorkMode.Mud;
        public byte[] GetTextureBytes() => Heights.ToBytesArray();


        public DeformableGroundData(int resolution)
        {
            Resolution= resolution;
            Heights = new (resolution * resolution);
        }
        virtual public void Setup(DeformableGroundSettings settings) { }
        virtual public void DrawTouch(Vector2 pos, int radiusInPixels, float lowValue)
        {
            //copied to liquidGroundData
            int posX = Mathf.RoundToInt(pos.x * Resolution), posY = Mathf.RoundToInt(pos.y * Resolution);
            int startX = posX - radiusInPixels, startY = posY - radiusInPixels,
                endX = startX + radiusInPixels, endY = startY + radiusInPixels;
            if (startX < 0) startX = 0;
            if (startY < 0) startY = 0;
            if (endX > Resolution - 1) endX = Resolution - 1;
            if (endY > Resolution - 1) endY = Resolution - 1;

            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    Heights[y * Resolution + x] -= lowValue;
                }
            }
        }

        virtual public void Smooth(float fluidityDelta)
        {
            for (int i = 0; i < Resolution; i += 2)
            {
                for (int j = 0; j < Resolution; j += 2)
                {
                    float a = Heights[i * Resolution + j],
                        b = Heights[i * Resolution + j + 1],
                        c = Heights[(i + 1) * Resolution + j],
                        d = Heights[(i + 1) * Resolution + j + 1];

                    float minValue = a;
                    CheckMin(b); 
                    CheckMin(c);
                    CheckMin(d);

                    Heights[i * Resolution + j] = MoveValue(a);
                    Heights[i * Resolution + j + 1] = MoveValue(b);
                    Heights[(i + 1) * Resolution + j] = MoveValue(c);
                    Heights[(i + 1) * Resolution + j + 1] = MoveValue(d);

                    void CheckMin(float val)
                    {
                        if (val < minValue)
                        {
                            minValue = val;
                        }
                    }
                    float MoveValue(float val)
                    {
                        if (Mathf.Abs(val) > minValue) val = Mathf.MoveTowards(val, minValue, fluidityDelta);
                        return val;
                    }
                }
            }
        }
        virtual public int RestoreHeight(float delta)
        {
            int cellsUsed = 0;
            for (int i = 0; i < Resolution; i++)
            {
                for (int j = 0; j < Resolution; j++)
                {
                    float value = Heights[i * Resolution + j];

                    if (value != 0f)
                    {
                        Heights[i * Resolution + j] = Mathf.MoveTowards(value, 0f, delta);
                        cellsUsed++;
                    }
                }
            }
            return cellsUsed;
        }        

    }
}
