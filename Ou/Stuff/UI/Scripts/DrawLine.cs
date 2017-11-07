using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{
    Vector3 vstart = Vector3.zero;
    Vector3 vend = Vector3.zero;
    private bool isStart = false;
    public Material LineMaterial;
    public List<LinePoints> lines;
    private List<Vector3> points;
    void Start()
    {
        lines=new List<LinePoints>();
        points = new List<Vector3>();
        LineMaterial = new Material(Shader.Find("Unlit/Color"));
        LineMaterial.color=Color.blue;
        StartCoroutine(Draw());
    }

    Vector3 UItoScrren(Vector3 pos)
    {
        return new Vector3(pos.x + Screen.width / 2, pos.y + Screen.height / 2, 0);
    }
    Vector3 Convert(Vector3 pos)
    {
        return new Vector3(pos.x/Screen.width,pos.y/Screen.height);
    }

    void OnPostRender()
    {
        StartCoroutine_Auto(Draw());
    }
    IEnumerator Draw()
    {
        for (int j = 0; j < lines.Count; j++)
        {
            GetPoints(lines[j], ref points);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            LineMaterial.SetPass(0);
            GL.Color(Color.blue);
            for (int i = 0; i < points.Count; i++)
            {
                GL.Vertex(Convert(points[i]));
            }
            GL.End();
            GL.PopMatrix();
            yield return 0;
        }
        yield return 0;
    }


    bool CheckInCanvas(Vector3 vstart)
    {
        if (vstart.x > 0 && vstart.x < Screen.width && vstart.y > 0 && vstart.y < Screen.height)
            return true;
        return false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            vstart = Input.mousePosition;
            if(CheckInCanvas(vstart))
                isStart = true;
        }
        if (Input.GetMouseButtonUp(0)&&isStart)
        {
            vend = Input.mousePosition;
            if (CheckInCanvas(vend))
            {
                lines.Add(new LinePoints(vstart, vend, "test", Color.blue));
            }
            isStart = false;
        }
    }
    void GetPoints(LinePoints line,ref List<Vector3> points)    
    {
        points.Add(line.startPos);
        var p1 = new Vector3(line.endPos.x, line.startPos.y, 0);
        points.Add(p1);
        points.Add(p1);
        points.Add(line.endPos);
        LineMaterial.color = line.lineColor;
    }
}
[System.Serializable]
public class LinePoints
{
    public Vector3 startPos;
    public Vector3 endPos;
    public string lineName;
    public Color lineColor;
    public LinePoints(Vector3 startPos, Vector3 endPos, string lineName, Color lineColor)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.lineName = lineName;
        this.lineColor = lineColor;
    }
}
