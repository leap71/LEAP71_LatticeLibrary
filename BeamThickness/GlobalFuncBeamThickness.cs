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
        /// <summary>
        /// Class to describe the beam thickness of a given point
        /// with respect to a global function.
        /// </summary>
        public class GlobalFuncBeamThickness : IBeamThickness
        {
            protected float     m_fMinBeamThickness;
            protected float     m_fMaxBeamThickness;


            public GlobalFuncBeamThickness(
                float fMinBeamThickness,
                float fMaxBeamThickness)
            {
                m_fMinBeamThickness = fMinBeamThickness;
                m_fMaxBeamThickness = fMaxBeamThickness;
            }

            public void UpdateCell(IUnitCell xCell)
            {
           
            }

            public float fGetBeamThickness(Vector3 vecPt)
            {
                float fRatio            = Uf.fLimitValue(0.02f * vecPt.X, 0f, 1f);
                float fBeamThickness    = Uf.fTransFixed(m_fMinBeamThickness, m_fMaxBeamThickness, fRatio);
                return fBeamThickness;
            }

            public void SetBoundingVoxels(Voxels voxBounding)
            {

            }
        }
    }
}