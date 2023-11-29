//
// SPDX-License-Identifier: CC0-1.0
//
// This example code file is released to the public under Creative Commons CC0.
// See https://creativecommons.org/publicdomain/zero/1.0/legalcode
//
// To the extent possible under law, LEAP 71 has waived all copyright and
// related or neighboring rights to this PicoGK Example Code.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//


using PicoGK;


namespace Leap71
{
    using ShapeKernel;
    using LatticeLibrary;

    namespace LatticeLibraryExamples
    {
        partial class LatticeLibraryShowCase
        {
            public static void RegularTask()
            {
                //Step 1: define bounding object
                BaseSphere oSphere				= new BaseSphere(new LocalFrame(), 50);
			    Voxels voxBounding              = oSphere.voxConstruct();



                //Step 2: define class for ICellArray interface
                float fNoiseLevel               = 0.2f;
			    ICellArray xCellArray			= new RegularCellArray(voxBounding, 20, 20, 20, fNoiseLevel);
                //ICellArray xCellArray           = new RegularUnitCell(20, 20, 20, fNoiseLevel);



                //Step 3: define class for ILatticeType interface
                ILatticeType xLatticeType		= new BodyCentreLattice();
                //ILatticeType xLatticeType       = new OctahedronLattice();
                //ILatticeType xLatticeType       = new RandomSplineLattice();



                //Step 4: define class for IBeamThickness interface
                IBeamThickness xBeamThickness	= new CellBasedBeamThickness(1f, 4f);
                //IBeamThickness xBeamThickness   = new ConstantBeamThickness(2f);
                //IBeamThickness xBeamThickness   = new BoundaryBeamThickness(1f, 4f);
                //IBeamThickness xBeamThickness   = new GlobalFuncBeamThickness(1f, 4f);
                xBeamThickness.SetBoundingVoxels(voxBounding);



                //Step 5: generate final lattice geometry from three components
                uint nSubSample                 = 5;
                Voxels voxLattice               = voxGetFinalLatticeGeometry(
                                                        xCellArray,
                                                        xLatticeType,
                                                        xBeamThickness,
                                                        nSubSample);


                //Step 6: post-processing
                voxLattice                      = Sh.voxOverOffset(voxLattice, 1f, 0f);
                voxLattice						= Sh.voxIntersect(voxLattice, voxBounding);



                //Step 7: visualization
                ColorFloat clrColor = Cp.clrRandom();
                Sh.PreviewVoxels(voxLattice, clrColor);
                Sh.PreviewVoxels(voxBounding, clrColor, 0.5f);

                foreach (IUnitCell xCell in xCellArray.aGetUnitCells())
                {
                    xCell.PreviewUnitCell();
                }



                //Step 8: export
                //Sh.ExportVoxelsToSTLFile(voxLattice, Sh.strGetExportPath(Sh.EExport.STL, "MyFirstRegularLattice"));
            }

            /// <summary>
            /// Functions to combine the lattice workflow components into a final object
            /// </summary>
            public static Voxels voxGetFinalLatticeGeometry(
                ICellArray      xCellArray,
                ILatticeType    xLatticeType,
                IBeamThickness  xBeamThickness,
                uint            nSubSample = 2)
            {
                Lattice oLattice = new Lattice();
                foreach (IUnitCell xCell in xCellArray.aGetUnitCells())
                {
                    xBeamThickness.UpdateCell(xCell);
                    xLatticeType.AddCell(ref oLattice, xCell, xBeamThickness, nSubSample);
                }
                Voxels voxLattice = new Voxels(oLattice);
                return voxLattice;
            }
        }
    }
}