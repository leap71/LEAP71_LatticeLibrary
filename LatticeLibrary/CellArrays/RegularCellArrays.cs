//
// SPDX-License-Identifier: Apache-2.0
//
// The LEAP 71 ShapeKernel is an open source geometry engine
// specifically for use in Computational Engineering Models (CEM).
//
// For more information, please visit https://leap71.com/shapekernel
// 
// This project is developed and maintained by LEAP 71 - © 2023 by LEAP 71
// https://leap71.com
//
// Computational Engineering will profoundly change our physical world in the
// years ahead. Thank you for being part of the journey.
//
// We have developed this library to be used widely, for both commercial and
// non-commercial projects alike. Therefore, have released it under a permissive
// open-source license.
// 
// The LEAP 71 ShapeKernel is based on the PicoGK compact computational geometry 
// framework. See https://picogk.org for more information.
//
// LEAP 71 licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with the
// License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, THE SOFTWARE IS
// PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED.
//
// See the License for the specific language governing permissions and
// limitations under the License.   
//


using System.Numerics;
using PicoGK;


namespace Leap71
{
    using ShapeKernel;

    namespace LatticeLibrary
    {
        public interface ICellArray
        {
            public List<IUnitCell> aGetUnitCells();
        }

        public class RegularCellArray : ICellArray
	    {
            protected Voxels            m_oVoxels;
            protected float             m_dX;
            protected float             m_dY;
            protected float             m_dZ;
            protected List<IUnitCell>   m_aUnitCells;
            protected float             m_fNoiseLevel;
            protected Random            m_oRandom;


            /// <summary>
            /// Regular grid cell array with customised x-, y-, z- unit cell dimensions.
            /// Grid houses the bounding box of the specified voxelfield.
            /// The noise level must be zero for a perfectly regular grid.
            /// The noise level is restricted to 0.3 as the maximum.
            /// </summary>
            public RegularCellArray(
                Voxels oVoxels,
                float dX,
                float dY,
                float dZ,
                float fNoiseLevel = 0)
		    {
                m_dX            = dX;
                m_dY            = dY;
                m_dZ            = dZ;
                m_fNoiseLevel   = Uf.fLimitValue(MathF.Abs(fNoiseLevel), 0f, 0.3f);
                m_oVoxels       = oVoxels;
                m_oVoxels.CalculateProperties(out float fVolumeCubicMM, out BBox3 oBBox);

                m_aUnitCells = new List<IUnitCell>();
                for (float fX = oBBox.vecMin.X - 0.5f * m_dX; fX <= oBBox.vecMax.X + 0.5f * m_dX; fX += m_dX)
                {
                    for (float fY = oBBox.vecMin.Y - 0.5f * m_dY; fY <= oBBox.vecMax.Y + 0.5f * m_dY; fY += m_dY)
                    {
                        for (float fZ = oBBox.vecMin.Z - 0.5f * m_dZ; fZ <= oBBox.vecMax.Z + 0.5f * m_dZ; fZ += m_dZ)
                        {
                            Vector3 vecCorner_01 = vecGetCorner(fX, fY, fZ);
                            Vector3 vecCorner_02 = vecGetCorner(fX, fY + m_dY, fZ);
                            Vector3 vecCorner_03 = vecGetCorner(fX + m_dX, fY + m_dY, fZ);
                            Vector3 vecCorner_04 = vecGetCorner(fX + m_dX, fY, fZ);
                            Vector3 vecCorner_05 = vecGetCorner(fX, fY, fZ + m_dZ);
                            Vector3 vecCorner_06 = vecGetCorner(fX, fY + m_dY, fZ + m_dZ);
                            Vector3 vecCorner_07 = vecGetCorner(fX + m_dX, fY + m_dY, fZ + m_dZ);
                            Vector3 vecCorner_08 = vecGetCorner(fX + m_dX, fY, fZ + m_dZ);

                            IUnitCell xCell = new CuboidCell(
                                vecCorner_01,
                                vecCorner_02,
                                vecCorner_03,
                                vecCorner_04,
                                vecCorner_05,
                                vecCorner_06,
                                vecCorner_07,
                                vecCorner_08);
                            m_aUnitCells.Add(xCell);
                        }
                    }
                }
            }

            protected Vector3 vecGetCorner(float fX, float fY, float fZ)
            {
                int iX = (int)(fX * 1000f);
                int iY = (int)(fY * 1000f);
                int iZ = (int)(fZ * 1000f);
                int iIndex  = iX * iY * iZ;
                m_oRandom   = new Random(iIndex);

                Vector3 vecCorner = new Vector3(fX, fY, fZ) + vecGetNoise();
                return vecCorner;
            }

