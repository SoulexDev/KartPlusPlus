using Assimp;
using KartPlusPlus.Physics;

namespace KartPlusPlus.Renderer {
    public class Model {
        public List<(Mesh, Material)> Meshes = new List<(Mesh, Material)>();
        public AABB BoundingBox;

        public Model() { }
        public Model(params (Mesh, Material)[] meshes) {
            Meshes.AddRange(meshes);
        }
    }
}
