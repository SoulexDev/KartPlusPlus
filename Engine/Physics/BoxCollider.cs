using BepuPhysics;
using BepuPhysics.Collidables;

namespace KartPlusPlus.Physics {
    public class BoxCollider : Collider {
        internal Box collidable;

        public float width {
            get { return collidable.Width; }
            set { collidable.Width = value; }
        }
        public float height {
            get { return collidable.Height; }
            set { collidable.Height = value; }
        }
        public float length {
            get { return collidable.Length; }
            set { collidable.Length = value; }
        }
        public override void Init() {
            //TODO: make fit object bounding box
            collidable = new Box(ObjTransform.LocalScale.X, ObjTransform.LocalScale.Y, ObjTransform.LocalScale.Z);
            collidableIndex = PhysicsSim.simulation.Shapes.Add(collidable);
            
            //always call base after in this scenario
            base.Init();
        }
        internal override BodyInertia GetBodyIntertia(float mass) {
            return collidable.ComputeInertia(mass);
        }
    }
}
