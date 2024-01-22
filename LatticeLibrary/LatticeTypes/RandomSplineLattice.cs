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
	    public class RandomSplineLattice : ILatticeType
        {
            protected float m_dX, m_dY, m_dZ;
            protected int m_nPasses;

            /// <summary>
            /// Custom lattice type that connects random corners of a cell.
            /// <param name="nPasses">How many passes the algorithm should
            /// perform. By default it runs one time, meaning each corner
            /// point is connected to one other point. For higher numbers
            /// each corner point is connected to nPasses other points
            /// </param>
            public RandomSplineLattice(int nPasses=1)
            {
                m_nPasses = nPasses;
            }

            public void AddCell(
                ref Lattice     oLattice,
                IUnitCell       xCell,
                IBeamThickness  xBeamThickness,
                uint            nSubSamples)
            {
                m_dX = xCell.oGetCellBounding().vecSize().X;
                m_dY = xCell.oGetCellBounding().vecSize().Y;
                m_dZ = xCell.oGetCellBounding().vecSize().Z;

                List<Vector3> aCornerPoints = xCell.aGetCornerPoints();

                for (int nPass = 0; nPass < m_nPasses; nPass++)
                {
                    for (int i = 0; i < aCornerPoints.Count; i++)
                    {
                        int j;
                        do
                        {
                            j = (int)(Uf.fGetRandomLinear(0, aCornerPoints.Count));
                        }
                        while (j == i);
                        // Make sure we don't connect the corner to itself

                        List<Vector3> aPoints = new List<Vector3>();
                        aPoints.Add(aCornerPoints[i]);
                        aPoints.Add(xCell.vecGetCellCentre() + vecGetNoise());
                        aPoints.Add(aCornerPoints[j]);

                        aPoints = SplineOperations.aGetNURBSpline(aPoints, 20);
                        AddBeam(ref oLattice, aPoints, xBeamThickness);
                    }
                }
            }

            protected Vector3 vecGetNoise()
            {
                Vector3 vecNoise = new Vector3(
                    Uf.fGetRandomLinear(-0.3f * m_dX, 0.3f * m_dX),
                    Uf.fGetRandomLinear(-0.3f * m_dY, 0.3f * m_dY),
                    Uf.fGetRandomLinear(-0.3f * m_dZ, 0.3f * m_dZ));
                return vecNoise;
            }

            protected void AddBeam(ref Lattice oLattice, List<Vector3> aPoints, IBeamThickness xBeamThickness)
            {
                for (int i = 1; i < aPoints.Count; i++)
                {
                    Vector3 vecPt11 = aPoints[i - 1];
                    Vector3 vecPt22 = aPoints[i];
                    float fRadius11 = 0.5f * xBeamThickness.fGetBeamThickness(vecPt11);
                    float fRadius22 = 0.5f * xBeamThickness.fGetBeamThickness(vecPt22);
                    oLattice.AddBeam(vecPt11, fRadius11, vecPt22, fRadius22);
                }
            }
        }
    }
}