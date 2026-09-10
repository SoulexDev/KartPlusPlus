using OpenTK.Graphics.OpenGL4;

namespace KartPlusPlus.Renderer {
    public enum VertexAttributeType { Int16 = 1, Int32 = 2, Int64 = 4, Float = 8, Vector2 = 16, Vector3 = 32, Vector4 = 64 };
    public class VertexAttribute {
        public int SizeInBytes;
        public int ComponentsCount;
        public VertexAttribPointerType PointerType;

        public static VertexAttribute Int16 = new VertexAttribute(VertexAttribPointerType.Int, sizeof(Int16), 1);
        public static VertexAttribute Int32 = new VertexAttribute(VertexAttribPointerType.Int, sizeof(Int32), 1);
        public static VertexAttribute Int64 = new VertexAttribute(VertexAttribPointerType.Int, sizeof(Int64), 1);
        public static VertexAttribute Float = new VertexAttribute(VertexAttribPointerType.Float, sizeof(float), 1);
        public static VertexAttribute Vector2 = new VertexAttribute(VertexAttribPointerType.Float, sizeof(float) * 2, 2);
        public static VertexAttribute Vector3 = new VertexAttribute(VertexAttribPointerType.Float, sizeof(float) * 3, 3);
        public static VertexAttribute Vector4 = new VertexAttribute(VertexAttribPointerType.Float, sizeof(float) * 4, 4);

        public static Dictionary<VertexAttributeType, VertexAttribute> attributeDict = new Dictionary<VertexAttributeType, VertexAttribute>
        {
            { VertexAttributeType.Int16, Int16 },
            { VertexAttributeType.Int32, Int32 },
            { VertexAttributeType.Int64, Int64 },
            { VertexAttributeType.Float, Float },
            { VertexAttributeType.Vector2, Vector2 },
            { VertexAttributeType.Vector3, Vector3 },
            { VertexAttributeType.Vector4, Vector4 },
        };

        public VertexAttribute(VertexAttribPointerType pointerType, int sizeInBytes, int componentsCount) {
            this.PointerType = pointerType;
            this.SizeInBytes = sizeInBytes;
            this.ComponentsCount = componentsCount;

            Console.WriteLine($"{pointerType.ToString()}, {sizeInBytes}");
        }
        public static int GetTotalSizeInBytes(params VertexAttribute[] attributes) {
            int size = 0;
            for (int i = 0; i < attributes.Length; i++) {
                size += attributes[i].SizeInBytes;
            }
            return size;
        }
    }
}
