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


namespace Leap71
{
    using ShapeKernel;

    namespace LatticeLibrary
	{
        public interface IRawTPMSPattern
        {
            public float fGetSignedDistance(float fX, float fY, float fZ);
        }

        public class RawGyroidTPMSPattern : IRawTPMSPattern
        {
            protected const float m_fFrequencyScale = (2f * MathF.PI);

            public RawGyroidTPMSPattern() { }

            public float fGetSignedDistance(float fX, float fY, float fZ)
            {
                float fDist =    MathF.Sin(m_fFrequencyScale * fX) * MathF.Cos(m_fFrequencyScale * fY) +
                                 MathF.Sin(m_fFrequencyScale * fY) * MathF.Cos(m_fFrequencyScale * fZ) +
                                 MathF.Sin(m_fFrequencyScale * fZ) * MathF.Cos(m_fFrequencyScale * fX);
                return fDist;
            }
        }

        public class RawLidinoidTPMSPattern : IRawTPMSPattern
        {
            protected const float m_fFrequencyScale = 0.5f * (2f * MathF.PI);

            public RawLidinoidTPMSPattern() { }

            public float fGetSignedDistance(float fX, float fY, float fZ)
            {
                float fDist =   +0.5f * (MathF.Sin(2 * m_fFrequencyScale * fX) * MathF.Cos(m_fFrequencyScale * fY) * MathF.Sin(m_fFrequencyScale * fZ) +
                                         MathF.Sin(2 * m_fFrequencyScale * fY) * MathF.Cos(m_fFrequencyScale * fZ) * MathF.Sin(m_fFrequencyScale * fX) +
                                         MathF.Sin(2 * m_fFrequencyScale * fZ) * MathF.Cos(m_fFrequencyScale * fX) * MathF.Sin(m_fFrequencyScale * fY))
                                                                                                                         
                               - 0.5f * (MathF.Cos(2 * m_fFrequencyScale * fX) * MathF.Cos(2 * m_fFrequencyScale * fY) +
                                         MathF.Cos(2 * m_fFrequencyScale * fY) * MathF.Cos(2 * m_fFrequencyScale * fZ) +
                                         MathF.Cos(2 * m_fFrequencyScale * fZ) * MathF.Cos(2 * m_fFrequencyScale * fX));
                return fDist;
            }
        }

        public class RawSchwarzPrimitiveTPMSPattern : IRawTPMSPattern
        {
            protected const float m_fFrequencyScale = (2f * MathF.PI);

            public RawSchwarzPrimitiveTPMSPattern() { }

            public float fGetSignedDistance(float fX, float fY, float fZ)
            {
                float fDist =  (MathF.Cos(m_fFrequencyScale * fX) +
                                MathF.Cos(m_fFrequencyScale * fY) +
                                MathF.Cos(m_fFrequencyScale * fZ));
                return fDist;
            }
        }

        public class RawSchwarzDiamondTPMSPattern : IRawTPMSPattern
        {
            protected const float m_fFrequencyScale = 0.5f * (2f * MathF.PI);

            public RawSchwarzDiamondTPMSPattern() { }

            public float fGetSignedDistance(float fX, float fY, float fZ)
            {
                float fDist =  +(MathF.Cos(m_fFrequencyScale * fX) *
                                 MathF.Cos(m_fFrequencyScale * fY) *
                                 MathF.Cos(m_fFrequencyScale * fZ))
                              - (MathF.Sin(m_fFrequencyScale * fX) *
                                 MathF.Sin(m_fFrequencyScale * fY) *
                                 MathF.Sin(m_fFrequencyScale * fZ));
                return fDist;
            }
        }

        public class RawTransitionTPMSPattern : IRawTPMSPattern
        {
            protected IRawTPMSPattern   m_xTPMS_01;
            protected IRawTPMSPattern   m_xTPMS_02;

            public RawTransitionTPMSPattern()
            {
                m_xTPMS_01      = new RawSchwarzDiamondTPMSPattern();
                m_xTPMS_02      = new RawSchwarzPrimitiveTPMSPattern();
            }

            public float fGetSignedDistance(float fX, float fY, float fZ)
            {
                float fDist_01  = m_xTPMS_01.fGetSignedDistance(fX, fY, fZ);
                float fDist_02  = m_xTPMS_02.fGetSignedDistance(fX, fY, fZ);
                float fRatio    = Uf.fLimitValue((float)(fX + 2f) / 5f, 0f, 1f);
                float fDist     = Uf.fTransFixed(
                                        fDist_01,
                                        fDist_02,
                                        fRatio);
                return fDist;
            }
        }
    }
}