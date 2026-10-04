using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities;
using BepuUtilities.Memory;
using KartPlusPlus.Engine;
using System.Numerics;

namespace KartPlusPlus.Physics {
    public partial class PhysicsSim {
        internal static Simulation simulation;

        private static ThreadDispatcher threadDispatcher;
        public static BufferPool BufferPool;

        public static float SecondsPerTick = 1.0f / 50.0f;
        public static long MillisecondsPerTick => (long)(SecondsPerTick * 1000);

        //body params
        private static CollidableProperty<bool> bodyGravities;
        private static CollidableProperty<int> bodyPhysicsLayers;

        public static void Init() {
            threadDispatcher = new ThreadDispatcher(Environment.ProcessorCount);
            BufferPool = new BufferPool();

            bodyGravities = new CollidableProperty<bool>(BufferPool);
            bodyPhysicsLayers = new CollidableProperty<int>(BufferPool);

            layers = new List<LayerMask>();
            layerNames = new List<string>();

            simulation = Simulation.Create(BufferPool, new DefaultNarrowPhaseCallbacks(bodyPhysicsLayers), 
                new DefaultPoseIntegratorCallbacks(new Vector3(0, -9.81f, 0), bodyGravities), new SolveDescription(8, 1));

            AddLayer("default");
        }
        public static void Tick() {
            if (Time.FixedDeltaTime <= 0) {
                Time.FixedDeltaTime = SecondsPerTick;
            }
            simulation.Timestep(SecondsPerTick, threadDispatcher);
        }
        public static void Dispose() {
            simulation.Dispose();
            threadDispatcher.Dispose();
            BufferPool.Clear();
            bodyGravities.Dispose();
        }
        public static void ChangeGravityState(BodyHandle bodyHandle, bool state) {
            bodyGravities.Allocate(bodyHandle) = state;
        }
        public static void ChangePhysicsLayer(BodyHandle bodyHandle, int layerIndex) {
            bodyPhysicsLayers.Allocate(bodyHandle) = layerIndex;
        }
        public static void ChangePhysicsLayer(StaticHandle staticHandle, int layerIndex) {
            bodyPhysicsLayers.Allocate(staticHandle) = layerIndex;
        }
        public static void ChangePhysicsLayer(CollidableReference collidable, int layerIndex) {
            bodyPhysicsLayers.Allocate(collidable) = layerIndex;
        }
        public static int GetPhysicsLayerIndex(BodyHandle bodyHandle) {
            return bodyPhysicsLayers.Allocate(bodyHandle);
        }
        public static int GetPhysicsLayerIndex(StaticHandle staticHandle) {
            return bodyPhysicsLayers.Allocate(staticHandle);
        }
        public static int GetPhysicsLayerIndex(CollidableReference collidable) {
            return bodyPhysicsLayers.Allocate(collidable);
        }
    }
}
