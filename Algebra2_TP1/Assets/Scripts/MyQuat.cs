using System;
using Unity.Mathematics;
using UnityEngine;

namespace CustomMath
{
    [Serializable]
    public struct MyQuat
    {
        #region Variables
        public float x;
        public float y;
        public float z;
        public float w;

        public float this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public MyQuat normalized
        {
            get
            {
                MyQuat copy = this;
                copy.Normalize();
                return copy;
            }
        }

        public Vector3 eulerAngles
        {
            get
            {
                Quaternion unityQ = this;
                return unityQ.eulerAngles;
            }
            set
            {
                Quaternion unityQ = Quaternion.Euler(value);
                x = unityQ.x;
                y = unityQ.y;
                z = unityQ.z;
                w = unityQ.w;
            }
        }
        #endregion

        #region Constants
        public const float kEpsilon = 1e-06f;
        public static MyQuat identity { get { return new MyQuat(0f, 0f, 0f, 1f); } }
        #endregion

        #region Constructors
        public MyQuat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public MyQuat(Quaternion q)
        {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }
        #endregion

        #region Operators
        public static MyQuat operator *(MyQuat lhs, MyQuat rhs)
        {
            return new MyQuat(
            lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
            lhs.w * rhs.y - lhs.x * rhs.z + lhs.y * rhs.w + lhs.z * rhs.x,
            lhs.w * rhs.z + lhs.x * rhs.y - lhs.y * rhs.x + lhs.z * rhs.w,
            lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z
            );
        }

        public static Vector3 operator *(MyQuat rotation, Vector3 point)
        {
            MyQuat qPoint = new MyQuat(point.x, point.y, point.z, 0f);
            MyQuat qConjugate = new MyQuat(-rotation.x, -rotation.y, -rotation.z, rotation.w);

            MyQuat result = rotation * qPoint * qConjugate;
            return new Vector3(result.x, result.y, result.z);
        }

        public static bool operator ==(MyQuat lhs, MyQuat rhs)
        {
            return (lhs.x == rhs.x &&
                lhs.y == rhs.y &&
                lhs.z == rhs.z &&
                lhs.w == rhs.w);
        }

        public static bool operator !=(MyQuat lhs, MyQuat rhs)
        {
            return (lhs.x != rhs.x ||
                lhs.y != rhs.y ||
                lhs.z != rhs.z ||
                lhs.w != rhs.w);
        }

        public static implicit operator Quaternion(MyQuat q)
        {
            return new Quaternion(q.x, q.y, q.z, q.w);
        }

        public static implicit operator MyQuat(Quaternion q)
        {
            return new MyQuat(q.x, q.y, q.z, q.w);
        }
        #endregion

        #region Functions
        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        public void Normalize()
        {
            float magnitude = math.sqrt(x * x + y * y + z * z + w * w);
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;
            w /= magnitude;
        }

        public static MyQuat Normalize(MyQuat q)
        {
            MyQuat normalized = q;
            normalized.Normalize();
            return normalized;

        }

        public void ToAngleAxis(out float angle, out Vector3 axis)
        {
            throw new NotImplementedException();
        }

        public void SetLookRotation(Vector3 view)
        {
            throw new NotImplementedException();
        }

        public void SetLookRotation(Vector3 view, Vector3 up)
        {
            throw new NotImplementedException();
        }

        public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            throw new NotImplementedException();
        }

        public static MyQuat FromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            throw new NotImplementedException();
        }

        public static MyQuat LookRotation(Vector3 forward, Vector3 upwards)
        {
            throw new NotImplementedException();
        }

        public static MyQuat LookRotation(Vector3 forward)
        {
            throw new NotImplementedException();
        }

        public static MyQuat RotateTowards(MyQuat from, MyQuat to, float maxDegreesDelta)
        {
            throw new NotImplementedException();
        }

        public static MyQuat Inverse(MyQuat rotation)
        {
            throw new NotImplementedException();
        }

        public static MyQuat AngleAxis(float angle, Vector3 axis)
        {
            Vector3 normAxis = axis.normalized;
            float rad = angle * Mathf.Deg2Rad * 0.5f;

            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);

            return new MyQuat(
                normAxis.x * sin,
                normAxis.y * sin,
                normAxis.z * sin,
                cos
            );
        }

        public static MyQuat Euler(float x, float y, float z)
        {
            float radX = x * Mathf.Deg2Rad * 0.5f;
            float radY = y * Mathf.Deg2Rad * 0.5f;
            float radZ = z * Mathf.Deg2Rad * 0.5f;

            float sinX = Mathf.Sin(radX);
            float cosX = Mathf.Cos(radX);
            float sinY = Mathf.Sin(radY);
            float cosY = Mathf.Cos(radY);
            float sinZ = Mathf.Sin(radZ);
            float cosZ = Mathf.Cos(radZ);

            MyQuat q;
            q.w = cosX * cosY * cosZ + sinX * sinY * sinZ;
            q.x = sinX * cosY * cosZ - cosX * sinY * sinZ;
            q.y = cosX * sinY * cosZ + sinX * cosY * sinZ;
            q.z = cosX * cosY * sinZ - sinX * sinY * cosZ;

            return Normalize(q);
        }

        public static MyQuat Euler(Vector3 euler)
        {
            throw new NotImplementedException();
        }

        public static float Angle(MyQuat a, MyQuat b)
        {
            throw new NotImplementedException();
        }

        public static float Dot(MyQuat a, MyQuat b)
        {
            throw new NotImplementedException();
        }

        public static MyQuat Lerp(MyQuat a, MyQuat b, float t)
        {
            throw new NotImplementedException();
        }

        public static MyQuat LerpUnclamped(MyQuat a, MyQuat b, float t)
        {
            throw new NotImplementedException();
        }

        public static MyQuat Slerp(MyQuat a, MyQuat b, float t)
        {
            throw new NotImplementedException();
        }

        public static MyQuat SlerpUnclamped(MyQuat a, MyQuat b, float t)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internals
        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }

        public override bool Equals(object other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(MyQuat other)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
