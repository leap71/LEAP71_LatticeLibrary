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
        public class ImplicitModular : IImplicit
		{
			protected IBeamThickness	    m_xWallThickness;
            protected IRawTPMSPattern       m_xRawTPMSPattern;
            protected ICoordinateTrafo      m_xCoordinateTrafo;
            protected ISplittingLogic       m_xSplittingLogic;
            protected float                 m_fFrequencyScale;

            /// <summary>
            /// Helper class for an advanced implicit pattern.
            /// </summary>
            public ImplicitModular(
                IRawTPMSPattern     xRawTPMSPattern,
                IBeamThickness      xWallThickness,
                ICoordinateTrafo    xCoordinateTrafo,
                ISplittingLogic     xSplittingLogic
                )
			{
                m_xWallThickness    = xWallThickness;
                m_xCoordinateTrafo  = xCoordinateTrafo;
                m_xRawTPMSPattern   = xRawTPMSPattern;
                m_xSplittingLogic   = xSplittingLogic;
            }

			public float fSignedDistance(in Vector3 vecPt)
			{
                m_xCoordinateTrafo.Apply(
                    out float fX,
                    out float fY,
                    out float fZ,
                    vecPt);

                float fRawSignedDistance    = m_xRawTPMSPattern.fGetSignedDistance(fX, fY, fZ);

                //wall thickness in carthesian coordinates
                float fWallThickness        = m_xWallThickness.fGetBeamThickness(vecPt);

                float fFinalSignedDistance  = m_xSplittingLogic.fGetAdvancedSignedDistance(fRawSignedDistance, fWallThickness);
                return fFinalSignedDistance;
            }
		}
    }
}