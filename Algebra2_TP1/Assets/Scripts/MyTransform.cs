using System.Collections.Generic;
using UnityEngine;

namespace CustomMath
{
    [System.Serializable]
    public class MyTransform
    {
         public Vec3 localPosition = Vec3.Zero;
        public Vec3 localScale = Vec3.One;

        [SerializeField] private Vector3 localEuler;
        [System.NonSerialized] public MyQuat localRotation = MyQuat.identity;

        [System.NonSerialized] public MyTransform parent;
        [System.NonSerialized] private List<MyTransform> children = new List<MyTransform>();


        public MyTransform()
        {
            localPosition = Vec3.Zero;
            localRotation = MyQuat.identity;
            localScale = Vec3.One;
            children = new List<MyTransform>();
        }


        public Vec3 position
        {
            get
            { return localToWorldMatrix.MultiplyPoint(Vec3.Zero); }
        }

        public MyQuat rotation
        {
            get
            {
                if (parent != null)
                    return parent.rotation * localRotation;
                return localRotation;
            }
        }

        public Vec3 lossyScale
        {
            get
            {
                if (parent != null)
                {
                    Vec3 pScale = parent.lossyScale;
                    return new Vec3(
                        localScale.x * pScale.x,
                        localScale.y * pScale.y,
                        localScale.z * pScale.z
                    );
                }
                return localScale;
            }
        }

        public MyMatrix4x4 localToWorldMatrix
        {
            get
            {
                MyMatrix4x4 local = MyMatrix4x4.TRS(localPosition, localRotation, localScale);
                if (parent != null)
                    return parent.localToWorldMatrix * local;
                return local;
            }
        }

        public MyMatrix4x4 worldToLocalMatrix
        {
            get
            {
                return Invert(localToWorldMatrix);
            }
        }

        public void SetParent(MyTransform newParent)
        {
            if (parent != null)
                parent.children.Remove(this);

            parent = newParent;

            if (newParent != null && !newParent.children.Contains(this))
                newParent.children.Add(this);
        }

        public void AddChild(MyTransform child)
        {
            if (!children.Contains(child))
            {
                children.Add(child);
                child.parent = this;
            }
        }

        public void UpdateRotationFromEuler()
        {
            localRotation = MyQuat.Euler(localEuler.x, localEuler.y, localEuler.z);

            foreach (var child in children)
                child.UpdateRotationFromEuler();
        }

        public Vec3 TransformPoint(Vec3 point)
        {
            return localToWorldMatrix.MultiplyPoint(point);
        }

        public Vec3 InverseTransformPoint(Vec3 point)
        {
            return worldToLocalMatrix.MultiplyPoint(point);
        }

        public Vec3 TransformDirection(Vec3 dir)
        {
            return localToWorldMatrix.MultiplyVector(dir);
        }

        public Vec3 InverseTransformDirection(Vec3 dir)
        {
            return worldToLocalMatrix.MultiplyVector(dir);
        }


        private MyMatrix4x4 Invert(MyMatrix4x4 m)
        {
            Matrix4x4 unityMat = new Matrix4x4();
            unityMat.m00 = m.m00; unityMat.m01 = m.m01; unityMat.m02 = m.m02; unityMat.m03 = m.m03;
            unityMat.m10 = m.m10; unityMat.m11 = m.m11; unityMat.m12 = m.m12; unityMat.m13 = m.m13;
            unityMat.m20 = m.m20; unityMat.m21 = m.m21; unityMat.m22 = m.m22; unityMat.m23 = m.m23;
            unityMat.m30 = m.m30; unityMat.m31 = m.m31; unityMat.m32 = m.m32; unityMat.m33 = m.m33;

            unityMat = unityMat.inverse;

            return new MyMatrix4x4(
                unityMat.m00, unityMat.m01, unityMat.m02, unityMat.m03,
                unityMat.m10, unityMat.m11, unityMat.m12, unityMat.m13,
                unityMat.m20, unityMat.m21, unityMat.m22, unityMat.m23,
                unityMat.m30, unityMat.m31, unityMat.m32, unityMat.m33
            );
        }
    }
}
