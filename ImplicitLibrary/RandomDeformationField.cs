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
        public class RandomDeformationField
		{
            Vector3[,,]     m_aGridPoints;
            Vector3[,,]     m_aNoiseVectors;
            BBox3           m_oBBox;
            uint            m_nXSamples;
            uint            m_nYSamples;
            uint            m_nZSamples;
         

            /// <summary>
            /// Stores a regular cuboid 3D grid with 3D random values at each grid point.
            /// Allows for tri-linear spatial interpolation for 3D modulations.
            /// </summary>
            public RandomDeformationField(
                BBox3 oBoundingBox,
                float fResolution,
                float fMinValue,
                float fMaxValue)
			{
                m_oBBox             = oBoundingBox;
                float fXSize        = m_oBBox.vecSize().X;
                float fYSize        = m_oBBox.vecSize().Y;
                float fZSize        = m_oBBox.vecSize().Z;
                m_nXSamples         = (uint)(fXSize / fResolution) + 1;
                m_nYSamples         = (uint)(fYSize / fResolution) + 1;
                m_nZSamples         = (uint)(fZSize / fResolution) + 1;

                // preview
                Sh.PreviewPoint(new (), 0.5f, Cp.clrRed);
                Sh.PreviewBoxWireframe(m_oBBox, Cp.clrRed);

                // sub cubes
                for (int iX = 1; iX <= m_nXSamples; iX++)
                {
                    for (int iY = 1; iY <= m_nYSamples; iY++)
                    {
                        for (int iZ = 1; iZ <= m_nZSamples; iZ++)
                        {
                            float fMinX = m_oBBox.vecMin.X + 1f / (float)m_nXSamples * (iX - 1f) * fXSize;
                            float fMaxX = m_oBBox.vecMin.X + 1f / (float)m_nXSamples * (iX - 0f) * fXSize;
                            float fMinY = m_oBBox.vecMin.Y + 1f / (float)m_nYSamples * (iY - 1f) * fYSize;
                            float fMaxY = m_oBBox.vecMin.Y + 1f / (float)m_nYSamples * (iY - 0f) * fYSize;
                            float fMinZ = m_oBBox.vecMin.Z + 1f / (float)m_nZSamples * (iZ - 1f) * fZSize;
                            float fMaxZ = m_oBBox.vecMin.Z + 1f / (float)m_nZSamples * (iZ - 0f) * fZSize;

                            Vector3 vecFramePos = new Vector3(  0.5f * (fMinX + fMaxX), 
                                                                0.5f * (fMinY + fMaxY),
                                                                fMinZ);
                            float dX = fMaxX - fMinX;
                            float dY = fMaxY - fMinY;
                            float dZ = fMaxZ - fMinZ;
                            BaseBox oSubCube = new (new LocalFrame(vecFramePos), dZ, dX, dY);
                            Sh.PreviewBoxWireframe(oSubCube, Cp.clrBlack);
                        }
                    }
                }

                // ref points
                m_aGridPoints   = new Vector3[m_nXSamples + 1, m_nYSamples + 1, m_nZSamples + 1];
                m_aNoiseVectors = new Vector3[m_nXSamples + 1, m_nYSamples + 1, m_nZSamples + 1];

                for (int iX = 0; iX <= m_nXSamples; iX++)
                {
                    for (int iY = 0; iY <= m_nYSamples; iY++)
                    {
                        for (int iZ = 0; iZ <= m_nZSamples; iZ++)
                        {
                            // rigid grid point
                            float fX        = fXSize / (float)m_nXSamples * iX + m_oBBox.vecMin.X;
                            float fY        = fYSize / (float)m_nYSamples * iY + m_oBBox.vecMin.Y;
                            float fZ        = fZSize / (float)m_nZSamples * iZ + m_oBBox.vecMin.Z;
                            Vector3 vecPt   = new Vector3(fX, fY, fZ);
                            
                            m_aGridPoints[iX, iY, iZ] = vecPt;
                            Sh.PreviewPoint(vecPt, 1.0f, Cp.clrRandom());

                            // random vector
                            Vector3 vecNoise = new Vector3(
                                Uf.fGetRandomLinear(fMinValue, fMaxValue),
                                Uf.fGetRandomLinear(fMinValue, fMaxValue),
                                Uf.fGetRandomLinear(fMinValue, fMaxValue));
                            m_aNoiseVectors[iX, iY, iZ] = vecNoise;
                        }
                    }
                }
            }

            /// <summary>
            /// Returns a tri-linear interpolation from the discrete data field based on the point's position.
            /// https://en.wikipedia.org/wiki/Trilinear_interpolation
            /// </summary>
            public Vector3 vecGetData(Vector3 vecPt)
            {
                // restict point to bounding box
                float fX            = float.Clamp(vecPt.X, m_oBBox.vecMin.X, m_oBBox.vecMax.X);
                float fY            = float.Clamp(vecPt.Y, m_oBBox.vecMin.Y, m_oBBox.vecMax.Y);
                float fZ            = float.Clamp(vecPt.Z, m_oBBox.vecMin.Z, m_oBBox.vecMax.Z);

                float dX            = m_oBBox.vecSize().X / (float)m_nXSamples;
                float dY            = m_oBBox.vecSize().Y / (float)m_nYSamples;
                float dZ            = m_oBBox.vecSize().Z / (float)m_nZSamples;
                int iLowerX         = (int)((fX - m_oBBox.vecMin.X) / dX);
                int iLowerY         = (int)((fY - m_oBBox.vecMin.Y) / dY);
                int iLowerZ         = (int)((fZ - m_oBBox.vecMin.Z) / dZ);
                int iUpperX         = iLowerX + 1;
                int iUpperY         = iLowerY + 1;
                int iUpperZ         = iLowerZ + 1;

                // get corner points
                Vector3 vecPt000    = m_aGridPoints[iLowerX, iLowerY, iLowerZ];
                Vector3 vecPt100    = m_aGridPoints[iUpperX, iLowerY, iLowerZ];
                Vector3 vecPt110    = m_aGridPoints[iUpperX, iUpperY, iLowerZ];
                Vector3 vecPt010    = m_aGridPoints[iLowerX, iUpperY, iLowerZ];
                Vector3 vecPt001    = m_aGridPoints[iLowerX, iLowerY, iUpperZ];
                Vector3 vecPt101    = m_aGridPoints[iUpperX, iLowerY, iUpperZ];
                Vector3 vecPt111    = m_aGridPoints[iUpperX, iUpperY, iUpperZ];
                Vector3 vecPt011    = m_aGridPoints[iLowerX, iUpperY, iUpperZ];

                // get corner vectors
                Vector3 vecDir000   = m_aNoiseVectors[iLowerX, iLowerY, iLowerZ];
                Vector3 vecDir100   = m_aNoiseVectors[iUpperX, iLowerY, iLowerZ];
                Vector3 vecDir110   = m_aNoiseVectors[iUpperX, iUpperY, iLowerZ];
                Vector3 vecDir010   = m_aNoiseVectors[iLowerX, iUpperY, iLowerZ];
                Vector3 vecDir001   = m_aNoiseVectors[iLowerX, iLowerY, iUpperZ];
                Vector3 vecDir101   = m_aNoiseVectors[iUpperX, iLowerY, iUpperZ];
                Vector3 vecDir111   = m_aNoiseVectors[iUpperX, iUpperY, iUpperZ];
                Vector3 vecDir011   = m_aNoiseVectors[iLowerX, iUpperY, iUpperZ];

                // interpolate position within sub-cube
                float fXRatio       = (fX - vecPt000.X) / dX;
                float fYRatio       = (fY - vecPt000.Y) / dY;
                float fZRatio       = (fZ - vecPt000.Z) / dZ;

                // see https://en.wikipedia.org/wiki/Trilinear_interpolation
                Vector3 vecPt00     = vecGetInter(vecPt000, vecPt100, fXRatio);
                Vector3 vecPt10     = vecGetInter(vecPt010, vecPt110, fXRatio);
                Vector3 vecPt01     = vecGetInter(vecPt001, vecPt101, fXRatio);
                Vector3 vecPt11     = vecGetInter(vecPt011, vecPt111, fXRatio);
                Vector3 vecPt0      = vecGetInter(vecPt00, vecPt10, fYRatio);
                Vector3 vecPt1      = vecGetInter(vecPt01, vecPt11, fYRatio);
                Vector3 vecCheckPt  = vecGetInter(vecPt0, vecPt1, fZRatio);

                Vector3 vecDir00    = vecGetInter(vecDir000, vecDir100, fXRatio);
                Vector3 vecDir10    = vecGetInter(vecDir010, vecDir110, fXRatio);
                Vector3 vecDir01    = vecGetInter(vecDir001, vecDir101, fXRatio);
                Vector3 vecDir11    = vecGetInter(vecDir011, vecDir111, fXRatio);
                Vector3 vecDir0     = vecGetInter(vecDir00, vecDir10, fYRatio);
                Vector3 vecDir1     = vecGetInter(vecDir01, vecDir11, fYRatio);
                Vector3 vecDir      = vecGetInter(vecDir0, vecDir1, fZRatio);
                return vecDir;
            }

            /// <summary>
            /// Interpolates linearly between min and max.
            /// </summary>
            Vector3 vecGetInter(
                Vector3 vecMin,
                Vector3 vecMax,
                float fRatio)
            {
                fRatio          = float.Clamp(fRatio, 0, 1);
                Vector3 vecPt   = vecMin + fRatio * (vecMax - vecMin);
                return vecPt;
            }
		}
    }
}