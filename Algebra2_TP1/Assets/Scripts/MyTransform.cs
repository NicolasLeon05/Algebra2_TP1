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
                if (parent == null)
                    return MyMatrix4x4.identity;

                // Inversa TRS simple (sin shear)
                Vec3 invScale = new Vec3(
                    1f / parent.lossyScale.x,
                    1f / parent.lossyScale.y,
                    1f / parent.lossyScale.z
                );

                MyQuat invRot = new MyQuat(
                    -parent.rotation.x,
                    -parent.rotation.y,
                    -parent.rotation.z,
                    parent.rotation.w
                );

                return
                    MyMatrix4x4.Scale(invScale) *
                    MyMatrix4x4.Rotate(invRot) *
                    MyMatrix4x4.Translate(-parent.position);
            }
        }


        public void SetParent(MyTransform newParent, bool worldPositionStays = true)
        {
            Vec3 worldPos = position;
            MyQuat worldRot = rotation;
            Vec3 worldScale = lossyScale;

            parent?.children.Remove(this);
            parent = newParent;

            if (newParent != null && !newParent.children.Contains(this))
                newParent.children.Add(this);

            if (worldPositionStays)
            {
                if (parent == null)
                {
                    localPosition = worldPos;
                    localRotation = worldRot;
                    localScale = worldScale;
                }
                else
                {
                    MyMatrix4x4 worldToLocal = parent.worldToLocalMatrix;

                    localPosition = worldToLocal.MultiplyPoint(worldPos);
                    localRotation = MyQuat.Normalize(
                        new MyQuat(
                            Quaternion.Inverse((Quaternion)parent.rotation) *
                            (Quaternion)worldRot
                        )
                    );

                    Vec3 pScale = parent.lossyScale;
                    localScale = new Vec3(
                        worldScale.x / pScale.x,
                        worldScale.y / pScale.y,
                        worldScale.z / pScale.z
                    );
                }
            }
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
    }
}