            protected Vector3 vecGetNoise()
            {
                Vector3 vecNoise = new Vector3(
                    fGetRandomLinear(-m_fNoiseLevel * m_dX, m_fNoiseLevel * m_dX),
                    fGetRandomLinear(-m_fNoiseLevel * m_dY, m_fNoiseLevel * m_dY),
                    fGetRandomLinear(-m_fNoiseLevel * m_dZ, m_fNoiseLevel * m_dZ));
                return vecNoise;
            }

            public float fGetRandomLinear(float fMin, float fMax)
            {
                float fValue = (float)(fMin + (fMax - fMin) * m_oRandom.NextDouble());
                return fValue;
            }

            public List<IUnitCell> aGetUnitCells()
            {
                return m_aUnitCells;
            }
        }

        public class RegularUnitCell : ICellArray
	    {
            protected float             m_dX;
            protected float             m_dY;
            protected float             m_dZ;
            protected List<IUnitCell>   m_aUnitCells;
            protected float             m_fNoiseLevel;
            protected Random            m_oRandom;


            /// <summary>
            /// Regular grid cell with customised x-, y-, z- unit cell dimensions.
            /// Grid consists of just one unit cell.
            /// The noise level must be zero for a perfectly regular grid.
            /// The noise level is restricted to 0.3 as the maximum.
            /// </summary>
            public RegularUnitCell(
                float dX,
                float dY,
                float dZ,
                float fNoiseLevel = 0)
		    {
                m_dX            = dX;
                m_dY            = dY;
                m_dZ            = dZ;
                m_fNoiseLevel   = Uf.fLimitValue(MathF.Abs(fNoiseLevel), 0f, 0.3f);

                m_aUnitCells = new List<IUnitCell>();
                float fX = -0.5f * m_dX;
                float fY = -0.5f * m_dY;
                float fZ = 0f;

                Vector3 vecCorner_01 = vecGetCorner(fX, fY, fZ);
                Vector3 vecCorner_02 = vecGetCorner(fX, fY + m_dY, fZ);
                Vector3 vecCorner_03 = vecGetCorner(fX + m_dX, fY + m_dY, fZ);
                Vector3 vecCorner_04 = vecGetCorner(fX + m_dX, fY, fZ);
                Vector3 vecCorner_05 = vecGetCorner(fX, fY, fZ + m_dZ);
                Vector3 vecCorner_06 = vecGetCorner(fX, fY + m_dY, fZ + m_dZ);
                Vector3 vecCorner_07 = vecGetCorner(fX + m_dX, fY + m_dY, fZ + m_dZ);
                Vector3 vecCorner_08 = vecGetCorner(fX + m_dX, fY, fZ + m_dZ);

                IUnitCell xCell = new CuboidCell(
                    vecCorner_01,
                    vecCorner_02,
                    vecCorner_03,
                    vecCorner_04,
                    vecCorner_05,
                    vecCorner_06,
                    vecCorner_07,
                    vecCorner_08);
                m_aUnitCells.Add(xCell);
            }

            protected Vector3 vecGetCorner(float fX, float fY, float fZ)
            {
                int iX = (int)(fX * 1000f);
                int iY = (int)(fY * 1000f);
                int iZ = (int)(fZ * 1000f);
                int iIndex  = iX * iY * iZ;
                m_oRandom   = new Random(iIndex);

                Vector3 vecCorner = new Vector3(fX, fY, fZ) + vecGetNoise();
                return vecCorner;
            }

            protected Vector3 vecGetNoise()
            {
                Vector3 vecNoise = new Vector3(
                    fGetRandomLinear(-m_fNoiseLevel * m_dX, m_fNoiseLevel * m_dX),
                    fGetRandomLinear(-m_fNoiseLevel * m_dY, m_fNoiseLevel * m_dY),
                    fGetRandomLinear(-m_fNoiseLevel * m_dZ, m_fNoiseLevel * m_dZ));
                return vecNoise;
            }

            public float fGetRandomLinear(float fMin, float fMax)
            {
                float fValue = (float)(fMin + (fMax - fMin) * m_oRandom.NextDouble());
                return fValue;
            }

            public List<IUnitCell> aGetUnitCells()
            {
                return m_aUnitCells;
            }
        }
    }
}