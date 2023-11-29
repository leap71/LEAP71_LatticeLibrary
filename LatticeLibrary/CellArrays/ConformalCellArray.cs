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


namespace Leap71
{
    using ShapeKernel;

    namespace LatticeLibrary
    {
	    public class ConformalCellArray : ICellArray
	    {
            protected uint              m_nNumberInX;
            protected uint              m_nNumberInY;
            protected uint              m_nNumberInZ;
            protected List<IUnitCell>   m_aUnitCells;

            /// <summary>
            /// Regular grid cell array that is conformal to the specified BaseBox object.
            /// Number of cells in each dimension can be specified.
            /// </summary>
            public ConformalCellArray(
                BaseBox oBox,
                uint nNumberInX,
                uint nNumberInY,
                uint nNumberInZ)
		    {
                m_nNumberInX    = nNumberInX;
                m_nNumberInY    = nNumberInY;
                m_nNumberInZ    = nNumberInZ;
            
                m_aUnitCells = new List<IUnitCell>();
                for (uint nX = 1; nX <= nNumberInX; nX++)
                {
                    for (uint nY = 1; nY <= nNumberInY; nY++)
                    {
                        for (uint nZ = 1; nZ <= nNumberInZ; nZ++)
                        {
                            Vector3 vecCorner_01 = vecGetInternalBoxPt(oBox, nX - 1, nY - 1, nZ - 1);
                            Vector3 vecCorner_02 = vecGetInternalBoxPt(oBox, nX,     nY - 1, nZ - 1);
                            Vector3 vecCorner_03 = vecGetInternalBoxPt(oBox, nX,     nY,     nZ - 1);
                            Vector3 vecCorner_04 = vecGetInternalBoxPt(oBox, nX - 1, nY,     nZ - 1);
                            Vector3 vecCorner_05 = vecGetInternalBoxPt(oBox, nX - 1, nY - 1, nZ);
                            Vector3 vecCorner_06 = vecGetInternalBoxPt(oBox, nX,     nY - 1, nZ);
                            Vector3 vecCorner_07 = vecGetInternalBoxPt(oBox, nX,     nY,     nZ);
                            Vector3 vecCorner_08 = vecGetInternalBoxPt(oBox, nX - 1, nY,     nZ);

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

            public ConformalCellArray(
                BaseLens oLens,
                uint nNumberInX,
                uint nNumberInY,
                uint nNumberInZ)
		    {
                m_nNumberInX    = nNumberInX;
                m_nNumberInY    = nNumberInY;
                m_nNumberInZ    = nNumberInZ;
            
                m_aUnitCells = new List<IUnitCell>();
                for (uint nX = 1; nX <= nNumberInX; nX++)
                {
                    for (uint nY = 1; nY <= nNumberInY; nY++)
                    {
                        for (uint nZ = 1; nZ <= nNumberInZ; nZ++)
                        {
                            Vector3 vecCorner_01 = vecGetInternalLensPt(oLens, nX - 1, nY - 1, nZ - 1);
                            Vector3 vecCorner_02 = vecGetInternalLensPt(oLens, nX,     nY - 1, nZ - 1);
                            Vector3 vecCorner_03 = vecGetInternalLensPt(oLens, nX,     nY,     nZ - 1);
                            Vector3 vecCorner_04 = vecGetInternalLensPt(oLens, nX - 1, nY,     nZ - 1);
                            Vector3 vecCorner_05 = vecGetInternalLensPt(oLens, nX - 1, nY - 1, nZ);
                            Vector3 vecCorner_06 = vecGetInternalLensPt(oLens, nX,     nY - 1, nZ);
                            Vector3 vecCorner_07 = vecGetInternalLensPt(oLens, nX,     nY,     nZ);
                            Vector3 vecCorner_08 = vecGetInternalLensPt(oLens, nX - 1, nY,     nZ);

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

            public ConformalCellArray(
                BasePipeSegment oSegment,
                uint nNumberInX,
                uint nNumberInY,
                uint nNumberInZ)
		    {
                m_nNumberInX    = nNumberInX;
                m_nNumberInY    = nNumberInY;
                m_nNumberInZ    = nNumberInZ;
            
                m_aUnitCells = new List<IUnitCell>();
                for (uint nX = 1; nX <= nNumberInX; nX++)
                {
                    for (uint nY = 1; nY <= nNumberInY; nY++)
                    {
                        for (uint nZ = 1; nZ <= nNumberInZ; nZ++)
                        {
                            Vector3 vecCorner_01 = vecGetInternalSegmentPt(oSegment, nX - 1, nY - 1, nZ - 1);
                            Vector3 vecCorner_02 = vecGetInternalSegmentPt(oSegment, nX,     nY - 1, nZ - 1);
                            Vector3 vecCorner_03 = vecGetInternalSegmentPt(oSegment, nX,     nY,     nZ - 1);
                            Vector3 vecCorner_04 = vecGetInternalSegmentPt(oSegment, nX - 1, nY,     nZ - 1);
                            Vector3 vecCorner_05 = vecGetInternalSegmentPt(oSegment, nX - 1, nY - 1, nZ);
                            Vector3 vecCorner_06 = vecGetInternalSegmentPt(oSegment, nX,     nY - 1, nZ);
                            Vector3 vecCorner_07 = vecGetInternalSegmentPt(oSegment, nX,     nY,     nZ);
                            Vector3 vecCorner_08 = vecGetInternalSegmentPt(oSegment, nX - 1, nY,     nZ);

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

            protected Vector3 vecGetInternalBoxPt(BaseBox oBox, uint nX, uint nY, uint nZ)
            {
                float fLengthRatio  = (1f * (float)nZ / (float)m_nNumberInZ) - 0f;        //0 to 1
                float fWidthRatio   = (2f * (float)nX / (float)m_nNumberInX) - 1f;        //(-1) to (+1)
                float fDepthRatio   = (2f * (float)nY / (float)m_nNumberInY) - 1f;        //(-1) to (+1)

                Vector3 vecPt       = oBox.vecGetSurfacePoint(fWidthRatio, fDepthRatio, fLengthRatio);
                return vecPt;
            }

            protected Vector3 vecGetInternalLensPt(BaseLens oLens, uint nX, uint nY, uint nZ)
            {
                float fPhiRatio     = (1f * (float)nZ / (float)m_nNumberInZ ) - 0f;        //0 to 1
                float fHeightRatio  = (1f * (float)nX / (float)m_nNumberInX ) - 0f;        //0 to 1
                float fRadiusRatio  = (1f * (float)nY / (float)m_nNumberInY ) - 0f;        //0 to 1

                Vector3 vecPt       = oLens.vecGetSurfacePoint(fHeightRatio, fPhiRatio, fRadiusRatio);
                return vecPt;
            }

            protected Vector3 vecGetInternalSegmentPt(BasePipeSegment oSegment, uint nX, uint nY, uint nZ)
            {
                float fPhiRatio     = (1f * (float)nZ / (float)m_nNumberInZ ) - 0f;        //0 to 1
                float fLengthRatio  = (1f * (float)nX / (float)m_nNumberInX ) - 0f;        //0 to 1
                float fRadiusRatio  = (1f * (float)nY / (float)m_nNumberInY ) - 0f;        //0 to 1

                Vector3 vecPt       = oSegment.vecGetSurfacePoint(fLengthRatio, fPhiRatio, fRadiusRatio);
                return vecPt;
            }

            public List<IUnitCell> aGetUnitCells()
            {
                return m_aUnitCells;
            }
        }

        public class ConformalShowcaseShapes
        {
            public static BaseBox oGetBox_01()
            {
                BaseBox oBox = new BaseBox(new LocalFrame(), 100);
                oBox.SetDepth(new LineModulation(fGetDepth_01));
                oBox.SetWidth(new LineModulation(fGetWidth_01));
                return oBox;
            }

            protected static float fGetWidth_01(float fLengthRatio)
            {
                return 60 + 20 * MathF.Cos(5f * fLengthRatio);
            }

            protected static float fGetDepth_01(float fLengthRatio)
            {
                return 80 - 40 * MathF.Cos(3f * fLengthRatio);
            }

            public static BaseLens oGetLens_01()
            {
                BaseLens oLens = new BaseLens(new LocalFrame(), 1f, 20f, 40f);
                oLens.SetHeight(
                    new SurfaceModulation(fGetLowerLens_01),
                    new SurfaceModulation(fGetUpperLens_01));
                return oLens;
            }

            protected static float fGetLowerLens_01(float fPhi, float fRadiusRatio)
            {
                return (-20) + 20f * fRadiusRatio;
            }

            protected static float fGetUpperLens_01(float fPhi, float fRadiusRatio)
            {
                return 20 + 5 * MathF.Cos(2f * fRadiusRatio);
            }

            public static BasePipeSegment oGetSegment_01()
            {
                BasePipeSegment oLens = new BasePipeSegment(
                    new LocalFrame(), 100f, 20f, 40f,
                    new LineModulation(0f),
                    new LineModulation(fGetPhiRange_01),
                    BasePipeSegment.EMethod.MID_RANGE);
                oLens.SetRadius(
                    new SurfaceModulation(fGetInnerRadius_01),
                    new SurfaceModulation(fGetOuterRadius_01));
                return oLens;
            }

            protected static float fGetPhiRange_01(float fLengthRatio)
            {
                return MathF.PI - 0.45f * MathF.PI * MathF.Cos(3f * fLengthRatio);
            }

            protected static float fGetInnerRadius_01(float fPhi, float fLengthRatio)
            {
                return 20f + 10f * fLengthRatio;
            }

            protected static float fGetOuterRadius_01(float fPhi, float fLengthRatio)
            {
                return fGetInnerRadius_01(fPhi, fLengthRatio) + 15 + 5 * MathF.Cos(4f * fPhi);
            }
        }
    }
}