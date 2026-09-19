using OpenTK.Mathematics;

namespace KartPlusPlus.Engine {
    public class KRandom {
        private static readonly Random random = new Random(System.DateTime.Now.Millisecond);
        public static int Range(int minInclusive, int maxExclusive) {
            return random.Next(minInclusive, maxExclusive);
        }
        public static float Range(float minInclusive, float maxInclusive) {
            //if min = 3, max = 5.5
            //[0,1] * 5.5 = [0,5.5]
            //[0,1] * (5.5 - 3 = 2.5) = [0, 2.5]
            //[0,2.5] + 3 = [3, 5.5]
            float randomValue = random.NextSingle();
            randomValue *= (maxInclusive - minInclusive);

            return randomValue + minInclusive;
        }
        public static Vector2 InCircle(float radius) {
            float randomRotation = random.NextSingle() * MathHelper.TwoPi;
            float randomLength = random.NextSingle() * radius;
            return new Vector2(
                (float)MathHelper.Cos(randomRotation),
                (float)MathHelper.Sin(randomRotation)) * randomLength;
        }
        public static Vector3 InSphere(float radius) {
            float u = random.NextSingle();
            float x = random.NextSingle() * 2 - 1;
            float y = random.NextSingle() * 2 - 1;
            float z = random.NextSingle() * 2 - 1;

            float magnitude = (float)Math.Sqrt(x * x + y * y + z * z);
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;

            float c = (float)Math.Cbrt(u);

            return new Vector3(x * c, y * c, z * c);
        }
        public static Vector3 InBox(float sizeX, float sizeY, float sizeZ) {
            return new Vector3(
                Range(sizeX * -0.5f, sizeX * 0.5f),
                Range(sizeY * -0.5f, sizeY * 0.5f),
                Range(sizeZ * -0.5f, sizeZ * 0.5f));
        }
    }
}
