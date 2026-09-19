using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Trees;
using System.Numerics;

namespace KartPlusPlus.Physics {
    public class HitHandler : IRayHitHandler {
        public bool AllowTest(CollidableReference collidable) {
            return true;
        }
        public bool AllowTest(CollidableReference collidable, int childIndex) {
            return true;
        }
        public void OnRayHit(in RayData ray, ref float maximumT, float t, in Vector3 normal, CollidableReference collidable, int childIndex) {
            
        }
    }
}
