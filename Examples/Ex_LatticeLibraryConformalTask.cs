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
            public static void ConformalTask()
            {
                //Step 1: define base shape to conform to
                BaseBox oShape                  = ConformalShowcaseShapes.oGetBox_01();
                //BaseLens oShape                 = ConformalShowcaseShapes.oGetLens_01();
                //BasePipeSegment oShape          = ConformalShowcaseShapes.oGetSegment_01();
                Voxels voxBounding              = oShape.voxConstruct();



                //Step 2: define class for ICellArray interface
                ICellArray xCellArray           = new ConformalCellArray(oShape, 6, 8, 15);



                //Step 3: define class for ILatticeType interface
                ILatticeType xLatticeType		= new BodyCentreLattice();
                //ILatticeType xLatticeType       = new OctahedronLattice();
                //ILatticeType xLatticeType       = new RandomSplineLattice();



                //Step 4: define class for IBeamThickness interface
                IBeamThickness xBeamThickness = new CellBasedBeamThickness(2f, 0.1f);
                //IBeamThickness xBeamThickness   = new ConstantBeamThickness(2f);
                //IBeamThickness xBeamThickness   = new BoundaryBeamThickness(1f, 4f);
                //IBeamThickness xBeamThickness   = new GlobalFuncBeamThickness(1f, 4f);
                xBeamThickness.SetBoundingVoxels(voxBounding);



                //Step 5: generate final lattice geometry from three components
                uint nSubSample             = 5;
                Voxels voxLattice           = voxGetFinalLatticeGeometry(
                                                        xCellArray,
                                                        xLatticeType,
                                                        xBeamThickness,
                                                        nSubSample);


                //Step 6: post-processing
                voxLattice                  = Sh.voxOverOffset(voxLattice, 0.5f, 0f);
                voxLattice                  = Sh.voxIntersect(voxLattice, voxBounding);



                //Step 7: visualization
                ColorFloat clrColor = Cp.clrRandom();
                Sh.PreviewVoxels(voxLattice, clrColor);
                Sh.PreviewVoxels(voxBounding, clrColor, 0.1f);

                foreach (IUnitCell xCell in xCellArray.aGetUnitCells())
                {
                    xCell.PreviewUnitCell();
                }



                //Step 8: export
                //Sh.ExportVoxelsToSTLFile(voxLattice, Sh.strGetExportPath(Sh.EExport.STL, "MyFirstConformalLattice"));
            }
        }
    }
}