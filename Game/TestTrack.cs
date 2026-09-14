using KartPlusPlus.AssetManagement;
using KartPlusPlus.Engine;
using KartPlusPlus.Renderer;
using KartPlusPlus.Utility;
using OpenTK.Mathematics;

namespace KartPlusPlus.Game
{
    public class TestTrack
    {
        public static void Create()
        {
            EngineObject obj = EngineObjectFactory.Instantiate("Free Camera");
            obj.AddComponent<FreeCam>("FreeCam");

            ResourceLoader.LoadResource(out EngineObject trackModel, "Models/TestTrack.fbx");

            ResourceLoader.LoadResource(out Texture2D protoLight, "Textures/Prototype_Light.png");
            EngineObject kart = EngineObjectFactory.Instantiate("Kart");
            kart.AddComponent<ModelRenderer>("model renderer").SetModel(CubeMesh.Generate(protoLight));
            kart.Transform.Position = Vector3.UnitZ * 10;
        }
    }
}
