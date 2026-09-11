using KartPlusPlus.AssetManagement;
using KartPlusPlus.Renderer;
using OpenTK.Graphics.OpenGL4;

namespace KartPlusPlus.Engine {
    public class DefaultResources {
        public static Shader OpaqueShader;
        public static Material OpaqueMaterial;
        //public static Shader TransparentShader;

        public static void Load() {
            ResourceLoader.LoadResource(out OpaqueShader, 
                ("shaders/simple_lit", ShaderType.VertexShader),
                ("shaders/simple_lit", ShaderType.FragmentShader));

            OpaqueMaterial = new Material(OpaqueShader);
            //ResourceLoader.LoadResource(out TransparentShader,
            //    ("shaders/simple_unlit", ShaderType.VertexShader),
            //    ("shaders/simple_unlit", ShaderType.FragmentShader));
        }
    }
}