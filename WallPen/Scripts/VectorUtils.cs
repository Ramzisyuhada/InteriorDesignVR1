using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace WallPen
{
    public static class VectorUtils
    {
        /// <summary>
        /// Gets the direction from one vector to another (normalized).
        /// </summary>
        /// <param name="from">The position we are at</param>
        /// <param name="to">The position we want to point at</param>
        /// <returns></returns>
        public static Vector3 GetDirection(Vector3 from, Vector3 to, bool normalized = true)
        {
            if (normalized)
                return (to - from).normalized;
            else
                return (to - from);
        }

        public static Vector3 GetDirection(Vector3Int from, Vector3Int to)
        {
            return (to - from);
        }

        public static Vector3 GetDirection(Vector2Int from, Vector2Int to)
        {
            Vector3 final = new Vector3(to.x - from.x, 0, to.y - from.y);
            return final.normalized;
        }

        public enum VectorAxis
        {
            X,
            Y,
            Z
        }

        public static Vector3 Set(Vector3 vec, VectorAxis axis, float newValue)
        {
            if (axis == VectorAxis.X)
                vec.x = newValue;
            if (axis == VectorAxis.Y)
                vec.y = newValue;
            if (axis == VectorAxis.Z)
                vec.z = newValue;

            return vec;
        }

        #region Swizzling
        /// <summary>
        /// The X and Z of a vector
        /// </summary>
        /// <returns></returns>
        public static Vector3 xz(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }

        /// <summary>
        /// The X and Y of a vector
        /// </summary>
        /// <returns></returns>
        public static Vector3 xy(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        /// <summary>
        /// The Y and Z of a vector
        /// </summary>
        /// <returns></returns>
        public static Vector3 yz(this Vector3 vector)
        {
            return new Vector3(0, vector.y, vector.z);
        }
        #endregion

        public static float GetVelocityInDirection(Vector3 vel, Vector3 dir)
        {
            return vel.magnitude * Mathf.Clamp01(Vector3.Dot(vel.normalized, dir));
        }

        public static float Scale(Vector3 vel, Vector3 dir)
        {
            return vel.magnitude * Mathf.Clamp01(Vector3.Dot(vel.normalized, dir));
        }

        public static Vector3Int RoundToInt(this Vector3 vec)
        {
            return new Vector3Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
        }

        public static Vector3 Multiply(Vector3 one, Vector3 two)
        {
            return new Vector3(one.x * two.x, one.y * two.y, one.z * two.z);
        }

        public static Vector3 Absolute(Vector3 vec)
        {
            return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
        }

        public static Vector3 RandomVector(float variance = 1)
        {
            return new Vector3(Random.Range(-variance, variance), Random.Range(-variance, variance), Random.Range(-variance, variance)).normalized;
        }

        public static Vector3 Center(Vector3 one, Vector3 two)
        {
            Vector3 vec = GetDirection(one, two, false);
            return vec / 2f;
        }

        public static Vector2 Center(Vector2Int one, Vector2Int two)
        {
            Vector2 new1 = new Vector2(one.x, one.y);
            Vector2 new2 = new Vector2(two.x, two.y);
            return Vector2.Lerp(new1, new2, 0.5f);
        }

        public static Vector3 GridToWorld(Vector2Int pos)
        {
            return new Vector3(pos.x, 0, pos.y);
        }

        public static Vector3 GridToWorld(Vector2 pos)
        {
            return new Vector3(pos.x, 0, pos.y);
        }

        public static Vector2Int WorldToXZGrid(Vector2 pos)
        {
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        }

        public static Vector3 Divide(Vector3 one, Vector3 two)
        {
            Vector3 v = new Vector3(one.x / two.x, one.y / two.y, one.z / two.z);
            return v;
        }

        public static Vector3 toVec3(this Vector2Int dir)
        {
            Vector3 vec = new Vector3(dir.x, 0, dir.y);
            return vec;
        }

        public static Vector3 AverageDirection(Vector3[] dirs)
        {
            Vector3 targetAlignment = Vector3.zero;
            for (int i = 0; i < dirs.Length; i++)
                targetAlignment += dirs[i];

            targetAlignment.Normalize();

            return targetAlignment;
        }

        public static Vector3 AverageDirection(Vector2Int[] dirs)
        {
            Vector2 targetAlignment = Vector2.zero;
            for (int i = 0; i < dirs.Length; i++)
                targetAlignment += dirs[i];

            targetAlignment.Normalize();

            return targetAlignment;
        }

        public static Vector2 CenterPoint(Vector2Int[] positions)
        {
            Vector2 center = new Vector2(0, 0);
            var numPoints = positions.Length;
            foreach (Vector2 point in positions)
            {
                center += point;
            }

            center /= numPoints;

            return center;
        }

        public static float IntersectionThickness(Bounds b, Ray r)
        {
            float x = r.direction.x;
            if (r.direction.x < 0.01f && r.direction.x > -0.01f)
                x = 0.01f;
            float y = r.direction.y;
            if (r.direction.y < 0.01f && r.direction.y > -0.01f)
                y = 0.01f;
            float z = r.direction.z;
            if (r.direction.z < 0.01f && r.direction.z > -0.01f)
                z = 0.01f;
            Vector3 dir = new Vector3(x, y, z);
            Vector3 t1 = Divide((b.min - r.origin), dir);
            Vector3 t2 = Divide((b.max - r.origin), dir);

            float tMin = Mathf.Max(Mathf.Max(Mathf.Min(t1.x, t2.x), Mathf.Min(t1.y, t2.y)), Mathf.Min(t1.z, t2.z));
            float tMax = Mathf.Min(Mathf.Min(Mathf.Max(t1.x, t2.x), Mathf.Max(t1.y, t2.y)), Mathf.Max(t1.z, t2.z));

            float thickness = tMax - tMin;
            return thickness;
        }

        public static Vector3 ApplyFriction(Vector3 vec, float friction)
        {
            vec.x /= 1 + friction * Time.deltaTime;
            vec.z /= 1 + friction * Time.deltaTime;
            return vec;
        }

        public static Vector3 SnapDirectionToMainVectors(Vector3 vec)
        {
            Vector3[] dirs = new Vector3[4] { Vector3.forward, -Vector3.forward, Vector3.right, -Vector3.right };
            return dirs.OrderBy(x => Vector3.Dot(vec, x)).FirstOrDefault();
        }
    }
}
