using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using OpenTK.Mathematics;

namespace KartPlusPlus.Physics {
    public class MeshCollider : Collider {
        internal BepuPhysics.Collidables.Mesh collidable;

        public override void Init() {
            //base.Init();
        }
        public void SetMesh(Renderer.Mesh mesh) {
            int vertexSizeInBytes = Renderer.VertexAttribute.GetTotalSizeInBytes(mesh.Attributes);
            int otherAttribSizeInBytes = vertexSizeInBytes - Renderer.VertexAttribute.Vector3.SizeInBytes;

            int vertexLengthInArr = vertexSizeInBytes / sizeof(float);
            int otherAttribLengthInArr = otherAttribSizeInBytes / sizeof(float);

            Vector3[] positions = new Vector3[mesh.Vertices.Length / vertexLengthInArr];

            int posIndex = 0;
            for (int i = 0; i < mesh.Vertices.Length; i += vertexLengthInArr) {
                positions[posIndex] = new Vector3(mesh.Vertices[i], mesh.Vertices[i + 1], mesh.Vertices[i + 2]);
                posIndex++;
            }
            int triangleCount = positions.Length / 3;
            PhysicsSim.BufferPool.Take(triangleCount, out Buffer<Triangle> triangles);

            for (int i = 0; i < triangleCount; i++) {
                ref Triangle tri = ref triangles[i];
                tri.C = (System.Numerics.Vector3)positions[mesh.Indices[i * 3]];
                tri.B = (System.Numerics.Vector3)positions[mesh.Indices[i * 3 + 1]];
                tri.A = (System.Numerics.Vector3)positions[mesh.Indices[i * 3 + 2]];
            }

            collidable = new Mesh(triangles, (System.Numerics.Vector3)ObjTransform.LocalScale, PhysicsSim.BufferPool);
            collidableIndex = PhysicsSim.simulation.Shapes.Add(collidable);
            TryCreateStaticCollider();
        }
        internal override BodyInertia GetBodyIntertia(float mass) {
            return collidable.ComputeClosedInertia(mass);
        }
    }
}