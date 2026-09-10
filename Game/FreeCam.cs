using KartPlusPlus.Engine;
using KartPlusPlus.Renderer;
using KartPlusPlus.UserInput;
using OpenTK.Mathematics;

namespace KartPlusPlus.Game {
    public class FreeCam : Component {
        //private Camera camera;
        private float mouseX, mouseY;
        private float moveSpeed = 16;
        public override void Init() {
            Camera.Main = EngineObject.AddComponent<Camera>("Camera").InitializeParameters(90f, 0.1f, 1000f);
        }
        public override void Update() {
            //look
            Vector2 mouseDelta = Input.GetMouseDelta();

            mouseX += mouseDelta.X * 360f;
            mouseY -= mouseDelta.Y * 360f;

            mouseY = MathHelper.Clamp(mouseY, -89f, 89f);

            ObjTransform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegToRad * mouseX)
                * Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegToRad * mouseY);

            //movement
            Vector3 moveVector = Vector3.Zero;
            moveVector += 
                ObjTransform.Forward * Input.GetVertical() + 
                ObjTransform.Right * Input.GetHorizontal() +
                ObjTransform.Up * Input.GetLateral();

            if (moveVector != Vector3.Zero) {
                moveVector.Normalize();

                ObjTransform.Position += moveVector * moveSpeed * Time.DeltaTime;
            }
        }
    }
}