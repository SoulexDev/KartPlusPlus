using OpenTK.Graphics.OpenGL4;
using static SDL3.SDL;

namespace KartPlusPlus.Renderer {
    internal class RenderPipeline {
        public static nint Window;
        public static nint GLContext;
        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

        public static int[] Lights = new int[0];

        private static List<IRenderer> renderers;

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

            renderers = new List<IRenderer>();
            return true;
        }
        public static void Render() {
            if (Camera.Main == null) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Main camera is null");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            //enable depth testing
            GL.Enable(EnableCap.DepthTest);

            //render light depth for shadows
            //foreach (var light in lights) {
            //    //light.transform.Rotate(Vector3.UnitZ, 240 * Time.deltaTime);
            //    light.transform.rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 50) *
            //        Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegToRad * 45);

            //    light.DrawShadows(renderers);
            //}

            //render scene
            //if (GUIManager.guiEnabled && GUIManager.sceneViewGui != null) {
            //    GUIManager.sceneViewGui.CorrectViewport();
            //    GUIManager.sceneViewGui.BindFBO();
            //}
            //else
            //    GL.Viewport(0, 0, Engine.screenWidth, Engine.screenHeight);

            //if (postEffects.Count > 0)
            //    postProcessBuffers.Item1.Bind();

            GL.Viewport(0, 0, ScreenWidth, ScreenHeight);

            GL.ClearColor(0.25f, 0.75f, 0.5f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Camera.Main.SetMatrices();
            foreach (IRenderer renderer in renderers) {
                renderer.Draw(Camera.Main);
            }

            //render particles
            //ParticleManager.DrawParticles();

            //render skybox
            //GL.Enable(EnableCap.DepthTest);
            //GL.Disable(EnableCap.CullFace);
            //GL.DepthFunc(DepthFunction.Lequal);

            //skyboxShader.Use();
            //skyboxShader.SetMatrix4("uProjection", Camera.main.projectionMatrix);
            //skyboxShader.SetMatrix4("uView", Camera.main.viewMatrix.ClearTranslation());
            //SkyboxMesh.mesh.Draw();

            //disable depth testing for rendering ui and post effects
            GL.Disable(EnableCap.DepthTest);

            //render post effects
            //if (postEffects.Count > 0) {
            //    postProcessBuffers.Item1.Unbind();
            //    PostProcess();
            //}

            //render ui
            //foreach (var canvas in canvases) {
            //    canvas.Draw();
            //}
            //if (GUIManager.guiEnabled && GUIManager.sceneViewGui != null) {
            //    GUIManager.sceneViewGui.UnbindFBO();
            //}

            //GUIManager.Render();

            SDL_GL_SwapWindow(Window);
        }
        public static void AddRenderer(IRenderer renderer) {
            if (!renderers.Contains(renderer))
                renderers.Add(renderer);
        }
        public static void RemoveRenderer(IRenderer renderer) {
            if (renderers.Contains(renderer))
                renderers.Remove(renderer);
        }
        public static void Dispose() {
            SDL_GL_UnloadLibrary();
        }
    }
}