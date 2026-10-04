using BepuPhysics;
using BepuUtilities;
using System.Numerics;

namespace KartPlusPlus.Physics {
    public struct DefaultPoseIntegratorCallbacks : IPoseIntegratorCallbacks {
        public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

        public bool AllowSubstepsForUnconstrainedBodies => false;

        public bool IntegrateVelocityForKinematics => false;

        private Bodies bodies;

        //body params
        public Vector3 Gravity;
        public CollidableProperty<bool> BodyGravities;
        public float LinearDamping;
        public float AngularDamping;

        //simd params
        private Vector3Wide gravityWideDt;
        private Vector<float> linearDampingDt;
        private Vector<float> angularDampingDt;

        public DefaultPoseIntegratorCallbacks(Vector3 gravity, CollidableProperty<bool> bodyGravities,
            float linearDamping = 0.03f, float angularDamping = 0.03f) : this() {
            Gravity = gravity;
            LinearDamping = linearDamping;
            AngularDamping = angularDamping;
            BodyGravities = bodyGravities;
        }
        public void Initialize(Simulation simulation) {
            BodyGravities.Initialize(simulation);
            bodies = simulation.Bodies;
        }
        public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position,
            QuaternionWide orientation, BodyInertiaWide localInertia,
            Vector<int> integrationMask, int workerIndex,
            Vector<float> dt, ref BodyVelocityWide velocity) {

            Span<float> gravityValues = stackalloc float[Vector<float>.Count];
            for (int i = 0; i < Vector<int>.Count; ++i) {
                int bodyIndex = bodyIndices[i];

                if (bodyIndex >= 0) {
                    BodyHandle bodyHandle = bodies.ActiveSet.IndexToHandle[bodyIndex];
                    gravityValues[i] = BodyGravities[bodyHandle] ? -9.81f : 0;
                }
            }
            //velocity.Linear = (velocity.Linear + gravityWideDt) * linearDampingDt;
            //velocity.Angular = velocity.Angular * angularDampingDt;
            velocity.Linear.Y += new Vector<float>(gravityValues) * dt;
        }
        public void PrepareForIntegration(float dt) {
            linearDampingDt = new Vector<float>(MathF.Pow(MathHelper.Clamp(1 - LinearDamping, 0, 1), dt));
            angularDampingDt = new Vector<float>(MathF.Pow(MathHelper.Clamp(1 - AngularDamping, 0, 1), dt));
            //TODO: cache gravity * dt
            gravityWideDt = Vector3Wide.Broadcast(Gravity * dt);
        }
    }
}
