using KartPlusPlus.Engine;
using KartPlusPlus.Physics;
using KartPlusPlus.Renderer;
using KartPlusPlus.UserInput;
using KartPlusPlus.Utility;
using OpenTK.Mathematics;

namespace KartPlusPlus.Game {
    public class KartController : Component {
        private EngineObject camObj;
        private Camera camera;
        private Rigidbody rb;
        private LayerMask ignoreMask;
        public override void Init() {
            camObj = EngineObjectFactory.Instantiate("camera");
            camera = camObj.AddComponent<Camera>("Camera").InitializeParameters(90f, 0.1f, 1000f);
            Camera.Main = camera;

            ObjTransform.Position = Vector3.UnitY * 2;

            EngineObject.AddComponent<BoxCollider>();
            rb = EngineObject.AddComponent<Rigidbody>();

            rb.LockRotationX = rb.LockRotationZ = true;

            EngineObject.PhysicsLayer = PhysicsSim.IndexFromName("player");
            ignoreMask = ~PhysicsSim.MaskFromNames("player");

            //PhysicsSim.ChangeFilter("default", "player", false);

            Console.WriteLine($"Default can collide with default? {PhysicsSim.CanCollide(0, 0)}");
            Console.WriteLine($"Default can collide with player? {PhysicsSim.CanCollide(0, EngineObject.PhysicsLayer)}");
            Console.WriteLine($"Player can collide with player? {PhysicsSim.CanCollide(EngineObject.PhysicsLayer, EngineObject.PhysicsLayer)}");
            Console.WriteLine($"Ignore mask can collide with default? {PhysicsSim.CanCollide(ignoreMask, 0)}");
            Console.WriteLine($"Ignore mask can collide with player? {PhysicsSim.CanCollide(ignoreMask, EngineObject.PhysicsLayer)}");

            //Console.WriteLine();
        }
        public override void PhysicsTick() {
            
        }
        public override void LateUpdate() {
            Vector3 forward = Quaternion.FromAxisAngle(Vector3.UnitY, ObjTransform.Rotation.ToEulerAngles().Y) * Vector3.UnitZ;
            camObj.Transform.Position = ObjTransform.Position - forward * 4 + Vector3.UnitY * 3;
            //camObj.Transform.Rotation = QuaternionExtensions.LookAt((camObj.Transform.Position - ObjTransform.Position).Normalized(), Vector3.UnitY);
            camObj.Transform.Rotation = QuaternionExtensions.LookAt(camObj.Transform.Position, ObjTransform.Position, Vector3.UnitY);
        }
    }
}
