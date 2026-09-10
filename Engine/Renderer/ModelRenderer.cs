using KartPlusPlus.Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace KartPlusPlus.Renderer {
    public sealed class ModelRenderer : Component, IRenderer {
        public Model Model;
        private Matrix4 modelMatrix;

        public ModelRenderer() {

        }
        public ModelRenderer(Model model, Matrix4 modelMatrix) {
            Model = model;
            this.modelMatrix = modelMatrix;

            RenderPipeline.AddRenderer(this);
        }
        public override void Init() {
            RenderPipeline.AddRenderer(this);
        }
        public override void OnDestroy() {
            RenderPipeline.RemoveRenderer(this);
        }
        public ModelRenderer SetMaterial(Material mat, int meshIndex = 0) {
            Model.Meshes[0] = (Model.Meshes[0].Item1, mat);
            return this;
        }
        public ModelRenderer SetModel(Model model) {
            Model = model;
            return this;
        }
        public ModelRenderer SetModelMatrix(Matrix4 modelMatrix) {
            this.modelMatrix = modelMatrix;
            return this;
        }
        public void Draw(Camera camera) {
            if (Model == null || (Model != null && Model.Meshes.Count == 0))
                return;

            //(Mesh, Material) pair = model.meshes[meshIndex];
            //Mesh mesh = pair.Item1;
            //Material mat = pair.Item2;

            //mat.shader.SetMatrix4("uModel", modelMatrix);
            //mat.shader.SetMatrix4("uView", camera.viewMatrix);
            //mat.shader.SetMatrix4("uProjection", camera.projectionMatrix);

            //for (int i = 0; i < RenderPipeline.lights.Count; i++)
            //{
            //    Light light = RenderPipeline.lights[i];

            //    mat.shader.SetMatrix4("uLightProjection", light.lightCamera.projectionMatrix);
            //    mat.shader.SetMatrix4("uLightView", light.lightCamera.viewMatrix);

            //    if (light.lightType == LightType.Directional)
            //        mat.shader.SetVector($"uLightPos{i}", new Vector4(light.transform.forward, 1));
            //    else
            //        mat.shader.SetVector($"uLightPos{i}", new Vector4(light.transform.position, 0));

            //    GL.ActiveTexture(TextureUnit.Texture0 + i);
            //    GL.BindTexture(TextureTarget.Texture2D, light.shadowTex);

            //    mat.shader.SetTexture($"uShadowTex{i}", 0);

            //    //GL.BindTexture(TextureTarget.Texture2D, 0);
            //}

            //mat.shader.SetVector("uViewPos", camera.transform.position);
            //mat.shader.SetFloat("uTime", Time.time);

            //mesh.Draw();

            Matrix4 tMatrix = ObjTransform == null ? modelMatrix : ObjTransform.WorldMatrix;
            foreach (var mesh in Model.Meshes) {
                //use material
                Material mat = mesh.Item2;

                if (mat == null)
                    continue;

                mat.Use();

                mat.Shader.SetMatrix4("uModel", tMatrix);
                mat.Shader.SetMatrix4("uView", camera.ViewMatrix);
                mat.Shader.SetMatrix4("uProjection", camera.ProjectionMatrix);

                //for (int i = 0; i < RenderPipeline.Lights.Count; i++) {
                //    Light light = RenderPipeline.Lights[i];

                //    mat.shader.SetMatrix4("uLightProjection", light.lightCamera.projectionMatrix);
                //    mat.shader.SetMatrix4("uLightView", light.lightCamera.viewMatrix);

                //    if (light.lightType == LightType.Directional)
                //        mat.shader.SetVector($"uLightPos{i}", new Vector4(light.transform.forward, 1));
                //    else
                //        mat.shader.SetVector($"uLightPos{i}", new Vector4(light.transform.position, 0));

                //    GL.ActiveTexture(TextureUnit.Texture0 + i);
                //    GL.BindTexture(TextureTarget.Texture2D, light.shadowTex);

                //    mat.shader.SetTexture($"uShadowTex{i}", 0);
                //}

                mat.Shader.SetVector("uViewPos", camera.ObjTransform.Position);
                mat.Shader.SetFloat("uTime", Time.ElapsedTime);

                mesh.Item1.Draw();
            }
        }
        public void Draw(Camera camera, Shader shader) {
            if (Model == null || (Model != null && Model.Meshes.Count == 0))
                return;

            //if (ObjTransform != null) {
            //    Matrix4.CreateScale(ObjTransform.LocalScale, out modelMatrix);
            //    modelMatrix *= Matrix4.CreateFromQuaternion(ObjTransform.rotation);
            //    modelMatrix *= Matrix4.CreateTranslation(ObjTransform.position);
            //}

            shader.Use();

            shader.SetMatrix4("uModel", ObjTransform == null ? modelMatrix : ObjTransform.WorldMatrix);
            shader.SetMatrix4("uView", camera.ViewMatrix);
            shader.SetMatrix4("uProjection", camera.ProjectionMatrix);

            shader.SetFloat("uTime", Time.ElapsedTime);

            foreach (var mesh in Model.Meshes) {
                mesh.Item1.Draw();
            }
        }
    }
}
