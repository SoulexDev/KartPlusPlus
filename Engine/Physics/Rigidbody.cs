using BepuPhysics;
using BepuPhysics.Collidables;
using KartPlusPlus.Engine;
using OpenTK.Mathematics;

namespace KartPlusPlus.Physics {
    public class Rigidbody : Component {
        internal BodyHandle bodyID { get; private set; }
        internal BodyInertia bodyIntertia { get; private set; }
        internal BodyReference bodyReference { get; private set; }

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
        public float Mass = 1;
        public float SleepThreshold = 0.001f;
        private bool useGravity;
        public bool UseGravity {
            get {
                return useGravity;
            }
            set {
                if (useGravity != value) {
                    useGravity = value;

                    PhysicsSim.ChangeGravityState(bodyID, useGravity);
                }
            }
        }
        public bool Interpolate = true;
        private bool lockRotationX = false;
        private bool lockRotationY = false;
        private bool lockRotationZ = false;
        public bool LockRotationX {
            get {
                return lockRotationX;
            }
            set {
                lockRotationX = value;

                if (lockRotationX) {
                    var intertiaTensor = bodyReference.LocalInertia.InverseInertiaTensor;
                    intertiaTensor.XX = 0;
                    bodyReference.LocalInertia.InverseInertiaTensor = intertiaTensor;
                }
                else {
                    var intertiaTensor = bodyReference.LocalInertia.InverseInertiaTensor;
                    intertiaTensor.XX = bodyIntertia.InverseInertiaTensor.XX;
                    bodyReference.LocalInertia.InverseInertiaTensor = intertiaTensor;
                }
            }
        }
        public bool LockRotationY {
            get {
                return lockRotationY;
            }
            set {
                lockRotationY = value;

                if (lockRotationY) {
                    var intertiaTensor = bodyReference.LocalInertia.InverseInertiaTensor;
                    intertiaTensor.YY = 0;
                    bodyReference.LocalInertia.InverseInertiaTensor = intertiaTensor;
                }
                else {
                    var intertiaTensor = bodyReference.LocalInertia.InverseInertiaTensor;
                    intertiaTensor.YY = bodyIntertia.InverseInertiaTensor.YY;
                    bodyReference.LocalInertia.InverseInertiaTensor = intertiaTensor;
                }
            }
        }
        public bool LockRotationZ {
            get {
                return lockRotationZ;
            }
            set {
                lockRotationZ = value;

                if (lockRotationZ) {
                    var intertiaTensor = bodyReference.LocalInertia.InverseInertiaTensor;
                    intertiaTensor.ZZ = 0;
                    bodyReference.LocalInertia.InverseInertiaTensor = intertiaTensor;
                }
                else {
                    var intertiaTensor = bodyReference.LocalInertia.InverseInertiaTensor;
                    intertiaTensor.ZZ = bodyIntertia.InverseInertiaTensor.ZZ;
                    bodyReference.LocalInertia.InverseInertiaTensor = intertiaTensor;
                }
            }
        }

        private Vector3 lastSimPos;
        private Quaternion lastSimRot;
        private Vector3 nextSimPos;
        private Quaternion nextSimRot;
        public override void Init() {
            List<Component> colliders = EngineObject.GetComponentsOfType<Collider>();

            BodyDescription bodyDescription;
            BodyActivityDescription bodyActivityDescription = new BodyActivityDescription(SleepThreshold);

            if (colliders.Count > 0) {
                Collider collider = colliders[0] as Collider;
                bodyIntertia = collider.GetBodyIntertia(Mass);
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

            //add body before setting params
            bodyID = PhysicsSim.simulation.Bodies.Add(bodyDescription);
            bodyReference = PhysicsSim.simulation.Bodies[bodyID];

            //set params
            UseGravity = true;
            lastSimPos = nextSimPos = ObjTransform.Position;
            lastSimRot = nextSimRot = ObjTransform.Rotation;
        }
        public override void OnComponentAdded(Component component) {

        }
        public override void PhysicsTick() {
            lastSimPos = nextSimPos;
            lastSimRot = nextSimRot;

            nextSimPos = (Vector3)bodyReference.Pose.Position;
            nextSimRot = (Quaternion)bodyReference.Pose.Orientation;
        }
        public override void Update() {
            float frameLerp = MathHelper.Clamp((Time.NextFixedFrameTime - Time.ElapsedTime) / PhysicsSim.SecondsPerTick, 0f, 1f);
            ObjTransform.Position = Vector3.Lerp(lastSimPos, nextSimPos, frameLerp);
            ObjTransform.Rotation = nextSimRot;
        }
        public void AddForce(Vector3 force) {
            bodyReference.Velocity.Linear += (System.Numerics.Vector3)force;
        }
        public void AddForceAtPosition(Vector3 force, Vector3 position) {
            bodyReference.ApplyImpulse((System.Numerics.Vector3)force, (System.Numerics.Vector3)position);
        }
        /// <summary>
        /// Get velocity at local point
        /// </summary>
        /// <param name="point">The local space point to get velocity at</param>
        /// <returns></returns>
        public Vector3 GetPointVelocity(Vector3 point) {
            bodyReference.GetVelocityForOffset((System.Numerics.Vector3)point, out System.Numerics.Vector3 velocity);

            return (Vector3)velocity;
        }
    }
}
