using KartPlusPlus.Engine;
using OpenTK.Mathematics;

namespace KartPlusPlus.Renderer {
    public enum CameraProjectionType { Perspective, Orthographic }
    public sealed class Camera : Component {
        public static Camera Main;

        public float FOV;
        public float OrthoWidth;
        public float OrthoHeight;
        public float NearPlane;
        public float FarPlane;
        public CameraProjectionType CameraProjectionType;

        public Matrix4 ViewMatrix;
        public Matrix4 ProjectionMatrix;

        public override void Init() {
            FOV = 90.0f;
            OrthoWidth = 16.0f;
            OrthoHeight = 9.0f;
            NearPlane = 0.1f;
            FarPlane = 1000.0f;

            CameraProjectionType = CameraProjectionType.Perspective;
        }
        public Camera InitializeParameters(float fov = 90.0f, float nearPlane = 0.1f, float farPlane = 1000.0f) {
            FOV = fov;
            NearPlane = nearPlane;
            FarPlane = farPlane;

            CameraProjectionType = CameraProjectionType.Perspective;

            return this;
        }
        public Camera InitializeParameters(float orthoWidth = 16.0f, float orthoHeight = 9.0f, float nearPlane = 0.1f, float farPlane = 1000.0f) {
            OrthoWidth = orthoWidth;
            OrthoHeight = orthoHeight;
            NearPlane = nearPlane;
            FarPlane = farPlane;

            CameraProjectionType = CameraProjectionType.Orthographic;

            return this;
        }
        public void SetMatrices() {
            ViewMatrix = Matrix4.LookAt(ObjTransform.Position, ObjTransform.Position + ObjTransform.Forward, Vector3.UnitY);

            switch (CameraProjectionType) {
                case CameraProjectionType.Perspective:
                    ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegToRad * FOV, 
                        (float)RenderPipeline.ScreenWidth / RenderPipeline.ScreenHeight, NearPlane, FarPlane);
                    break;
                case CameraProjectionType.Orthographic:
                    ProjectionMatrix = Matrix4.CreateOrthographic(OrthoWidth, OrthoHeight, NearPlane, FarPlane);
                    break;
                default:
                    break;
            }
        }
    }
}
