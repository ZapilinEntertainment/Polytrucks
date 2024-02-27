using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class LiquidGroundData : DeformableGroundData
	{
        [System.Flags]
        private enum CellDirection : byte { Nowhere = 0, Up = 1, Right = 2, Down = 4, Left = 8, AllDirections =Up | Right | Down | Left };

        private readonly struct FlowPoint
        {
            public readonly CellDirection Direction;
            public readonly float Force;
            public readonly float MoveTime;

            public FlowPoint(CellDirection direction, float force, float moveTime)
            {
                Direction = direction;
                Force = force;
                MoveTime = moveTime;
            }

            public FlowPoint Slow(float step)
            {
                if (Force < step) return new FlowPoint();
                else return new FlowPoint(Direction, Force - step, MoveTime);
            }
        }


        private float _fluidity;
        private DeformableGroundSettings.LiquidSettings _liquidSettings;
		private readonly FlowPoint[] _flowPoints;
        public override GroundDataWorkMode Workmode => GroundDataWorkMode.Fluid;



        public LiquidGroundData(int resolution) : base(resolution) {
            int count = Heights.Length;
            _flowPoints = new FlowPoint[count];
		}

        override public void Setup(DeformableGroundSettings settings) {
            _fluidity = settings.Fluidity;
            _liquidSettings = settings.LiquidConfig;
            for (int i = 0; i < Heights.Length; i++)
            {
                _flowPoints[i] = new FlowPoint();
            }
        }

        override public void DrawTouch(Vector2 pos, int radiusInPixels, float lowValue)
        {
            //cosource : DeformableGroundData
            int posX = Mathf.RoundToInt(pos.x * Resolution), posY = Mathf.RoundToInt(pos.y * Resolution);
            int startX = posX - radiusInPixels, startY = posY - radiusInPixels,
                endX = startX + radiusInPixels, endY = startY + radiusInPixels;
            if (startX < 0) startX = 0;
            if (startY < 0) startY = 0;
            if (endX > Resolution - 1) endX = Resolution - 1;
            if (endY > Resolution - 1) endY = Resolution - 1;

            float time = Time.time;
            //Debug.Log(endY * endX);
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    int index = y * Resolution + x;
                    Heights[index] -= lowValue;
                    //
                    int dirX = x - posX, dirY = y - posY;
                    CellDirection flowDirection = 0;
                    if (dirX >= 0) flowDirection |= CellDirection.Right;
                    if (dirX <= 0) flowDirection |= CellDirection.Left;
                    if (dirY >= 0) flowDirection |= CellDirection.Up;
                    if (dirY <= 0) flowDirection |= CellDirection.Down;


                   
                   // flowDirection = CellDirection.AllDirections;
                   // flowDirection -= ~flowDirection;

                    _flowPoints[index] = new FlowPoint(flowDirection, _liquidSettings.InitialForceCf * Mathf.Abs(lowValue), time + _liquidSettings.WaveGenerationTime);
                    //
                }
            }
        }

        override public void Smooth(float fluidityDelta)
        {
            float time = Time.time;
            for (int row = 0; row < Resolution; row ++)
            {
                for (int column = 0; column < Resolution; column ++)
                {
                    
                    int originCellIndex = row * Resolution + column;
                    FlowPoint flowPoint = _flowPoints[originCellIndex];
                    CellDirection waveDirection = flowPoint.Direction;
                    
                    if (waveDirection != CellDirection.Nowhere && flowPoint.MoveTime < time)
                    {
                        float nextForce = flowPoint.Force * _liquidSettings.WaveDownforceCf;
                        if (nextForce > _liquidSettings.MinWaveValue) {

                            bool WaveLeft = (column != 0) && waveDirection.HasFlag(CellDirection.Left),
                                WaveRight = (column != Resolution - 1) && waveDirection.HasFlag(CellDirection.Right),
                                WaveUp = (row != Resolution - 1) && waveDirection.HasFlag(CellDirection.Up),
                                WaveDown = (row != 0) && waveDirection.HasFlag(CellDirection.Down);

                            if (WaveLeft)
                            {
                                if (WaveUp) CheckCell(row + 1, column - 1);
                                CheckCell(row, column - 1);
                                if (WaveDown) CheckCell(row - 1, column - 1);
                            }
                            if (WaveUp) CheckCell(row + 1, column);
                            if (WaveDown) CheckCell(row - 1, column);
                            if (WaveRight)
                            {
                                if (WaveUp) CheckCell(row + 1, column + 1);
                                CheckCell(row, column + 1);
                                if (WaveDown) CheckCell(row - 1, column + 1);
                            }
                        }

                        void CheckCell(int row, int column)
                        {
                            int index = row * Resolution + column;
                            _flowPoints[index] = new FlowPoint(waveDirection, nextForce, time + _liquidSettings.WaveGenerationTime);
                            Heights[index] += flowPoint.Force * _liquidSettings.WaveHeightCf;
                        }
                    }
                }
            }
        }

        override public int RestoreHeight(float delta)
        {
            int cellsUsed = 0;
            float time = Time.time;
            for (int i = 0; i < Resolution; i++)
            {
                for (int j = 0; j < Resolution; j++)
                {
                    int index = i * Resolution + j;
                    float value = Heights[index];

                    if (value != 0f)
                    {
                        Heights[index] = Mathf.MoveTowards(value, 0f, delta);
                        cellsUsed++;
                    }
                    else
                    {
                        Heights[index] = 0f;
                    }

                    var flow = _flowPoints[index];
                    if (flow.Direction != CellDirection.Nowhere && flow.MoveTime <time)
                    {
                        _flowPoints[index] = new();
                    }
                }
            }
            return cellsUsed;
        }
    }
}
