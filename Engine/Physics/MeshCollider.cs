using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;

namespace KartPlusPlus.Physics {
    public class MeshCollider : Collider {
        internal BepuPhysics.Collidables.Mesh collidable;

        public override void Init() {
            base.Init();
        }
        public void SetMesh(Renderer.Mesh mesh) {
            //VertexAttribute.GetTotalSizeInBytes(mesh.Attributes);
            //Buffer<Triangle> triangles = new Buffer<Triangle>();

            collidable = new BepuPhysics.Collidables.Mesh();
        }
        internal override BodyInertia GetBodyIntertia(float mass) {
            return collidable.ComputeClosedInertia(mass);
        }
    }
}