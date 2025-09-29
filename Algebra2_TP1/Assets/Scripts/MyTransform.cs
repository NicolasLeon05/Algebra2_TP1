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

        public void SetParent(MyTransform newParent)
        {
            parent?.children.Remove(this);

            parent = newParent;

            if (newParent != null && !newParent.children.Contains(this))
                newParent.AddChild(this);
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
