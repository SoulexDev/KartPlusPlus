using BepuPhysics;
using BepuPhysics.Collidables;
using KartPlusPlus.Engine;

namespace KartPlusPlus.Physics {
    public class Collider : Component {
        internal TypedIndex collidableIndex;
        protected StaticHandle staticHandle;

        public ContinuousDetectionMode continuousDetectionMode = ContinuousDetectionMode.Discrete;

        protected bool isStatic;

        private bool hasRigidbody;

        public override void Init() {
            TryCreateStaticCollider();

            ObjTransform.OnChanged += OnTransformChanged;
            EngineObject.physicsLayerChanged += OnPhysicsLayerChanged;

            OnPhysicsLayerChanged(EngineObject.PhysicsLayer);
        }
        private void OnPhysicsLayerChanged(int layer) {
            if (!hasRigidbody) {
                PhysicsSim.ChangePhysicsLayer(staticHandle, layer);
            }
        }
        public override void OnComponentAdded(Component component) {
            if (component is Rigidbody) {
                PhysicsSim.simulation.Statics.Remove(staticHandle);

                isStatic = false;
                hasRigidbody = true;
            }
        }
        public override void OnComponentRemoved(Component component) {
            if (component is Rigidbody) {
                RigidPose pose = new RigidPose((System.Numerics.Vector3)ObjTransform.Position, (System.Numerics.Quaternion)ObjTransform.Rotation);
                StaticDescription staticDescription = new StaticDescription(pose, collidableIndex);
                staticHandle = PhysicsSim.simulation.Statics.Add(staticDescription);

                isStatic = true;
                hasRigidbody = false;
            }
        }
        public override void OnDestroy() {
            ObjTransform.OnChanged -= OnTransformChanged;
            EngineObject.physicsLayerChanged -= OnPhysicsLayerChanged;
        }
        protected void TryCreateStaticCollider() {
            if (!EngineObject.HasComponentOfType<Rigidbody>()) {
                RigidPose pose = new RigidPose((System.Numerics.Vector3)ObjTransform.Position, (System.Numerics.Quaternion)ObjTransform.Rotation);
                StaticDescription staticDescription = new StaticDescription(pose, collidableIndex);
                staticHandle = PhysicsSim.simulation.Statics.Add(staticDescription);

                isStatic = true;
            }
        }
        private void OnTransformChanged() {
            if (isStatic) {
                PhysicsSim.simulation.Statics[staticHandle].GetDescription(out StaticDescription desc);
                desc.Pose = ObjTransform;
                PhysicsSim.simulation.Statics[staticHandle].ApplyDescription(desc);
            }
        }
        internal virtual BodyInertia GetBodyIntertia(float mass) {
            return default;
        }
    }
}
