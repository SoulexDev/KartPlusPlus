using BepuPhysics;
using OpenTK.Mathematics;

namespace KartPlusPlus.Engine {
    public class Transform : Component {
        public Transform Parent;
        public List<Transform> Children;

        new public Transform ObjTransform;

        private bool isDirty = true;
        public event Action OnChanged;

        public Vector3 LocalPosition = Vector3.Zero;
        public Quaternion LocalRotation = Quaternion.Identity;
        private Vector3 localScale = Vector3.One;
        public Vector3 LocalScale {
            get {
                return localScale;
            }
            set {
                localScale = value;
                SetDirty();
            }
        }

        private Matrix4 localMatrix = Matrix4.Identity;
        public Matrix4 LocalMatrix {
            get {
                if (isDirty) {
                    localMatrix = Matrix4.CreateScale(LocalScale) * Matrix4.CreateFromQuaternion(LocalRotation) * Matrix4.CreateTranslation(LocalPosition);
                    isDirty = false;
                }

                return localMatrix;
            }
        }
        //private Matrix4 worldMatrix = Matrix4.Identity;
        public Matrix4 WorldMatrix {
            get {
                //worldMatrix = localMatrix;

                //if (Parent != null) {
                //    worldMatrix = localMatrix * Parent.WorldMatrix;
                //}

                return Parent != null ? LocalMatrix * Parent.WorldMatrix : LocalMatrix;
            }
        }

        public Vector3 Position {
            get {
                Vector3 worldPos = LocalPosition;
                Transform nextParent = Parent;

                while (nextParent != null) {
                    worldPos = worldPos * Parent.LocalScale;
                    worldPos = Parent.LocalRotation * worldPos;
                    worldPos += Parent.LocalPosition;

                    nextParent = nextParent.Parent;
                }

                return worldPos;
            }
            set {
                if (Parent != null) {
                    LocalPosition = Parent.InverseTransformPoint(value);
                    return;
                }

                LocalPosition = value;

                SetDirty();
            }
        }
        public Quaternion Rotation {
            get {
                Quaternion worldRotation = LocalRotation;
                Transform nextParent = Parent;

                while (nextParent != null) {
                    worldRotation *= nextParent.LocalRotation;
                    nextParent = nextParent.Parent;
                }

                return worldRotation;
            }
            set {
                if (Parent == null) {
                    LocalRotation = value;
                    return;
                }

                LocalRotation = Parent.Rotation.Inverted() * value;

                SetDirty();
            }
        }
        
        public Vector3 Forward {
            get {
                return Rotation * Vector3.UnitZ;
            }
        }
        public Vector3 Right {
            get {
                return Rotation * Vector3.UnitX; 
            }
        }
        public Vector3 Up {
            get {
                return Rotation * Vector3.UnitY;
            }
        }
        internal Transform() {
            //localScale = Vector3.One;
            Children = new List<Transform>();
            SetDirty();
        }
        internal Transform (EngineObject obj) {
            //localScale = Vector3.One;
            EngineObject = obj;
            Children = new List<Transform>();
            SetDirty();
        }
        internal void SetDirty() {
            isDirty = true;
            Children.ForEach(c => c.SetDirty());

            OnChanged?.Invoke();
        }
        public void SetParent(Transform transform, bool keepWorld = false) {
            if (transform == this) {
                Console.WriteLine("Cannot set transform as parent of self");
                return;
            }

            if (transform != null) {
                if (keepWorld) {
                    Vector3 relativePos = Position - transform.Position;
                    Quaternion relativeRot = Rotation * transform.Rotation.Inverted();
                    //Vector3 relativeScale = LocalScale / transform.LocalScale;

                    Parent = transform;
                    transform.Children.Add(this);

                    LocalPosition = relativePos;
                    LocalRotation = relativeRot;
                    //LocalScale = relativeScale;
                }
                else {
                    Parent = transform;
                    transform.Children.Add(this);

                    LocalPosition = Vector3.Zero;
                    LocalRotation = Quaternion.Identity;
                    LocalScale = Vector3.One;
                }
            }
            else {
                if (keepWorld) {
                    Vector3 worldPos = Position;
                    Quaternion worldRot = Rotation;
                    //Vector3 worldScale = LocalScale;

                    Parent.Children.Remove(this);
                    Parent = null;

                    Position = worldPos;
                    Rotation = worldRot;
                    //LocalScale = worldScale;
                }
                else {
                    Parent.Children.Remove(this);
                    Parent = null;
                }
            }

            SetDirty();
        }
        /// <summary>
        /// Transform a point from world space to local space
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 InverseTransformPoint(Vector3 point) {
            if (Parent != null) {
                point = Parent.InverseTransformPoint(point);
            }

            point = point - LocalPosition;
            point = LocalRotation.Inverted() * point;
            point /= LocalScale;

            return point;
        }
        public void Rotate(Vector3 axis, float angle) {
            Rotation *= Quaternion.FromAxisAngle(axis, angle * (MathF.PI / 180.0f));
        }
        public static implicit operator RigidPose(Transform transform) {
            return new RigidPose((System.Numerics.Vector3)transform.Position, (System.Numerics.Quaternion)transform.Rotation);
        }
    }
}