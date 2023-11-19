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
        /// <summary>
        /// Interface that returns a beam thickness for a given point in space.
        /// </summary>
        public interface IBeamThickness
        {
            public float fGetBeamThickness(Vector3 vecPt);
            public void  UpdateCell(IUnitCell xCell);
            public void  SetBoundingVoxels(Voxels voxBounding);
        }

        /// <summary>
        /// Class to describe a constant beam thickness of a given point
        /// independant of unit cell, boundary voxelfield etc...
        /// </summary>
        public class ConstantBeamThickness : IBeamThickness
        {
            protected float m_fBeamThickness;

            public ConstantBeamThickness(float fBeamThickness)
            {
                m_fBeamThickness = fBeamThickness;
            }

            public float fGetBeamThickness(Vector3 vecPt)
            {
                return m_fBeamThickness;
            }

            public void UpdateCell(IUnitCell xCell)
            {

            }

            public void SetBoundingVoxels(Voxels voxBounding)
            {

            }
        }
    }
}