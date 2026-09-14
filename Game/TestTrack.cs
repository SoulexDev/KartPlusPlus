using KartPlusPlus.AssetManagement;
using KartPlusPlus.Engine;
using KartPlusPlus.Physics;
using KartPlusPlus.Renderer;
using KartPlusPlus.Utility;
using OpenTK.Mathematics;

namespace KartPlusPlus.Game {
    public class TestTrack {
        public static void Create() {
            EngineObject obj = EngineObjectFactory.Instantiate("Free Camera");
            obj.AddComponent<FreeCam>("FreeCam");

            ResourceLoader.LoadResource(out EngineObject trackModel, "Models/TestTrack.fbx");

            ResourceLoader.LoadResource(out Texture2D protoLight, "Textures/Prototype_Light.png");
            EngineObject kart = EngineObjectFactory.Instantiate("Kart");
            kart.Transform.Position = Vector3.UnitY * 10;
            kart.AddComponent<BoxCollider>("collider");
            Rigidbody rb = kart.AddComponent<Rigidbody>("rigidbody");
            kart.AddComponent<ModelRenderer>("model renderer").SetModel(CubeMesh.Generate(protoLight));

            EngineObject ground = EngineObjectFactory.Instantiate("Ground");
            ground.Transform.LocalScale = new Vector3(4, 0.2f, 4);
            BoxCollider groundCol = ground.AddComponent<BoxCollider>("collider");
        }
    }
}
