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
using Leap71.ShapeKernel;

namespace Leap71
{
    namespace LatticeLibrary
	{
        public interface ICoordinateTrafo
        {
            public void Apply(out float fX, out float fY, out float fZ, Vector3 vecPt);
        }

        public class ScaleTrafo : ICoordinateTrafo
        {
            protected float m_fUnitX;
            protected float m_fUnitY;
            protected float m_fUnitZ;

            public ScaleTrafo(float fUnitX, float fUnitY, float fUnitZ)
            {
                m_fUnitX = fUnitX;
                m_fUnitY = fUnitY;
                m_fUnitZ = fUnitZ;
            }

            public void Apply(out float fX, out float fY, out float fZ, Vector3 vecPt)
            {
                fX = vecPt.X / m_fUnitX;
                fY = vecPt.Y / m_fUnitY;
                fZ = vecPt.Z / m_fUnitZ;
            }
        }

        public class FunctionalScaleTrafo : ICoordinateTrafo
        {
            public FunctionalScaleTrafo() { }

            public void Apply(out float fX, out float fY, out float fZ, Vector3 vecPt)
            {
                float fRatio = Uf.fLimitValue(vecPt.Z / 50f, 0f, 1f);
                fX = vecPt.X / (Uf.fTransFixed(20, 5, fRatio));
                fY = vecPt.Y / (Uf.fTransFixed(20, 5, fRatio));
                fZ = 10;
            }
        }

        public class RadialTrafo : ICoordinateTrafo
        {
            protected uint m_nSamplesPerRound;
            protected float m_dPhiPerZ;

            public RadialTrafo(uint nSamplesPerRound, float dPhiPerZ)
            {
                m_nSamplesPerRound  = nSamplesPerRound;
                m_dPhiPerZ          = dPhiPerZ;
            }

            public void Apply(out float fX, out float fY, out float fZ, Vector3 vecPt)
            {
                fZ              = vecPt.Z;
                float fRadius   = VecOperations.fGetRadius(vecPt);
                float fPhi      = VecOperations.fGetPhi(vecPt) + m_dPhiPerZ * fZ;
                fX              = fRadius;
                fY              = (m_nSamplesPerRound * fPhi);
            }
        }

        public class CombinedTrafo : ICoordinateTrafo
        {
            protected List<ICoordinateTrafo> m_aTrafos;

            public CombinedTrafo(List<ICoordinateTrafo> aTrafos)
            {
                m_aTrafos = aTrafos;
            }

            public void Apply(out float fX, out float fY, out float fZ, Vector3 vecPt)
            {
                fX = vecPt.X;
                fY = vecPt.Y;
                fZ = vecPt.Z;

                foreach (ICoordinateTrafo xTrafo in m_aTrafos)
                {
                    xTrafo.Apply(out fX, out fY, out fZ, vecPt);
                    vecPt = new Vector3(fX, fY, fZ);
                }
            }
        }
    }
}