using UnityEngine;


namespace MonkeyJam.Util {
    public static class ExtensionMethods {

        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null) {
            return new Vector2(x.HasValue ? (float)x : vector.x, y.HasValue ? (float)y : vector.y);
        }

        public static Vector2 Add(this Vector2 vector, float? x = null, float? y = null) {
            return vector + new Vector2(x.HasValue ? (float)x : 0, y.HasValue ? (float)y : 0);
        }
    }
}