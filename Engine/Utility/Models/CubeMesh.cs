using KartPlusPlus.Engine;
using KartPlusPlus.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace KartPlusPlus.Utility {
    public class CubeMesh {
        private static float[] vertices = new float[]
        {
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f, 0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f, 0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, -1.0f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  -1.0f, 0.0f, 0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  -1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  -1.0f, 0.0f, 0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  -1.0f, 0.0f, 0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  -1.0f, 0.0f, 0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  -1.0f, 0.0f, 0.0f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 0.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 0.0f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f, 0.0f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f, 0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f, 0.0f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f, 0.0f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f, 0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f, 0.0f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f, 0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f
        };

        public static Mesh mesh;
        public static void Create() {
            mesh = new Mesh(vertices, PrimitiveType.Triangles, BufferUsageHint.StaticDraw, VertexAttribute.Vector3, VertexAttribute.Vector3, VertexAttribute.Vector2);
        }
        public static Model Generate(Texture2D texture) {
            Material mat = new Material(DefaultResources.OpaqueShader).DisableCullFace();
            mat.Texture2Ds.Add(("uDiffuse0", texture));

            Mesh mesh = new Mesh(vertices, PrimitiveType.Triangles, BufferUsageHint.StaticDraw,
                VertexAttribute.Vector3, VertexAttribute.Vector3, VertexAttribute.Vector2);

            return new Model((mesh, mat));
        }
    }
}