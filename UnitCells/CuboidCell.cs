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
        public interface IUnitCell
        {
            public List<Vector3>    aGetCornerPoints();
            public Vector3          vecGetCellCentre();
            public BBox3            oGetCellBounding();
            public void             PreviewUnitCell();
        }

        /// <summary>
        /// Simple unit cell that has 8 corner points and the shape of a cuboid.
        /// Suitable for a regular quad grid pattern.
        /// </summary>
	    public class CuboidCell : IUnitCell
        {
            protected List<Vector3> m_aCornerPoints;
            protected Vector3       m_vecCentre;
            protected BBox3         m_oBBox;

            /// <summary>
            /// Simple struct to hold the corner vertices of a cuboid
            /// unit cell with
            /// lower corners 1, 2, 3, 4 and
            /// upper corners 5, 6, 7, 8.
            /// </summary>
            public CuboidCell(
                in Vector3 vecCorner_01,
                in Vector3 vecCorner_02,
                in Vector3 vecCorner_03,
                in Vector3 vecCorner_04,
                in Vector3 vecCorner_05,
                in Vector3 vecCorner_06,
                in Vector3 vecCorner_07,
                in Vector3 vecCorner_08)
		    {
                m_aCornerPoints = new List<Vector3>()
                {
                    vecCorner_01,
                    vecCorner_02,
                    vecCorner_03,
                    vecCorner_04,
                    vecCorner_05,
                    vecCorner_06,
                    vecCorner_07,
                    vecCorner_08
                };

                float fMinX = float.MaxValue;
                float fMaxX = float.MinValue;
                float fMinY = float.MaxValue;
                float fMaxY = float.MinValue;
                float fMinZ = float.MaxValue;
                float fMaxZ = float.MinValue;

                m_vecCentre = new Vector3();
                foreach (Vector3 vecCorner in m_aCornerPoints)
                {
                    m_vecCentre += vecCorner;

                    if (vecCorner.X > fMaxX)
                    {
                        fMaxX = vecCorner.X;
                    }
                    if (vecCorner.Y > fMaxY)
                    {
                        fMaxY = vecCorner.Y;
                    }
                    if (vecCorner.Z > fMaxZ)
                    {
                        fMaxZ = vecCorner.Z;
                    }

                    if (vecCorner.X < fMinX)
                    {
                        fMinX = vecCorner.X;
                    }
                    if (vecCorner.Y < fMinY)
                    {
                        fMinY = vecCorner.Y;
                    }
                    if (vecCorner.Z < fMinZ)
                    {
                        fMinZ = vecCorner.Z;
                    }
                }
                m_vecCentre /= m_aCornerPoints.Count;
                m_oBBox = new BBox3(new Vector3(fMinX, fMinY, fMinZ), new Vector3(fMaxX, fMaxY, fMaxZ));
            }

            public Vector3 vecGetCellCentre()
            {
                return m_vecCentre;
            }

            public BBox3 oGetCellBounding()
            {
                return m_oBBox;
            }

            public List<Vector3> aGetCornerPoints()
            {
                return m_aCornerPoints;
            }

            public void PreviewUnitCell()
            {
                //wireframe
                Sh.PreviewLine(new List<Vector3>() { m_aCornerPoints[0], m_aCornerPoints[1], m_aCornerPoints[2], m_aCornerPoints[3], m_aCornerPoints[0] }, Cp.clrBlack);
                Sh.PreviewLine(new List<Vector3>() { m_aCornerPoints[4], m_aCornerPoints[5], m_aCornerPoints[6], m_aCornerPoints[7], m_aCornerPoints[4] }, Cp.clrBlack);
                Sh.PreviewLine(new List<Vector3>() { m_aCornerPoints[0], m_aCornerPoints[4] }, Cp.clrBlack);
                Sh.PreviewLine(new List<Vector3>() { m_aCornerPoints[1], m_aCornerPoints[5] }, Cp.clrBlack);
                Sh.PreviewLine(new List<Vector3>() { m_aCornerPoints[2], m_aCornerPoints[6] }, Cp.clrBlack);
                Sh.PreviewLine(new List<Vector3>() { m_aCornerPoints[3], m_aCornerPoints[7] }, Cp.clrBlack);

                ////corners
                //Sh.PreviewPointCloud(aGetCornerPoints(), 0.1f, Cp.clrRed);

                ////faces
                //ColorFloat clrColor = Cp.clrRandom();
                //Sh.PreviewMesh(MeshUtility.mshFromQuad(m_aCornerPoints[0], m_aCornerPoints[1], m_aCornerPoints[2], m_aCornerPoints[3]), clrColor, 0.3f);
                //Sh.PreviewMesh(MeshUtility.mshFromQuad(m_aCornerPoints[4], m_aCornerPoints[5], m_aCornerPoints[6], m_aCornerPoints[7]), clrColor, 0.3f);
                //Sh.PreviewMesh(MeshUtility.mshFromQuad(m_aCornerPoints[0], m_aCornerPoints[1], m_aCornerPoints[5], m_aCornerPoints[4]), clrColor, 0.3f);
                //Sh.PreviewMesh(MeshUtility.mshFromQuad(m_aCornerPoints[1], m_aCornerPoints[2], m_aCornerPoints[6], m_aCornerPoints[5]), clrColor, 0.3f);
                //Sh.PreviewMesh(MeshUtility.mshFromQuad(m_aCornerPoints[2], m_aCornerPoints[3], m_aCornerPoints[7], m_aCornerPoints[6]), clrColor, 0.3f);
                //Sh.PreviewMesh(MeshUtility.mshFromQuad(m_aCornerPoints[3], m_aCornerPoints[0], m_aCornerPoints[4], m_aCornerPoints[7]), clrColor, 0.3f);
            }
        }
    }
}