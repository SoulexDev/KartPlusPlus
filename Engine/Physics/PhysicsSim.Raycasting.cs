using OpenTK.Mathematics;

namespace KartPlusPlus.Physics {
    public partial class PhysicsSim {
        public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, out RaycastHit raycastHit, LayerMask layerMask) {
            raycastHit = new RaycastHit(layerMask);
            simulation.RayCast((System.Numerics.Vector3)origin, (System.Numerics.Vector3)direction, maxDistance, ref raycastHit);
            
            return raycastHit.Hit;
        }
    }
}
