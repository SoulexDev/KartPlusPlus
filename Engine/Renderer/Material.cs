using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections;

namespace KartPlusPlus.Renderer {
    public class Material {
        public Shader Shader;

        private BlendingFactor srcBlendMode = BlendingFactor.SrcAlpha;
        private BlendingFactor dstBlendMode = BlendingFactor.OneMinusSrcAlpha;

        private LogicOp logicOp = LogicOp.Noop;

        private TriangleFace cullFaceMode = TriangleFace.Back;

        private DepthFunction depthFunction = DepthFunction.Lequal;

        public List<(string, Texture2D)> Texture2Ds = new List<(string, Texture2D)>();
        public List<(string, int)> Integers = new List<(string, int)>();
        public List<(string, float)> Floats = new List<(string, float)>();
        public List<(string, Vector2)> Vector2s = new List<(string, Vector2)>();
        public List<(string, Vector3)> Vector3s = new List<(string, Vector3)>();
        public List<(string, Vector4)> Vector4s = new List<(string, Vector4)>();

        private BitArray materialStates;

        public Material(Shader shader) {
            Shader = shader;

            //blending, logic op, cull face, depth test
            materialStates = new BitArray([false, false, true, true]);
        }
        public Material EnableBlending(BlendingFactor srcBlendMode, BlendingFactor dstBlendMode) {
            materialStates[0] = true;
            this.srcBlendMode = srcBlendMode;
            this.dstBlendMode = dstBlendMode;
            return this;
        }
        public Material DisableBlending() {
            materialStates[0] = false;
            return this;
        }
        public Material EnableLogicOp(LogicOp logicOp) {
            materialStates[1] = true;
            this.logicOp = logicOp;
            return this;
        }
        public Material DisableLogicOp() {
            materialStates[1] = false;
            return this;
        }
        public Material EnableCullFace(TriangleFace cullFaceMode) {
            materialStates[2] = true;
            this.cullFaceMode = cullFaceMode;
            return this;
        }
        public Material DisableCullFace() {
            materialStates[2] = false;
            return this;
        }
        public Material EnableDepthTest(DepthFunction depthFunction) {
            materialStates[3] = true;
            this.depthFunction = depthFunction;
            return this;
        }
        public Material DisableDepthTest() {
            materialStates[3] = false;
            return this;
        }
        public Material Clone() {
            Material clone = new Material(Shader);

            clone.materialStates = new BitArray(materialStates);
            clone.srcBlendMode = srcBlendMode;
            clone.dstBlendMode = dstBlendMode;
            clone.logicOp = logicOp;
            clone.cullFaceMode = cullFaceMode;
            clone.depthFunction = depthFunction;
            clone.Texture2Ds = new List<(string, Texture2D)>(Texture2Ds);
            clone.Integers = new List<(string, int)>(Integers);
            clone.Floats = new List<(string, float)>(Floats);
            clone.Vector2s = new List<(string, Vector2)>(Vector2s);
            clone.Vector3s = new List<(string, Vector3)>(Vector3s);
            clone.Vector4s = new List<(string, Vector4)>(Vector4s);

            return clone;
        }
        public Material AssignTexture(string textureName, Texture2D texture) {
            if (!Texture2Ds.Exists(t => t.Item1 == textureName)) {
                Texture2Ds.Add((textureName, texture));
            }
            else {
                int index = Texture2Ds.FindIndex(t => t.Item1 == textureName);
                if (index == -1)
                    Console.WriteLine($"Texture {textureName} is not present in material.");
                else
                    Texture2Ds[index] = (textureName, texture);
            }

            return this;
        }
        public void Use() {
            Shader.Use();

            //blending
            if (materialStates[0]) {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(srcBlendMode, dstBlendMode);
            }
            else
                GL.Disable(EnableCap.Blend);

            //logic op
            if (materialStates[1]) {
                GL.Enable(EnableCap.ColorLogicOp);
                GL.LogicOp(logicOp);
            }
            else
                GL.Disable(EnableCap.ColorLogicOp);

            //cull face
            if (materialStates[2]) {
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(cullFaceMode);
            }
            else
                GL.Disable(EnableCap.CullFace);

            //depth test
            if (materialStates[3]) {
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(depthFunction);
            }
            else
                GL.Disable(EnableCap.DepthTest);

            //textures
            if (Texture2Ds.Count > 0) {
                for (int i = 0; i < Texture2Ds.Count; i++) {
                    //TODO: increment and decrement light count depending on distance to determine the shadow texture offset
                    Texture2Ds[i].Item2.Bind(GetTextureUnit(i + RenderPipeline.Lights.Length));
                    Shader.SetTexture(Texture2Ds[i].Item1, i + RenderPipeline.Lights.Length);
                }
            }

            //integers
            if (Integers.Count > 0) {
                for (int i = 0; i < Integers.Count; i++) {
                    Shader.SetInt(Integers[i].Item1, Integers[i].Item2);
                }
            }

            //floats
            if (Floats.Count > 0) {
                for (int i = 0; i < Floats.Count; i++) {
                    Shader.SetFloat(Floats[i].Item1, Floats[i].Item2);
                }
            }

            //vector2s
            if (Vector2s.Count > 0) {
                for (int i = 0; i < Vector2s.Count; i++) {
                    Shader.SetVector(Vector2s[i].Item1, Vector2s[i].Item2);
                }
            }

            //vector3s
            if (Vector3s.Count > 0) {
                for (int i = 0; i < Vector3s.Count; i++) {
                    Shader.SetVector(Vector3s[i].Item1, Vector3s[i].Item2);
                }
            }

            //vector4s
            if (Vector4s.Count > 0) {
                for (int i = 0; i < Vector4s.Count; i++) {
                    Shader.SetVector(Vector4s[i].Item1, Vector4s[i].Item2);
                }
            }
        }
        private static TextureUnit GetTextureUnit(int index) {
            if (index >= 32) {
                Console.WriteLine("Requested texture unit index is out of bounds.");
                return TextureUnit.Texture31;
            }
            return TextureUnit.Texture0 + index;
        }
    }
}
