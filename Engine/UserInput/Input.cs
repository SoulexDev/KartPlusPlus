using KartPlusPlus.Renderer;
using OpenTK.Mathematics;
using static SDL3.SDL;

namespace KartPlusPlus.UserInput {
    public enum DeviceType { KeyboardMouse, GamePad }
    public class Input {
        private static DeviceType deviceType = DeviceType.KeyboardMouse;

        private static SDLBool[] currentKeyState;
        private static SDLBool[] lastKeyState;
        public static void Init() {
            SDL_CaptureMouse(true);
        }
        public static void GrabState() {
            currentKeyState = SDL_GetKeyboardState().ToArray();
        }
        public static void StoreState() {
            lastKeyState = currentKeyState;
        }
        public static float GetHorizontal() {
            switch (deviceType) {
                case DeviceType.KeyboardMouse:
                    float dir = 0;
                    if (currentKeyState[(int)SDL_Scancode.SDL_SCANCODE_A])
                        dir += 1;
                    if (currentKeyState[(int)SDL_Scancode.SDL_SCANCODE_D])
                        dir -= 1;

                    return dir;
                case DeviceType.GamePad:
                    return 0;
                default:
                    return 0;
            }
        }
        public static float GetVertical() {
            switch (deviceType) {
                case DeviceType.KeyboardMouse:
                    float dir = 0;
                    if (currentKeyState[(int)SDL_Scancode.SDL_SCANCODE_S])
                        dir -= 1;
                    if (currentKeyState[(int)SDL_Scancode.SDL_SCANCODE_W])
                        dir += 1;

                    return dir;
                case DeviceType.GamePad:
                    return 0;
                default:
                    return 0;
            }
        }
        public static float GetLateral() {
            switch (deviceType) {
                case DeviceType.KeyboardMouse:
                    float dir = 0;
                    if (currentKeyState[(int)SDL_Scancode.SDL_SCANCODE_Q])
                        dir -= 1;
                    if (currentKeyState[(int)SDL_Scancode.SDL_SCANCODE_E])
                        dir += 1;

                    return dir;
                case DeviceType.GamePad:
                    return 0;
                default:
                    return 0;
            }
        }
        public static Vector2 GetMouseDelta() {
            SDL_MouseButtonFlags buttonFlags = SDL_GetRelativeMouseState(out float deltaX, out float deltaY);

            return new Vector2(deltaX / RenderPipeline.ScreenWidth, deltaY / RenderPipeline.ScreenHeight) * -1;
        }
        public static bool GetKeyDown(SDL_Scancode key) {
            if (lastKeyState == null || currentKeyState == null)
                return false;

            return !lastKeyState[(int)key] && currentKeyState[(int)key];
        }
        public static bool GetKeyUp(SDL_Scancode key) {
            if (lastKeyState == null || currentKeyState == null)
                return false;

            return lastKeyState[(int)key] && !currentKeyState[(int)key];
        }
        public static bool GetKey(SDL_Scancode key) {
            if (currentKeyState == null)
                return false;

            return currentKeyState[(int)key];
        }
    }
}