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
        partial class ImplicitLibraryShowCase
        {
            public static void LogicSplitTask()
            {
                //Step 1: define bounding object
                BaseBox oBox				    = new BaseBox(new LocalFrame(), 50, 50, 50);
			    Voxels voxBounding              = oBox.voxConstruct();



                //Step 2: define class for IImplicit interface
                float fUnitSize                 = 10f;
                float fWallThickness            = 1f;
                //IImplicit xImplicitPattern_01   = new ImplicitSplitWallGyroid(fUnitSize, fWallThickness, true);
                //IImplicit xImplicitPattern_02   = new ImplicitSplitWallGyroid(fUnitSize, fWallThickness, false);
                IImplicit xImplicitPattern_01   = new ImplicitSplitVoidGyroid(fUnitSize, fWallThickness, true);
                IImplicit xImplicitPattern_02   = new ImplicitSplitVoidGyroid(fUnitSize, fWallThickness, false);



                //Step 3: generate final implicit geometry
                Voxels voxImplicit_01           = Sh.voxIntersectImplicit(
                                                        voxBounding,
                                                        xImplicitPattern_01);
                Voxels voxImplicit_02           = Sh.voxIntersectImplicit(
                                                        voxBounding,
                                                        xImplicitPattern_02);


                //Step 4: visualization
                ColorFloat clrColor             = Cp.clrRandom();
                Sh.PreviewVoxels(voxImplicit_01, clrColor);
                Sh.PreviewVoxels(voxImplicit_02, Cp.clrRandom());
                Sh.PreviewVoxels(voxBounding, clrColor, 0.3f);



                ////Step 5: export
                //Sh.ExportVoxelsToSTLFile(voxImplicit_01, Sh.strGetExportPath(Sh.EExport.STL, "MyFirstLogicSplitGyroid_01"));
                //Sh.ExportVoxelsToSTLFile(voxImplicit_02, Sh.strGetExportPath(Sh.EExport.STL, "MyFirstLogicSplitGyroid_02"));


                Library.Log("Finished Task successfully.");
            }
        }
    }
}