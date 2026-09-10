using KartPlusPlus.Renderer;
using static SDL3.SDL;

namespace KartPlusPlus {
    public class KartPlusPlus {
        //needed to access assets
        public static string AssetsPath;
        public static string RawPath;

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
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        private static SDL_AppResult SDL_AppIterate(nint appState) {
            RenderPipeline.Render();
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        private unsafe static SDL_AppResult SDL_AppEvent(nint appstate, SDL_Event* sdlEvent) {
            return SDL_AppResult.SDL_APP_CONTINUE;
        }
        private static void SDL_AppQuit(nint appState, SDL_AppResult result) {
            RenderPipeline.Dispose();
            SDL_Quit();
        }
    }
}