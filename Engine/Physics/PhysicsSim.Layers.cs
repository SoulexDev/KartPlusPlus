using System.Runtime.CompilerServices;

//layer system
//-each object has a layer
//-layers can be represented as an individual bit in a mask
//-example collision matrix
//[0][1][2]  [ ]

//[1][1][1]  [0]
//[ ][1][1]  [1]
//[ ][ ][1]  [2]
//
//[0] = 100
//[1] = 010
//[2] = 001
//
//value = 1 >> (count - index - 1)
//[4] 10000 & [1] 11110 = 11110

namespace KartPlusPlus.Physics {
    public partial class PhysicsSim {
        public static int LayerCount => layers.Count;
        private static List<LayerMask> layers;
        private static List<string> layerNames;
        public static void AddLayer(string layerName) {
            layers.Add(new LayerMask());
            layerNames.Add(layerName);
        }
        public static void ChangeFilter(string layerName, string otherLayerName, bool bit) {
            //get the layer from its name, change its mask using the layer index of the other layer
            int aIndex = IndexFromName(layerName);
            int bIndex = IndexFromName(otherLayerName);

            layers[bIndex].SetBit(aIndex, bit);
            layers[aIndex].SetBit(bIndex, bit);
        }
        public static LayerMask MaskFromNames(params string[] layerNames) {
            ulong mask = 0ul;
            for (int i = 0; i < layerNames.Length; i++) {
                ulong bitToSet = GetTestBit(IndexFromName(layerNames[i]));
                mask |= bitToSet;
            }

            return mask;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static LayerMask LayerFromName(string layerName) {
            return layers[IndexFromName(layerName)];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexFromName(string layerName) {
            int index = layerNames.IndexOf(layerName);

            if (index == -1) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Layer {layerName} does not exist.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return index;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanCollide(int aIndex, int bIndex) {
            //a and bIndex are the indices of the layers that are being tested

            if (aIndex > bIndex) {
                ulong otherBit = GetTestBit(aIndex);
                return (layers[aIndex].Mask & otherBit) == otherBit;
            }
            else {
                ulong otherBit = GetTestBit(bIndex);
                return (layers[aIndex].Mask & otherBit) == otherBit;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanCollide(ulong layerMask, int layerIndex) {
            ulong bit = GetTestBit(layerIndex);
            return (bit & layerMask) == bit;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong GetTestBit(int layerIndex) {
            return 1ul << (layers.Count - 1 - layerIndex);
        }
    }
}
