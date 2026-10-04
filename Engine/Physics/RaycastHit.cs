using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Trees;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace KartPlusPlus.Physics {
    public struct RaycastHit : IRayHitHandler {
        public bool Hit { get; private set; }
        public RayData Ray { get; private set; }
        public Vector3 Point { get; private set; }
        public Vector3 Normal { get; private set; }
        public CollidableReference Collidable { get; private set; }
        public float Distance { get; private set; }
        private ulong layerMask;

        public RaycastHit(ulong layerMask) {
            this.layerMask = layerMask;
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable) {
            return PhysicsSim.CanCollide(layerMask, PhysicsSim.GetPhysicsLayerIndex(collidable));
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable, int childIndex) {
            return PhysicsSim.CanCollide(layerMask, PhysicsSim.GetPhysicsLayerIndex(collidable));
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRayHit(in RayData ray, ref float maximumT, float t, in Vector3 normal, CollidableReference collidable, int childIndex) {
            Ray = ray;
            Point = ray.Origin + ray.Direction * t;
            Normal = normal;
            Collidable = collidable;
            Distance = t;

            Hit = true;
        }
    }
}
