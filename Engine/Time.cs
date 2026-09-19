using KartPlusPlus.Physics;
using static SDL3.SDL;

namespace KartPlusPlus.Engine {
    public class Time {
        private static float lastFrameTime;
        private static float lastFixedFrameTime;

        public static float ElapsedTime = 0.0f;
        public static float DeltaTime = 0.0f;
        public static float FixedDeltaTime = 0.0f;
        public static float NextFixedFrameTime;

        internal static void Update() {
            ElapsedTime = SDL_GetTicks() / 1000.0f;
            DeltaTime = ElapsedTime - lastFrameTime;
            lastFrameTime = ElapsedTime;
        }
        internal static void FixedUpdate() {
            if (ElapsedTime < lastFixedFrameTime) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"We went BACKWARDS in TIME?? now:{ElapsedTime}, earlier:{lastFixedFrameTime}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            FixedDeltaTime = ElapsedTime - lastFixedFrameTime;
            lastFixedFrameTime = ElapsedTime;
            NextFixedFrameTime = lastFixedFrameTime + PhysicsSim.SecondsPerTick;
        }
    }
}
