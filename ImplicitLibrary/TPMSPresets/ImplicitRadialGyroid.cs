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
        public class ImplicitRadialGyroid : IImplicit
		{
            protected float         m_fFrequencyScale;
			protected float		    m_fWallThickness;
            protected uint          m_nSamplesPerRound;

            /// <summary>
            /// Helper class for an implicit gyroid pattern.
            /// </summary>
            public ImplicitRadialGyroid(uint nUnitsPerRound, float fUnitSizeInZ, float fWallThickness)
			{
                m_fFrequencyScale    = (2f * MathF.PI) / fUnitSizeInZ;
                m_fWallThickness     = fWallThickness;
                m_nSamplesPerRound   = nUnitsPerRound;
            }

			public float fSignedDistance(in Vector3 vecPt)
			{
                //map carthesian point to cylindrical coordinates
                float fRadius       = VecOperations.fGetRadius(vecPt);
                float dPhi          = (2f * MathF.PI) / m_nSamplesPerRound;
                float fPhi          = VecOperations.fGetPhi(vecPt) + MathF.PI;
                float fPhiIntervals = fPhi / dPhi;
                float fUnitSize     = (2f * MathF.PI) / m_fFrequencyScale;
                double dX           = (double)fRadius;
                double dY           = (double)(fPhiIntervals * fUnitSize);
                double dZ           = vecPt.Z;

                //calculate the gyroid surface equation
                double dDist =   Math.Sin(m_fFrequencyScale * dX) * Math.Cos(m_fFrequencyScale * dY) +
                                 Math.Sin(m_fFrequencyScale * dY) * Math.Cos(m_fFrequencyScale * dZ) +
                                 Math.Sin(m_fFrequencyScale * dZ) * Math.Cos(m_fFrequencyScale * dX);

                //apply thickness to the gyroid surface
                return (float)(Math.Abs(dDist) - 0.5f * m_fWallThickness);
            }
		}
    }
}