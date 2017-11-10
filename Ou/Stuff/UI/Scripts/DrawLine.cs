using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Linedata
{
    public List<Vector3> verts;
    public List<int> triganles;

    public Linedata()
    {
        triganles = new List<int>();
        verts = new List<Vector3>();
    }
}
public class DrawLine : MonoBehaviour
{
    private Linedata datas;
    private CanvasRenderer canvasRenderer;
    public Material material;
    private Mesh mesh;
    #region Calc

    void Awake()
    {
        canvasRenderer = gameObject.AddComponent<CanvasRenderer>();
        canvasRenderer.SetMaterial(material, null);
        mesh = new Mesh();
        datas = new Linedata();
    }

    void Update()
    {
        mesh.Clear();
        mesh.vertices = datas.verts.ToArray();
        mesh.triangles = datas.triganles.ToArray();
        canvasRenderer.SetMesh(mesh);
    }
    void ADDLine(Vector3 start, Vector3 end, float width, ref Linedata ld)
    {
        float h = 0.5f * width;
        float angle = getAngle(start, end);
        int initIndex = ld.verts.Count;
        ld.verts.Add(new Vector3(start.x + h * Mathf.Cos(angle / 180 * Mathf.PI),
            start.y + h * Mathf.Sin(angle / 180 * Mathf.PI)));
        ld.verts.Add(new Vector3(start.x - h * Mathf.Cos(angle / 180 * Mathf.PI),
            start.y - h * Mathf.Sin(angle / 180 * Mathf.PI)));
        ld.verts.Add(new Vector3(end.x + h * Mathf.Cos(angle / 180 * Mathf.PI),
            end.y + h * Mathf.Sin(angle / 180 * Mathf.PI)));
        ld.verts.Add(new Vector3(end.x - h * Mathf.Cos(angle / 180 * Mathf.PI),
            end.y - h * Mathf.Sin(angle / 180 * Mathf.PI)));

        ld.triganles.Add(initIndex);
        ld.triganles.Add(initIndex + 1);
        ld.triganles.Add(initIndex + 3);
        ld.triganles.Add(initIndex);
        ld.triganles.Add(initIndex + 2);
        ld.triganles.Add(initIndex + 3);
    }
    float getAngle(Vector3 start, Vector3 end)
    {
        Vector3 a = end - start;
        Vector3 z = Vector3.forward;
        Vector3 b = Vector3.Cross(a, z);
        return Vector3.Angle(new Vector3(1, 0, 0), b);
    }

    #endregion

    public void DrawlinePoint(LinePoint lp)
    {
        Vector3 mid = new Vector3(lp.startPos.x, lp.endPos.y, 0);
        ADDLine(lp.startPos, mid, lp.width, ref datas);
        ADDLine(mid, lp.endPos, lp.width, ref datas);
    }

    public void Clean()
    {
        datas.triganles.Clear();
        datas.verts.Clear();
    }
}
public class LinePoint
{
    public Vector3 startPos, endPos;
    public float width;
    public string name;

    public LinePoint(Vector3 startPos, Vector3 endPos, float width, string name)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.width = width;
        this.name = name;
    }
}
