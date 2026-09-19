using OpenTK.Mathematics;

namespace KartPlusPlus.Utility {
    public static class QuaternionExtensions {
        public static Quaternion LookAt(Vector3 forward, Vector3 up) {
            Matrix4 mat = Matrix4.LookAt(Vector3.Zero, -forward, up);

            return Quaternion.FromMatrix(new Matrix3(mat));
        }
        public static Quaternion LookAt(Vector3 eye, Vector3 target, Vector3 up) {
            Matrix4 mat = Matrix4.LookAt(target, eye, up);

            return Quaternion.FromMatrix(new Matrix3(mat));
        }
    }
}
