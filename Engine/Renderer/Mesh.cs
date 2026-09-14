using OpenTK.Graphics.OpenGL4;

namespace KartPlusPlus.Renderer {
    public class Mesh {
        public uint VAO;
        public uint VBO;
        public uint EBO;
        public int Count;
        public float[] Vertices;
        public int[] Indices;
        public VertexAttribute[] Attributes;
        public PrimitiveType PrimitiveType;
        public BufferUsageHint DrawType;

        private bool generatedBuffers;
        private bool usingIndices;

        public Mesh() {

        }
        public Mesh(PrimitiveType primitiveType, BufferUsageHint drawType) {
            PrimitiveType = primitiveType;
            DrawType = drawType;
        }
        public Mesh(float[] vertices, PrimitiveType primitiveType, BufferUsageHint drawType, params VertexAttribute[] attributes) {
            PrimitiveType = primitiveType;
            DrawType = drawType;
            SetMesh(vertices, attributes);
        }
        public Mesh(float[] vertices, int[] indices, PrimitiveType primitiveType, BufferUsageHint drawType, params VertexAttribute[] attributes) {
            PrimitiveType = primitiveType;
            DrawType = drawType;
            SetMesh(vertices, indices, attributes);
        }
        public Mesh(List<float> vertices, PrimitiveType primitiveType, BufferUsageHint drawType, params VertexAttribute[] attributes) {
            PrimitiveType = primitiveType;
            DrawType = drawType;
            SetMesh(vertices.ToArray(), attributes);
        }
        public Mesh(List<float> vertices, List<int> indices, PrimitiveType primitiveType, BufferUsageHint drawType, params VertexAttribute[] attributes) {
            PrimitiveType = primitiveType;
            DrawType = drawType;
            SetMesh(vertices.ToArray(), indices.ToArray(), attributes);
        }
        public Mesh SetMesh(float[] vertices, params VertexAttribute[] attributes) {
            if (vertices.Length <= 0) {
                Console.WriteLine("Attempted to create mesh with no data. This is not allowed.");
                return this;
            }

            Vertices = vertices;
            Attributes = attributes;

            usingIndices = false;

            if (!generatedBuffers) {
                GL.GenVertexArrays(1, out VAO);
                GL.GenBuffers(1, out VBO);
            }

            Count = vertices.Length;

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Count * sizeof(float), vertices.ToArray(), DrawType);

            int stride = VertexAttribute.GetTotalSizeInBytes(attributes);
            int offset = 0;
            for (int i = 0; i < attributes.Length; i++) {
                VertexAttribute attrib = attributes[i];

                GL.VertexAttribPointer(i, attrib.ComponentsCount, attrib.PointerType, false, stride, offset);
                GL.EnableVertexAttribArray(i);

                offset += attrib.SizeInBytes;
            }

            generatedBuffers = true;
            return this;
        }
        public Mesh SetMesh(float[] vertices, int[] indices, params VertexAttribute[] attributes) {
            if (vertices.Length <= 0 || indices.Length <= 0) {
                Console.WriteLine("Attempted to create mesh with no data. This is not allowed.");
                return this;
            }

            Vertices = vertices;
            Indices = indices;
            Attributes = attributes;

            usingIndices = true;

            if (!generatedBuffers) {
                GL.GenVertexArrays(1, out VAO);
                GL.GenBuffers(1, out VBO);
                GL.GenBuffers(1, out EBO);
            }

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices.ToArray(), DrawType);

            Count = indices.Length;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Count * sizeof(uint), indices.ToArray(), DrawType);

            int stride = VertexAttribute.GetTotalSizeInBytes(attributes);
            int offset = 0;
            for (int i = 0; i < attributes.Length; i++) {
                VertexAttribute attrib = attributes[i];

                GL.VertexAttribPointer(i, attrib.ComponentsCount, attrib.PointerType, false, stride, offset);
                GL.EnableVertexAttribArray(i);

                offset += attrib.SizeInBytes;
            }

            generatedBuffers = true;
            return this;
        }
        //draw normally
        public void Draw() {
            if (!generatedBuffers || Count == 0)
                return;

            GL.BindVertexArray(VAO);
            if (usingIndices)
                GL.DrawElements(PrimitiveType, Count, DrawElementsType.UnsignedInt, 0);
            else
                GL.DrawArrays(PrimitiveType, 0, Count);

            GL.BindVertexArray(0);
        }
        //draw override with special primitive type
        public void Draw(PrimitiveType primitiveType) {
            if (!generatedBuffers)
                return;

            GL.BindVertexArray(VAO);
            if (usingIndices)
                GL.DrawElements(primitiveType, Count, DrawElementsType.UnsignedInt, 0);
            else
                GL.DrawArrays(primitiveType, 0, Count);

            GL.BindVertexArray(0);
        }
        public void Clear() {
            Count = 0;
        }
    }
}
