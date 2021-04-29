using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
 
public class FrustumTest : MonoBehaviour
{
    public Camera CulingCamera;
    public Renderer[] CullingTestObjects;
    private Plane[] planes;
    void OnEnable()
    {
        planes = new Plane[6];
    }
    void Update()
    {
        GeometryUtility.CalculateFrustumPlanes(CulingCamera, planes);
        for (var index = 0; index < CullingTestObjects.Length; index++)
        {
            var bounds = CullingTestObjects[index].bounds;
            var result = GeometryUtility.TestPlanesAABB(planes, bounds);
            CullingTestObjects[index].enabled = result;
        }
    }
    [MenuItem("Test/Create")]
    static void Create()
    {
        var gos = new List<GameObject>();
        var root = new GameObject("Root").transform;
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                for (var k = 0; k < 10; k++)
                {
                    var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.position = new Vector3(i, j, k) * 2;
                    go.transform.parent = root;
                    gos.Add(go);
                }
            }
        }
        var test = new GameObject("FrustumTest").AddComponent<FrustumTest>();
        test.CulingCamera = Camera.main;
        test.CullingTestObjects = gos.Select(item => item.GetComponent<Renderer>()).ToArray();
    }
}