using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;
using KartPlusPlus.Engine;
using System.Runtime.CompilerServices;

namespace KartPlusPlus.Physics {
    public partial class PhysicsSim {
        internal static Simulation simulation;

        private static ThreadDispatcher threadDispatcher;
        private static BufferPool bufferPool;

        public static float SecondsPerTick = 1.0f / 50.0f;
        public static long MillisecondsPerTick => (long)(SecondsPerTick * 1000);

        public static void Init() {
            threadDispatcher = new ThreadDispatcher(Environment.ProcessorCount);
            bufferPool = new BufferPool();

            simulation = Simulation.Create(bufferPool, new NarrowPhaseCallbacks(),
                new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, -9.81f, 0)), new SolveDescription(8, 1));
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
            bufferPool.Clear();
        }

        public struct NarrowPhaseCallbacks : INarrowPhaseCallbacks {
            public SpringSettings ContactSpringiness;
            public float MaximumRecoveryVelocity;
            public float FrictionCoefficient;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref float speculativeMargin) {
                return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB) {
                return true;
            }
            public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold> {
                pairMaterial.FrictionCoefficient = FrictionCoefficient;
                pairMaterial.MaximumRecoveryVelocity = MaximumRecoveryVelocity;
                pairMaterial.SpringSettings = ContactSpringiness;
                return true;
            }
            public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold) {
                return true;
            }
            public void Initialize(Simulation simulation) {
                if (ContactSpringiness.AngularFrequency == 0 && ContactSpringiness.TwiceDampingRatio == 0) {
                    ContactSpringiness = new SpringSettings(30, 1);
                    MaximumRecoveryVelocity = 2;
                    FrictionCoefficient = 1;
                }
            }
            public void Dispose() {

            }
        }
        public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks {
            public System.Numerics.Vector3 Gravity;
            public float LinearDamping;
            public float AngularDamping;

            public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

            public bool AllowSubstepsForUnconstrainedBodies => false;

            public bool IntegrateVelocityForKinematics => false;

            Vector3Wide gravityWideDt;
            System.Numerics.Vector<float> linearDampingDt;
            System.Numerics.Vector<float> angularDampingDt;

            public PoseIntegratorCallbacks(System.Numerics.Vector3 gravity, float linearDamping = 0.03f, float angularDamping = 0.03f) : this() {
                Gravity = gravity;
                LinearDamping = linearDamping;
                AngularDamping = angularDamping;
            }
            public void Initialize(Simulation simulation) {

            }
            public void IntegrateVelocity(System.Numerics.Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation, BodyInertiaWide localInertia, System.Numerics.Vector<int> integrationMask, int workerIndex, System.Numerics.Vector<float> dt, ref BodyVelocityWide velocity) {
                //velocity.Linear = (velocity.Linear + gravityWideDt) * linearDampingDt;
                //velocity.Angular = velocity.Angular * angularDampingDt;
                velocity.Linear += gravityWideDt;
            }
            public void PrepareForIntegration(float dt) {
                linearDampingDt = new System.Numerics.Vector<float>(MathF.Pow(MathHelper.Clamp(1 - LinearDamping, 0, 1), dt));
                angularDampingDt = new System.Numerics.Vector<float>(MathF.Pow(MathHelper.Clamp(1 - AngularDamping, 0, 1), dt));
                //TODO: cache gravity * dt
                gravityWideDt = Vector3Wide.Broadcast(Gravity * dt);
            }
        }
    }
}
