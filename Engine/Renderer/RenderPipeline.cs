using OpenTK.Graphics.OpenGL4;
using static SDL3.SDL;
namespace KartPlusPlus.Renderer {
    internal class RenderPipeline {
        public static nint Window;
        public static nint GLContext;
        public static int ScreenWidth = 720;
        public static int ScreenHeight = 480;

        public static bool Init() {
            if (!SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO | SDL_InitFlags.SDL_INIT_AUDIO)) {
                return false;
            }

            SDL_GL_LoadLibrary("");

            Window = SDL_CreateWindow("Kart++", ScreenWidth, ScreenHeight, SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MAJOR_VERSION, 4);
            SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MINOR_VERSION, 6);
            SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_CORE);

            GLContext = SDL_GL_CreateContext(Window);
            GL.LoadBindings(new SDLBindingContext());

            if (!SDL_GL_SetSwapInterval(1)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not set v-sync.");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            return true;
        }
        public static void Render() {
            SDL_GL_SwapWindow(Window);
        }
        public static void Dispose() {
            SDL_GL_UnloadLibrary();
        }
    }
}