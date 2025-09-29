using UnityEngine;
using CustomMath;

public class Main : MonoBehaviour
{
    public Mesh cubeMesh;
    public Mesh capsuleMesh;
    public Mesh cylinderMesh;

    [Header("Transforms")]
    public MyTransform cube;
    public MyTransform capsule;
    public MyTransform cylinder;

    void Start()
    {
        capsule.SetParent(cube);
        cylinder.SetParent(capsule);
    }

    private void OnValidate()
    {
        cube?.UpdateRotationFromEuler();
        capsule?.UpdateRotationFromEuler();
        cylinder?.UpdateRotationFromEuler();
    }

    void OnDrawGizmos()
    {
        if (cube == null || capsule == null || cylinder == null) return;

        //Cube
        Gizmos.color = Color.red;
        Gizmos.DrawMesh(
            cubeMesh,
            (Vector3)cube.position,
            (Quaternion)cube.rotation,
            (Vector3)cube.lossyScale
        );

        //Capsule
        Gizmos.color = Color.green;
        Gizmos.DrawMesh(
            capsuleMesh,
            (Vector3)capsule.position,
            (Quaternion)capsule.rotation,
            (Vector3)capsule.lossyScale
        );

        //Cylinder
        Gizmos.color = Color.blue;
        Gizmos.DrawMesh(
            cylinderMesh,
            (Vector3)cylinder.position,
            (Quaternion)cylinder.rotation,
            (Vector3)cylinder.lossyScale
        );
    }
}
