using KartPlusPlus.Engine;
using KartPlusPlus.Renderer;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace KartPlusPlus.AssetManagement {
    public class ResourceLoader {
        private static Dictionary<ShaderType, string> shaderFileExtensions = new Dictionary<ShaderType, string>
        {
            { ShaderType.VertexShader, ".vert" },
            { ShaderType.GeometryShader, ".geo" },
            { ShaderType.FragmentShader, ".frag" },
            { ShaderType.ComputeShader, ".comp" },
            { ShaderType.TessControlShader, ".tesc" },
            { ShaderType.TessEvaluationShader, ".tese" }
        };
        private static Dictionary<Assimp.TextureWrapMode, TextureWrapMode> texWrapModeBindings = new Dictionary<Assimp.TextureWrapMode, TextureWrapMode>
        {
            { Assimp.TextureWrapMode.Wrap, TextureWrapMode.Repeat },
            { Assimp.TextureWrapMode.Mirror, TextureWrapMode.MirroredRepeat },
            { Assimp.TextureWrapMode.Clamp, TextureWrapMode.ClampToBorder },
            { Assimp.TextureWrapMode.Decal, TextureWrapMode.ClampToBorder },
        };

        private static Assimp.AssimpContext importer = new Assimp.AssimpContext();

        //resources
        private static Dictionary<string, Texture2D> textureResources = new Dictionary<string, Texture2D>();
        private static Dictionary<string, Shader> shaderResources = new Dictionary<string, Shader>();

        public static bool LoadResource(out Texture2D texture, string filePath,
            TextureWrapMode textureWrapMode = TextureWrapMode.Repeat,
            TextureMinFilter minFilter = TextureMinFilter.Linear,
            TextureMagFilter magFilter = TextureMagFilter.Linear,
            bool generateMipMap = true) {
            if (textureResources.TryGetValue(filePath, out texture))
                return true;

            string preProcessFilePath = filePath;
            filePath = Path.Combine(KartPlusPlus.AssetsPath, filePath);

            Console.WriteLine("Loading texture " + filePath);

            texture = new Texture2D(filePath, textureWrapMode, minFilter, magFilter, generateMipMap);

            if (!textureResources.ContainsKey(preProcessFilePath))
                textureResources.Add(preProcessFilePath, texture);

            return true;
        }
        public static bool LoadResource(out Shader shader, params (string, ShaderType)[] fileInfo) {
            if (shaderResources.TryGetValue(fileInfo[0].Item1, out shader))
                return true;

            string preProcessFilePath = fileInfo[0].Item1;

            Console.WriteLine("Loading shader " + Path.Combine(KartPlusPlus.AssetsPath, fileInfo[0].Item1));

            (string, ShaderType)[] shaderInfo = new (string, ShaderType)[fileInfo.Length];

            for (int i = 0; i < fileInfo.Length; i++) {
                var sInfo = shaderInfo[i];

                string filePath = Path.Combine(KartPlusPlus.AssetsPath, fileInfo[i].Item1);

                sInfo.Item1 = File.ReadAllText(filePath + shaderFileExtensions[fileInfo[i].Item2]);
                sInfo.Item2 = fileInfo[i].Item2;

                if (sInfo.Item1 == string.Empty) {
                    shader = -1;
                    return false;
                }

                shaderInfo[i] = sInfo;
            }

            if (Shader.CreateShaderProgram(out int id, shaderInfo)) {
                shader = id;

                if (!shaderResources.ContainsKey(preProcessFilePath))
                    shaderResources.Add(preProcessFilePath, shader);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Shader Loaded!");
                Console.ForegroundColor = ConsoleColor.White;

                return true;
            }
            else {
                shader = -1;
                return false;
            }
        }
        public static bool LoadResource(out Model model, string filePath) {
            filePath = Path.Combine(KartPlusPlus.AssetsPath, filePath);

            Console.WriteLine("Loading model " + filePath);

            string fileType = Path.GetExtension(filePath);

            using (Stream stream = File.OpenRead(filePath)) {
                Assimp.PostProcessSteps processFlags =
                    Assimp.PostProcessSteps.Triangulate |
                    Assimp.PostProcessSteps.PreTransformVertices |
                    Assimp.PostProcessSteps.MakeLeftHanded;

                Assimp.Scene scene = importer.ImportFileFromStream(stream, processFlags, fileType);

                if (scene == null || ((scene.SceneFlags & Assimp.SceneFlags.Incomplete) != 0) || scene.RootNode == null) {
                    Console.WriteLine($"Failed to import model {filePath}");
                    model = null;
                    return false;
                }

                model = new Model();

                ProcessNode(filePath, scene.RootNode, scene, ref model);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Model Loaded!");
            Console.ForegroundColor = ConsoleColor.White;

            return true;
        }
        public static bool LoadResource(out List<ModelRenderer> modelRenderers, string filePath) {
            filePath = Path.Combine(KartPlusPlus.AssetsPath, filePath);

            Console.WriteLine("Loading model " + filePath);

            string fileType = Path.GetExtension(filePath);

            using (Stream stream = File.OpenRead(filePath)) {
                Console.WriteLine(fileType);
                Assimp.Scene scene = importer.ImportFileFromStream(stream, Assimp.PostProcessSteps.Triangulate, fileType);

                if (scene == null || ((scene.SceneFlags & Assimp.SceneFlags.Incomplete) != 0) || scene.RootNode == null) {
                    Console.WriteLine($"Failed to import model {filePath}");
                    modelRenderers = null;
                    return false;
                }

                modelRenderers = new List<ModelRenderer>();

                ProcessScene(filePath, scene.RootNode, scene, modelRenderers);
            }
            return true;
        }
        public static bool LoadResource(out EngineObject sceneObject, string filePath) {
            filePath = Path.Combine(KartPlusPlus.AssetsPath, filePath);

            Console.WriteLine("Loading model " + filePath);

            string fileType = Path.GetExtension(filePath);

            using (Stream stream = File.OpenRead(filePath)) {
                Console.WriteLine(fileType);
                Assimp.Scene scene = importer.ImportFileFromStream(stream, Assimp.PostProcessSteps.Triangulate, fileType);

                if (scene == null || ((scene.SceneFlags & Assimp.SceneFlags.Incomplete) != 0) || scene.RootNode == null) {
                    Console.WriteLine($"Failed to import model {filePath}");
                    sceneObject = null;
                    return false;
                }

                sceneObject = EngineObjectFactory.Instantiate(scene.RootNode.Name);

                ProcessScene(filePath, scene.RootNode, scene, sceneObject);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Model Loaded!");
            Console.ForegroundColor = ConsoleColor.White;

            return true;
        }
        private static void ProcessScene(string filePath, Assimp.Node node, Assimp.Scene scene, EngineObject parentObject) {
            EngineObject childObject = EngineObjectFactory.Instantiate(node.Name);
            node.Transform.Decompose(out Assimp.Vector3D scale, out Assimp.Quaternion rotation, out Assimp.Vector3D position);

            childObject.Transform.SetParent(parentObject.Transform);
            childObject.Transform.LocalPosition = new Vector3(position.X, position.Y, position.Z);
            childObject.Transform.LocalScale = new Vector3(scale.X, scale.Y, scale.Z);
            childObject.Transform.LocalRotation = new Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);

            Model model = new Model();

            ModelRenderer modelRenderer = childObject.AddComponent<ModelRenderer>("Model Renderer");
            modelRenderer.SetModel(model);

            for (int i = 0; i < node.MeshCount; i++) {
                Assimp.Mesh mesh = scene.Meshes[node.MeshIndices[i]];
                model.Meshes.Add(ProcessMesh(filePath, mesh, scene));
            }
            for (int i = 0; i < node.ChildCount; i++) {
                ProcessScene(filePath, node.Children[i], scene, childObject);
            }
        }
        private static void ProcessScene(string filePath, Assimp.Node node, Assimp.Scene scene, List<ModelRenderer> modelRenderers) {
            Assimp.Matrix4x4 mat4 = Assimp.Matrix4x4.FromScaling(new Assimp.Vector3D(0.01f));
            for (int i = 0; i < node.MeshCount; i++) {
                Assimp.Mesh mesh = scene.Meshes[node.MeshIndices[i]];
                Model model = new Model(ProcessMesh(filePath, mesh, scene));

                node.Transform *= mat4;
                node.Transform.Decompose(out Assimp.Vector3D scale, out Assimp.Quaternion rotation, out Assimp.Vector3D position);

                Matrix4 modelMatrix = Matrix4.CreateFromQuaternion(new Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W));
                modelMatrix *= Matrix4.CreateScale(new Vector3(scale.X, scale.Y, scale.Z));
                modelMatrix *= Matrix4.CreateTranslation(new Vector3(position.X, position.Y, position.Z));

                modelRenderers.Add(new ModelRenderer(model, Matrix4.Identity));
            }
            for (int i = 0; i < node.ChildCount; i++) {
                ProcessScene(filePath, node.Children[i], scene, modelRenderers);
            }
        }
        private static void ProcessNode(string filePath, Assimp.Node node, Assimp.Scene scene, ref Model model) {
            for (int i = 0; i < node.MeshCount; i++) {
                Assimp.Mesh mesh = scene.Meshes[node.MeshIndices[i]];
                model.Meshes.Add(ProcessMesh(filePath, mesh, scene));
            }
            for (int i = 0; i < node.ChildCount; i++) {
                ProcessNode(filePath, node.Children[i], scene, ref model);
            }
        }
        private static (Mesh, Material) ProcessMesh(string filePath, Assimp.Mesh assimpMesh, Assimp.Scene scene) {
            //float[] vertices = new float[mesh.VertexCount];
            List<float> vertices = new List<float>();
            List<int> indices = new List<int>();
            Material mat = new Material(DefaultResources.OpaqueShader);

            //VertexAttributeType types = 0;

            //types |= (assimpMesh.HasVertices ? VertexAttributeType.Vector3 : 0);
            //types |= (assimpMesh.HasNormals ? VertexAttributeType.Vector3 : 0);
            //types |= (assimpMesh.HasNormals ? VertexAttributeType.Vector3 : 0);

            //vertices
            for (int i = 0; i < assimpMesh.VertexCount; i++) {
                //position
                if (assimpMesh.HasVertices) {
                    Assimp.Vector3D position = assimpMesh.Vertices[i];
                    vertices.Add(position.X);
                    vertices.Add(position.Y);
                    vertices.Add(position.Z);
                }

                //normal
                if (assimpMesh.HasNormals) {
                    Assimp.Vector3D normal = assimpMesh.Normals[i];
                    vertices.Add(normal.X);
                    vertices.Add(normal.Y);
                    vertices.Add(normal.Z);
                }

                //uvs
                if (assimpMesh.HasTextureCoords(0)) {
                    Assimp.Vector3D texCoord = assimpMesh.TextureCoordinateChannels[0][i];
                    vertices.Add(texCoord.X);
                    vertices.Add(texCoord.Y);
                }

                //uvs
                //for (int t = 0; t < assimpMesh.TextureCoordinateChannelCount; t++)
                //{

                //}

                //vertex colors
                //for (int v = 0; v < assimpMesh.VertexColorChannelCount; v++)
                //{
                //    if (assimpMesh.HasVertexColors(v))
                //    {
                //        Assimp.Color4D color = assimpMesh.VertexColorChannels[v][i];
                //        vertices.Add(color.R);
                //        vertices.Add(color.G);
                //        vertices.Add(color.B);
                //        vertices.Add(color.A);
                //    }
                //}
            }

            //indices
            for (int i = 0; i < assimpMesh.FaceCount; i++) {
                Assimp.Face face = assimpMesh.Faces[i];
                for (int j = 0; j < face.IndexCount; j++) {
                    indices.Add(face.Indices[j]);
                }
            }

            //materials
            if (assimpMesh.MaterialIndex >= 0) {
                List<(string, Texture2D)> textures = new List<(string, Texture2D)>();

                Assimp.Material assimpMat = scene.Materials[assimpMesh.MaterialIndex];
                LoadMaterialTextures(filePath, textures, assimpMat, Assimp.TextureType.Diffuse, "uDiffuse");
                //LoadMaterialTextures(textures, assimpMat, Assimp.TextureType.Specular, "uSpecular");

                mat.Texture2Ds.AddRange(textures);
            }

            Mesh mesh = new Mesh(vertices, indices, PrimitiveType.Triangles,
                assimpMesh.HasBones ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw,
                VertexAttribute.Vector3, VertexAttribute.Vector3, VertexAttribute.Vector2);

            return (mesh, mat);
        }
        private static void LoadMaterialTextures(string filePath, List<(string, Texture2D)> textures, Assimp.Material mat, Assimp.TextureType type, string textureName) {
            for (int i = 0; i < mat.GetMaterialTextureCount(type); i++) {
                mat.GetMaterialTexture(type, i, out Assimp.TextureSlot assimpTexture);
                //TODO: update texture to allow per U/V wrap modes
                //Console.WriteLine(assimpTexture.FilePath);
                //Console.WriteLine(filePath);

                //string fileFolder = Path.GetDirectoryName(filePath);
                //string fileName = assimpTexture.FilePath.Replace(ASECore.RawPath, "");
                string texturePath = Path.Combine(Path.GetDirectoryName(filePath), assimpTexture.FilePath);

                Console.WriteLine(texturePath);
                LoadResource(out Texture2D tex, texturePath);

                //texWrapModeBindings[assimpTexture.WrapModeU]
                textures.Add((textureName + i, tex));
            }
        }
    }
}
