using KartPlusPlus.AssetManagement;
using KartPlusPlus.Engine;
using KartPlusPlus.Physics;
using KartPlusPlus.Renderer;
using KartPlusPlus.Utility;
using OpenTK.Mathematics;

namespace KartPlusPlus.Game {
    public class TestTrack {
        public static void Create() {
            //EngineObject obj = EngineObjectFactory.Instantiate("Free Camera");
            //obj.AddComponent<FreeCam>("FreeCam");

            PhysicsSim.AddLayer("player");

            ResourceLoader.LoadResource(out Model trackModel, "Models/TestTrack.fbx");
            ResourceLoader.LoadResource(out Texture2D protoLight, "Textures/Prototype_Light.png");

            EngineObject track = EngineObjectFactory.Instantiate("track");
            track.AddComponent<ModelRenderer>().SetModel(trackModel);
            track.AddComponent<MeshCollider>().SetMesh(trackModel.Meshes[0].Item1);

            //EngineObject floor = EngineObjectFactory.Instantiate("floor");
            //floor.AddComponent<ModelRenderer>().SetModel(CubeMesh.Generate(protoLight));
            //floor.Transform.LocalScale = new Vector3(10, 0.25f, 10);
            //floor.AddComponent<BoxCollider>();

            Model cubeModel = CubeMesh.Generate(protoLight);

            EngineObject kart = EngineObjectFactory.Instantiate("kart");
            kart.AddComponent<ModelRenderer>().SetModel(cubeModel);
            kart.AddComponent<KartController>();
            kart.Transform.LocalScale = new Vector3(1.5f, 1, 2);

            //EngineObject freeCam = EngineObjectFactory.Instantiate("freecam");
            //freeCam.AddComponent<FreeCam>();

            //for (int i = 0; i < 8; i++) {
            //    EngineObject box = EngineObjectFactory.Instantiate($"box {i}");
            //    box.Transform.Position = Vector3.UnitY * (1 + i) * 3 + KRandom.InSphere(1);
            //    box.AddComponent<BoxCollider>();
            //    Rigidbody rb = box.AddComponent<Rigidbody>();
            //    box.AddComponent<ModelRenderer>().SetModel(cubeModel);
            //}
        }
    }
}
