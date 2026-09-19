using OpenTK.Mathematics;

namespace KartPlusPlus.Physics {
    public partial class PhysicsSim {
        public bool Raycast(Vector3 origin, Vector3 direction, float maxDistance) {
            HitHandler hitHandler = default;
            simulation.RayCast((System.Numerics.Vector3)origin, (System.Numerics.Vector3)direction, maxDistance, ref hitHandler);

            return false;
        }
    }
}
