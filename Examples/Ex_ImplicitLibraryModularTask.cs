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
            public static void ModularTask()
            {
                //Step 1: define bounding object
                BasePipe oPipe                      = new BasePipe(new LocalFrame(), 50, 20, 50);
                Voxels voxBounding                  = oPipe.voxConstruct();



                //Step 2: define class for IRawTPMSPattern interface
                //IRawTPMSPattern xRawTPMSPattern      = new RawGyroidTPMSPattern();
                //IRawTPMSPattern xRawTPMSPattern      = new RawLidinoidTPMSPattern();
                //IRawTPMSPattern xRawTPMSPattern      = new RawSchwarzPrimitiveTPMSPattern();
                //IRawTPMSPattern xRawTPMSPattern      = new RawSchwarzDiamondTPMSPattern();
                RawTransitionTPMSPattern xRawTPMSPattern     = new RawTransitionTPMSPattern();



                //Step 3: define class for ICoordinateTrafo interface
                ICoordinateTrafo xCoordinateTrafo   = new ScaleTrafo(10f, 10f, 10f);
                //ICoordinateTrafo xCoordinateTrafo     = new RadialTrafo(40, 0.001f);
                //ICoordinateTrafo xCoordinateTrafo   = new FunctionalScaleTrafo();
                //ICoordinateTrafo xCoordinateTrafo   = new CombinedTrafo(new List<ICoordinateTrafo>()
                //                                                    {
                //                                                        new ScaleTrafo(20f, 20f, 10f),
                //                                                        new RadialTrafo(2, 0f)
                //                                                    });


                //Step 4: define class for ISplittingLogic interface
                ISplittingLogic xSplittingLogic     = new FullWallLogic();
                //ISplittingLogic xSplittingLogic     = new FullVoidLogic();
                //ISplittingLogic xSplittingLogic     = new PositiveHalfWallLogic();
                //ISplittingLogic xSplittingLogic     = new NegativeHalfWallLogic();
                //ISplittingLogic xSplittingLogic     = new PositiveVoidLogic();
                //ISplittingLogic xSplittingLogic     = new NegativeVoidLogic();



                //Step 5: define class for IBeamThickness interface
                IBeamThickness xWallThickness       = new ConstantBeamThickness(0.5f);
                //IBeamThickness xWallThickness       = new GlobalFuncBeamThickness(0.5f, 2f);



                //Step 6: define class for IImplicit interface
                IImplicit xImplicitPattern          = new ImplicitModular(
                                                                    xRawTPMSPattern,
                                                                    xWallThickness,
                                                                    xCoordinateTrafo,
                                                                    xSplittingLogic);


                //Step 7: generate final implicit geometry
                Voxels voxImplicit                  = Sh.voxIntersectImplicit(
                                                        voxBounding,
                                                        xImplicitPattern);


                //Step 4: visualization
                ColorFloat clrColor = Cp.clrRandom();
                Sh.PreviewVoxels(voxImplicit, clrColor);
                Sh.PreviewVoxels(voxBounding, clrColor, 0.3f);



                //Step 5: export
                //Sh.ExportVoxelsToSTLFile(voxImplicit, Sh.strGetExportPath(Sh.EExport.STL, "MyFirstModularImplicit"));

                Library.Log("Finished Task successfully.");
            }
        }
    }
}