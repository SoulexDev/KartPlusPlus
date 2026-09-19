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
        public override void Init() {
            camObj = EngineObjectFactory.Instantiate("camera");
            camera = camObj.AddComponent<Camera>("Camera").InitializeParameters(90f, 0.1f, 1000f);
            Camera.Main = camera;

            ObjTransform.Position = Vector3.UnitY * 2;

            EngineObject.AddComponent<BoxCollider>();
            rb = EngineObject.AddComponent<Rigidbody>();

            rb.LockRotationX = rb.LockRotationY = rb.LockRotationZ = true;
            //rb.UseGravity = false;
        }
        public override void PhysicsTick() {
            rb.AngularVelocity = Vector3.Zero;

            Vector3 force = ObjTransform.Forward * Input.GetVertical() * 25 * Time.FixedDeltaTime;
            if (force != Vector3.Zero)
                rb.AddForce(force);
        }
        public override void LateUpdate() {
            camObj.Transform.Position = ObjTransform.Position + ObjTransform.Forward * 4 + Vector3.UnitY * 3;
            //camObj.Transform.Rotation = QuaternionExtensions.LookAt((camObj.Transform.Position - ObjTransform.Position).Normalized(), Vector3.UnitY);
            camObj.Transform.Rotation = QuaternionExtensions.LookAt(camObj.Transform.Position, ObjTransform.Position, Vector3.UnitY);
        }
    }
}
