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
            protected List<List<List<Vector3>>> m_aDiscretePoints;
            protected List<List<List<Vector3>>> m_aDiscreteData;
            protected BBox3                     m_oBBox;
            protected uint                      m_nXSamples;
            protected uint                      m_nYSamples;
            protected uint                      m_nZSamples;
         

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

                m_aDiscretePoints   = new List<List<List<Vector3>>>();
                m_aDiscreteData     = new List<List<List<Vector3>>>();

                for (int iX = 0; iX < m_nXSamples; iX++)
                {
                    float fX = fXSize / (float)(m_nXSamples - 1) * iX + m_oBBox.vecMin.X;

                    List<List<Vector3>> aNewDataGrid = new List<List<Vector3>>();
                    List<List<Vector3>> aNewPointGrid = new List<List<Vector3>>();

                    for (int iY = 0; iY < m_nYSamples; iY++)
                    {
                        float fY = fYSize / (float)(m_nYSamples - 1) * iY + m_oBBox.vecMin.Y;

                        List<Vector3> aNewDataList = new List<Vector3>();
                        List<Vector3> aNewPointList = new List<Vector3>();

                        for (int iZ = 0; iZ < m_nZSamples; iZ++)
                        {
                            //rigid grid point
                            float fZ        = fZSize / (float)(m_nZSamples - 1) * iZ + m_oBBox.vecMin.Z;
                            Vector3 vecPt   = new Vector3(fX, fY, fZ);
                            aNewPointList.Add(vecPt);

                            //random data point
                            aNewDataList.Add(new Vector3(
                                Uf.fGetRandomLinear(fMinValue, fMaxValue),
                                Uf.fGetRandomLinear(fMinValue, fMaxValue),
                                Uf.fGetRandomLinear(fMinValue, fMaxValue)));
                        }

                        aNewDataGrid.Add(aNewDataList);
                        aNewPointGrid.Add(aNewPointList);
                    }
                    m_aDiscreteData.Add(aNewDataGrid);
                    m_aDiscretePoints.Add(aNewPointGrid);
                }
            }

            /// <summary>
            /// Returns a tri-linear interpolation from the discrete data field based on the point's position.
            /// https://en.wikipedia.org/wiki/Trilinear_interpolation
            /// </summary>
            public Vector3 vecGetData(Vector3 vecPt)
            {
                float dX    = m_oBBox.vecSize().X / (float)(m_nXSamples - 1);
                float dY    = m_oBBox.vecSize().Y / (float)(m_nYSamples - 1);
                float dZ    = m_oBBox.vecSize().Z / (float)(m_nZSamples - 1);
                int iLowerX = (int)((vecPt.X - m_oBBox.vecMin.X) / dX);
                int iLowerY = (int)((vecPt.Y - m_oBBox.vecMin.Y) / dY);
                int iLowerZ = (int)((vecPt.Z - m_oBBox.vecMin.Z) / dZ);
                int iUpperX = iLowerX + 1;
                int iUpperY = iLowerY + 1;
                int iUpperZ = iLowerZ + 1;

                //limit
                iLowerX = (int)Math.Min(m_nXSamples - 1, Math.Max(0, iLowerX));
                iUpperX = (int)Math.Min(m_nXSamples - 1, Math.Max(0, iUpperX));
                iLowerY = (int)Math.Min(m_nYSamples - 1, Math.Max(0, iLowerY));
                iUpperY = (int)Math.Min(m_nYSamples - 1, Math.Max(0, iUpperY));
                iLowerZ = (int)Math.Min(m_nZSamples - 1, Math.Max(0, iLowerZ));
                iUpperZ = (int)Math.Min(m_nZSamples - 1, Math.Max(0, iUpperZ));

                Vector3 vecPt_xyz = m_aDiscretePoints[iLowerX][iLowerY][iLowerZ];
                Vector3 vecPt_XYZ = m_aDiscretePoints[iUpperX][iUpperY][iUpperZ];

                float fLowerX = vecPt_xyz.X;
                float fUpperX = vecPt_XYZ.X;
                float fLowerY = vecPt_xyz.Y;
                float fUpperY = vecPt_XYZ.Y;
                float fLowerZ = vecPt_xyz.Z;
                float fUpperZ = vecPt_XYZ.Z;

                //interpolate
                float fXRatio = (vecPt.X - fLowerX) / (fUpperX - fLowerX);
                float fYRatio = (vecPt.Y - fLowerY) / (fUpperY - fLowerY);
                float fZRatio = (vecPt.Z - fLowerZ) / (fUpperZ - fLowerZ);

                Vector3 vecInterX_yz = vecGetLinearInterpolation(
                    m_aDiscreteData[iLowerX][iLowerY][iLowerZ],
                    m_aDiscreteData[iUpperX][iLowerY][iLowerZ],
                    fXRatio);

                Vector3 vecInterX_yZ = vecGetLinearInterpolation(
                    m_aDiscreteData[iLowerX][iLowerY][iUpperZ],
                    m_aDiscreteData[iUpperX][iLowerY][iUpperZ],
                    fXRatio);

                Vector3 vecInterX_Yz = vecGetLinearInterpolation(
                    m_aDiscreteData[iLowerX][iUpperY][iLowerZ],
                    m_aDiscreteData[iUpperX][iUpperY][iLowerZ],
                    fXRatio);

                Vector3 vecInterX_YZ = vecGetLinearInterpolation(
                    m_aDiscreteData[iLowerX][iUpperY][iUpperZ],
                    m_aDiscreteData[iUpperX][iUpperY][iUpperZ],
                    fXRatio);

                Vector3 vecInterY_z = vecGetLinearInterpolation(
                    vecInterX_yz,
                    vecInterX_Yz,
                    fYRatio);

                Vector3 vecInterY_Z = vecGetLinearInterpolation(
                    vecInterX_yZ,
                    vecInterX_YZ,
                    fYRatio);

                Vector3 vecInter = vecGetLinearInterpolation(
                    vecInterY_z,
                    vecInterY_Z,
                    fZRatio);

                return vecInter;
            }

            protected Vector3 vecGetLinearInterpolation(
                Vector3 vecMin,
                Vector3 vecMax,
                float fRatio)
            {
                Vector3 vecPt = vecMin + fRatio * (vecMax - vecMin);
                return vecPt;
            }
		}
    }
}