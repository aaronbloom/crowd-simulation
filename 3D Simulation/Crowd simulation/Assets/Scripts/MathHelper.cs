using UnityEngine;

namespace Assets.Scripts {
    internal static class MathHelper {
        public static float Difference(float a, float b) {
            return Mathf.Abs(a - b);
        }
    }
}
