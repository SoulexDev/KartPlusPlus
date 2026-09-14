using BepuPhysics;
using KartPlusPlus.Renderer;

namespace KartPlusPlus.Physics {
    public class MeshCollider : Collider {
        internal BepuPhysics.Collidables.Mesh collidable;

        public override void Init() {
            base.Init();
        }
        public void SetMesh(Mesh mesh) {
            //VertexAttribute.GetTotalSizeInBytes(mesh.Attributes);
            collidable = new BepuPhysics.Collidables.Mesh();
        }
        internal override BodyInertia GetBodyIntertia(float mass) {
            return collidable.ComputeClosedInertia(mass);
        }
    }
}