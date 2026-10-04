using System.Runtime.CompilerServices;

namespace KartPlusPlus.Physics {
    public class LayerMask {
        public ulong Mask;

        public LayerMask() {
            Mask = ulong.MaxValue;
        }
        public LayerMask(ulong mask) {
            Mask = mask;
        }
        public void SetBit(int bitIndex, bool bit) {
            int reverseIndex = PhysicsSim.LayerCount - 1 - bitIndex;

            ulong bitmask = bit ? 1ul << reverseIndex : ~(1ul << reverseIndex);
            Mask &= bitmask;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LayerMask LayerFromName(string layerName) {
            return PhysicsSim.LayerFromName(layerName);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexFromName(string layerName) {
            return PhysicsSim.IndexFromName(layerName);
        }
        public static implicit operator ulong(LayerMask layer) => layer.Mask;
        public static implicit operator LayerMask(ulong value) => new LayerMask(value);
    }
}