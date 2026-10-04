using KartPlusPlus.Engine;
using KartPlusPlus.Renderer;
using KartPlusPlus.Physics;
using KartPlusPlus.UserInput;
using KartPlusPlus.Game;
using KartPlusPlus.AssetManagement;
using KartPlusPlus.Utility;

using static SDL3.SDL;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace KartPlusPlus {
    public class KartPlusPlus {
        //needed to access assets
        public static string AssetsPath;
        public static string RawPath;

        public static List<EngineObject> EngineObjects = new List<EngineObject>();

        //stuff for physics updates
        private static Stopwatch fixedStepTimer;
        private static long timerOffset;

        //Initialize the program
        public static void Main(string[] args) {
            KartPlusPlus program = new KartPlusPlus();
            program.Init();
        }
        public int Init() {
            AssetsPath = AppDomain.CurrentDomain.BaseDirectory;
            RawPath = AssetsPath;
            AssetsPath = Path.Combine(AssetsPath, "assets");
            Console.WriteLine(AssetsPath);
            return SDL_RunApp(0, 0, SDL_Main, 0);
        }
        unsafe int SDL_Main(int argc, nint argv) {
            return SDL_EnterAppMainCallbacks(argc, argv, SDL_AppInit, SDL_AppIterate, SDL_AppEvent, SDL_AppQuit);
        }
        private static SDL_AppResult SDL_AppInit(nint appState, int argc, nint argv) {
            //initialize core modules
            if (!RenderPipeline.Init()) {
                return SDL_AppResult.SDL_APP_FAILURE;
            }
            Input.Init();
            PhysicsSim.Init();
            DefaultResources.Load();

            SDL_SetWindowRelativeMouseMode(RenderPipeline.Window, true);
            //SDL_HideCursor();

            fixedStepTimer = new Stopwatch();
            fixedStepTimer.Start();

            TestTrack.Create();

            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        private static SDL_AppResult SDL_AppIterate(nint appState) {
            Time.Update();
            Input.GrabState();

            foreach (var obj in EngineObjects) {
                obj.components.ForEach(c => c.Update());
            }
            foreach (var obj in EngineObjects) {
                obj.components.ForEach(c => c.LateUpdate());
            }

            if (fixedStepTimer.ElapsedMilliseconds + timerOffset > PhysicsSim.MillisecondsPerTick) {
                timerOffset = fixedStepTimer.ElapsedMilliseconds + timerOffset - PhysicsSim.MillisecondsPerTick;
                fixedStepTimer.Restart();

                Time.FixedUpdate();

                foreach (var obj in EngineObjects) {
                    obj.components.ForEach(c => c.PhysicsTick());
                }

                PhysicsSim.Tick();
            }

            RenderPipeline.Render();

            Input.StoreState();
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        private unsafe static SDL_AppResult SDL_AppEvent(nint appstate, SDL_Event* sdlEvent) {
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        private static void SDL_AppQuit(nint appState, SDL_AppResult result) {
            RenderPipeline.Dispose();
            PhysicsSim.Dispose();
            SDL_Quit();
        }
    }
}