using KartPlusPlus.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace KartPlusPlus.Utility {
    public class FullscreenQuadMesh {
        public static List<float> vertices = new List<float>
        {
            -1.0f,  1.0f,  0.0f, 1.0f,
            -1.0f, -1.0f,  0.0f, 0.0f,
            1.0f, -1.0f,  1.0f, 0.0f,

            -1.0f,  1.0f,  0.0f, 1.0f,
            1.0f, -1.0f,  1.0f, 0.0f,
            1.0f,  1.0f,  1.0f, 1.0f
        };
        public static Mesh mesh;
        public static void Create() {
            mesh = new Mesh(vertices, PrimitiveType.Triangles, BufferUsageHint.StaticDraw, VertexAttribute.Vector2, VertexAttribute.Vector2);
        }
    }
}