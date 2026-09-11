using BepuPhysics;
using BepuPhysics.Collidables;
using KartPlusPlus.Engine;

namespace KartPlusPlus.Physics {
    public class Collider : Component {
        internal TypedIndex collidableIndex;
        protected StaticHandle staticHandle;

        public ContinuousDetectionMode continuousDetectionMode = ContinuousDetectionMode.Discrete;

        protected bool isStatic;

        public override void Init() {
            if (!EngineObject.HasComponentOfType<Rigidbody>()) {
                RigidPose pose = new RigidPose((System.Numerics.Vector3)ObjTransform.Position, (System.Numerics.Quaternion)ObjTransform.Rotation);
                StaticDescription staticDescription = new StaticDescription(pose, collidableIndex);
                staticHandle = PhysicsSim.simulation.Statics.Add(staticDescription);

                isStatic = true;
            }

            ObjTransform.OnChanged += OnTransformChanged;
        }
        public override void OnComponentAdded(Component component) {
            if (component is Rigidbody) {
                PhysicsSim.simulation.Statics.Remove(staticHandle);

                isStatic = false;
            }
        }
        public override void OnComponentRemoved(Component component) {
            if (component is Rigidbody) {
                RigidPose pose = new RigidPose((System.Numerics.Vector3)ObjTransform.Position, (System.Numerics.Quaternion)ObjTransform.Rotation);
                StaticDescription staticDescription = new StaticDescription(pose, collidableIndex);
                staticHandle = PhysicsSim.simulation.Statics.Add(staticDescription);

                isStatic = true;
            }
        }
        public override void OnDestroy() {
            ObjTransform.OnChanged -= OnTransformChanged;
        }
        private void OnTransformChanged() {
            if (isStatic) {
                PhysicsSim.simulation.Statics[staticHandle].GetDescription(out StaticDescription desc);
                desc.Pose = ObjTransform;
                PhysicsSim.simulation.Statics[staticHandle].ApplyDescription(desc);
            }
        }
        internal virtual BodyInertia GetBodyIntertia(float mass) {
            return default(BodyInertia);
        }
    }
}
