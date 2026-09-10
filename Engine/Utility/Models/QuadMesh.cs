using KartPlusPlus.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace KartPlusPlus.Utility {
    public class QuadMesh {
        public static float[] vertices =
        {
            //position           //normal           //uv
             0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,//triangle 1
            -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,

             0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,//triangle 2
            -0.5f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 1.0f
        };

        public static Mesh mesh;
        public static void Create() {
            mesh = new Mesh(vertices, PrimitiveType.Triangles, BufferUsageHint.StaticDraw, VertexAttribute.Vector3, VertexAttribute.Vector3, VertexAttribute.Vector2);
        }
    }
}
