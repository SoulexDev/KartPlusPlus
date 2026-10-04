using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using System.Runtime.CompilerServices;

namespace KartPlusPlus.Physics {
    public struct DefaultNarrowPhaseCallbacks : INarrowPhaseCallbacks {
        public SpringSettings ContactSpringiness;
        public float MaximumRecoveryVelocity;
        public float FrictionCoefficient;
        public CollidableProperty<int> PhysicsLayers;

        public DefaultNarrowPhaseCallbacks(CollidableProperty<int> physicsLayers) {
            PhysicsLayers = physicsLayers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref float speculativeMargin) {
            bool mobilityCheck = a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
            bool filterCheck = PhysicsSim.CanCollide(PhysicsLayers[a], PhysicsLayers[b]);

            return mobilityCheck && filterCheck;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB) {
            bool mobilityCheck = pair.A.Mobility == CollidableMobility.Dynamic || pair.B.Mobility == CollidableMobility.Dynamic;
            bool filterCheck = PhysicsSim.CanCollide(PhysicsLayers[pair.A], PhysicsLayers[pair.B]);

            return mobilityCheck && filterCheck;
        }
        public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold> {
            pairMaterial.FrictionCoefficient = FrictionCoefficient;
            pairMaterial.MaximumRecoveryVelocity = MaximumRecoveryVelocity;
            pairMaterial.SpringSettings = ContactSpringiness;
            return true;
        }
        public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold) {
            return true;
        }
        public void Initialize(Simulation simulation) {
            PhysicsLayers.Initialize(simulation);
            if (ContactSpringiness.AngularFrequency == 0 && ContactSpringiness.TwiceDampingRatio == 0) {
                ContactSpringiness = new SpringSettings(30, 1);
                MaximumRecoveryVelocity = 2;
                FrictionCoefficient = 1;
            }
        }
        public void Dispose() {

        }
    }
}
