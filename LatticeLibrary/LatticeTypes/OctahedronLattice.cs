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
    namespace LatticeLibrary
    {
	    public class OctahedronLattice : ILatticeType
        {
            /// <summary>
            /// Octahedron lattice type that connects adjacent face centres of a unit cell.
            /// </summary>
            public OctahedronLattice() { }

            public void AddCell(
                ref Lattice     oLattice,
                IUnitCell       xCell,
                IBeamThickness  xBeamThickness,
                uint            nSubSamples = 2)
            {
                List<Vector3> aCornerPoints = xCell.aGetCornerPoints();

                if (aCornerPoints.Count != 8)
                {
                    throw new Exception("Octahedron Lattice only supports Unit Cells with 8 Corners.");
                }

                Vector3 vecPt0 = aCornerPoints[0];
                Vector3 vecPt1 = aCornerPoints[1];
                Vector3 vecPt2 = aCornerPoints[2];
                Vector3 vecPt3 = aCornerPoints[3];
                Vector3 vecPt4 = aCornerPoints[4];
                Vector3 vecPt5 = aCornerPoints[5];
                Vector3 vecPt6 = aCornerPoints[6];
                Vector3 vecPt7 = aCornerPoints[7];

                Vector3 vecLowerFace    = 0.25f * (vecPt0 + vecPt1 + vecPt2 + vecPt3);
                Vector3 vecUpperFace    = 0.25f * (vecPt4 + vecPt5 + vecPt6 + vecPt7);
                Vector3 vecForwardFace  = 0.25f * (vecPt4 + vecPt5 + vecPt0 + vecPt1);
                Vector3 vecRightFace    = 0.25f * (vecPt6 + vecPt5 + vecPt2 + vecPt1);
                Vector3 vecBackwardFace = 0.25f * (vecPt6 + vecPt7 + vecPt2 + vecPt3);
                Vector3 vecLeftFace     = 0.25f * (vecPt4 + vecPt7 + vecPt0 + vecPt3);

                //beam connecting logic
                AddBeam(ref oLattice, vecLowerFace, vecRightFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecLowerFace, vecLeftFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecLowerFace, vecForwardFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecLowerFace, vecBackwardFace, xBeamThickness, nSubSamples);

                AddBeam(ref oLattice, vecUpperFace, vecRightFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecUpperFace, vecLeftFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecUpperFace, vecForwardFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecUpperFace, vecBackwardFace, xBeamThickness, nSubSamples);

                AddBeam(ref oLattice, vecForwardFace, vecRightFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecForwardFace, vecLeftFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecBackwardFace, vecRightFace, xBeamThickness, nSubSamples);
                AddBeam(ref oLattice, vecBackwardFace, vecLeftFace, xBeamThickness, nSubSamples);
            }

            protected void AddBeam(ref Lattice oLattice, Vector3 vecPt1, Vector3 vecPt2, IBeamThickness xBeamThickness, uint nSamples = 2)
            {
                if (nSamples == 2)
                {
                    float fRadius1 = 0.5f * xBeamThickness.fGetBeamThickness(vecPt1);
                    float fRadius2 = 0.5f * xBeamThickness.fGetBeamThickness(vecPt2);
                    oLattice.AddBeam(vecPt1, fRadius1, vecPt2, fRadius2);
                }
                else
                {
                    for (int i = 1; i < nSamples; i++)
                    {
                        Vector3 vecPt11 = vecPt1 + (float)(i - 1) / (float)(nSamples - 1) * (vecPt2 - vecPt1);
                        Vector3 vecPt22 = vecPt1 + (float)(i) / (float)(nSamples - 1) * (vecPt2 - vecPt1);
                        float fRadius11 = 0.5f * xBeamThickness.fGetBeamThickness(vecPt11);
                        float fRadius22 = 0.5f * xBeamThickness.fGetBeamThickness(vecPt22);
                        oLattice.AddBeam(vecPt11, fRadius11, vecPt22, fRadius22);
                    }
                }
            }
        }
    }
}