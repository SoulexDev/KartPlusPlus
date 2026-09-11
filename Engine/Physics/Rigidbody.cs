using BepuPhysics;
using BepuPhysics.Collidables;
using KartPlusPlus.Engine;
using OpenTK.Mathematics;

namespace KartPlusPlus.Physics {
    public class Rigidbody : Component {
        internal BodyHandle bodyID { get; private set; }
        internal BodyInertia bodyIntertia { get; private set; }

        public Vector3 LinearVelocity {
            get {
                return (Vector3)PhysicsSim.simulation.Bodies[bodyID].Velocity.Linear;
            }
            set {
                PhysicsSim.simulation.Bodies[bodyID].Velocity.Linear = (System.Numerics.Vector3)value;
            }
        }
        public Vector3 AngularVelocity {
            get {
                return (Vector3)PhysicsSim.simulation.Bodies[bodyID].Velocity.Angular;
            }
            set {
                PhysicsSim.simulation.Bodies[bodyID].Velocity.Angular = (System.Numerics.Vector3)value;
            }
        }

        private bool _kinematic;
        public bool Kinematic {
            get { return _kinematic; }
            set {
                _kinematic = value;
                if (_kinematic) {
                    PhysicsSim.simulation.Bodies[bodyID].BecomeKinematic();
                }
                else
                    PhysicsSim.simulation.Bodies[bodyID].SetLocalInertia(bodyIntertia);
            }
        }
        public override void Init() {
            List<Component> colliders = EngineObject.GetComponentsOfType<Collider>();

            BodyDescription bodyDescription;
            BodyActivityDescription bodyActivityDescription = new BodyActivityDescription(0.001f);

            if (colliders.Count > 0) {
                Collider collider = (colliders[0] as Collider);
                bodyIntertia = collider.GetBodyIntertia(1);
                //TODO: make this an option later
                //ContinuousDetection continuousDetection = ContinuousDetection.Continuous();

                //CollidableDescription collidableDescription = new CollidableDescription(collider.collidableIndex, continuousDetection);
                bodyDescription = BodyDescription.CreateDynamic((System.Numerics.Vector3)ObjTransform.Position, bodyIntertia, collider.collidableIndex, 0.01f);
            }
            else {
                BodyVelocity bodyVelocity = new BodyVelocity(System.Numerics.Vector3.Zero);
                CollidableDescription collidableDescription = new CollidableDescription();
                bodyDescription = BodyDescription.CreateKinematic((System.Numerics.Vector3)ObjTransform.Position, bodyVelocity, collidableDescription, bodyActivityDescription);
            }

            bodyID = PhysicsSim.simulation.Bodies.Add(bodyDescription);
        }
        public override void OnComponentAdded(Component component) {

        }
        public override void PhysicsTick() {
            ObjTransform.Position = (Vector3)PhysicsSim.simulation.Bodies[bodyID].Pose.Position;
            ObjTransform.Rotation = (Quaternion)PhysicsSim.simulation.Bodies[bodyID].Pose.Orientation;
        }
        public void AddForce(Vector3 force) {
            PhysicsSim.simulation.Bodies[bodyID].ApplyLinearImpulse((System.Numerics.Vector3)force);
        }
        public void AddForceAtPosition(Vector3 force, Vector3 position) {
            PhysicsSim.simulation.Bodies[bodyID].ApplyImpulse((System.Numerics.Vector3)force, (System.Numerics.Vector3)position);
        }
        /// <summary>
        /// Get velocity at local point
        /// </summary>
        /// <param name="point">The local space point to get velocity at</param>
        /// <returns></returns>
        public Vector3 GetPointVelocity(Vector3 point) {
            PhysicsSim.simulation.Bodies[bodyID].GetVelocityForOffset((System.Numerics.Vector3)point, out System.Numerics.Vector3 velocity);

            return (Vector3)velocity;
        }
    }
}
