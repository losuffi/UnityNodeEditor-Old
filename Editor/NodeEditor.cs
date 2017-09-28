using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class NodeEditor : EditorWindow
{
    List<Rect> Nodes = new List<Rect>();
    Dictionary<int, int> Lines = new Dictionary<int, int>();
    Vector2 MousePos;
    int b;
    GameObject t = null;
    bool IsExcide = false;
    [MenuItem("Custom/Relative editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindowWithRect<NodeEditor>(new Rect(0, 0, 500, 500));
       // editor.Init();
    }
    //private void OnEnable()
    //{
    //    hideFlags = HideFlags.HideAndDontSave;
    //    if (ser == null)
    //        ser = new Test1();
    //}
    private void OnGUI()
    {
        //b = EditorGUILayout.IntField("BNumber", b);
        //t = (GameObject)EditorGUILayout.ObjectField("添加组件", t, typeof(GameObject), true);
        //if (t != null)
        //{
        //    t.transform.eulerAngles = new Vector3(b, 0, 0);
        //}
    }
#if X
    public void Init()
    {
        Debug.Log(inj);
        inj++;
        texture = AssetDatabase.LoadAssetAtPath(@"Assets/Editor/UI/Test.png", typeof(Texture)) as Texture;
        current = -1;
    }
    Texture texture;
    int current;
    void OnGUI()
    {
        MousePos = Event.current.mousePosition;
        GUI.DrawTexture(new Rect(0,0,this.position.width,this.position.height),texture);
        if(Event.current.button==1&&Event.current.type== EventType.MouseDown)
        {
            ASelectedMenu();
        }
        BeginWindows();
        for(int i = 0; i < Nodes.Count; i++)
        {
            Nodes[i] = GUI.Window(i, Nodes[i], DrawNodeWindow, "Node" + i);
        }
        foreach (int i in Lines.Keys)
        {
            DrawNodeCurve(Nodes[i], Nodes[Lines[i]]);
        }
        if (current != -1)
        {
            DrawNodeCurve(Nodes[current], Event.current.mousePosition);
            Repaint();
        }
        EndWindows();
    }

    void DrawNodeWindow(int id)
    {
        if (GUI.Button(new Rect(Nodes[id].width - 20, Nodes[id].height / 2, 20, 20), "〉"))
        {
            if (current != -1)
                return;
            current = id;
        }
        if (GUI.Button(new Rect(0, Nodes[id].height / 2, 20, 20), "○"))
        {
            if (current == -1)
                return;
            if (Lines.ContainsKey(current))
            {
                Lines[current] = id;
            }
            else
            {
                Lines.Add(current, id);
            }
            current = -1;
        }
        GUI.DragWindow();
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 Direcrt = (endPos - startPos).normalized;
        //startPos += Direcrt * start.width / 2;
        Vector3 startTan = startPos + Vector3.up * 0;
        Vector3 endTan = endPos - Vector3.up * 0;
        Color shadowCol = new Color(1, 1, 1, 0.6f);
        int Nmax = 3;
        for (int i = 0; i < Nmax; i++) // Draw a shadow
            Handles.DrawBezier(startPos,endPos, startTan, endTan, shadowCol, null, (Nmax - i) * 5);
       // Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.yellow, null, 1);
    }
    void DrawNodeCurve(Rect start, Vector2 end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y, 0);
        Vector3 Direcrt = (endPos - startPos).normalized;
        //startPos += Direcrt * start.width / 2;
        Vector3 startTan = startPos + Vector3.up * 0;
        Vector3 endTan = endPos - Vector3.up * 0;
        Color shadowCol = new Color(1, 1, 1, 0.6f);
        int Nmax = 3;
        for (int i = 0; i < Nmax; i++) // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (Nmax - i) * 5);
        // Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.yellow, null, 1);
    }
    void ASelectedMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Create Node"), false, ContextCallBack, "CN");
        menu.ShowAsContext();

    }
    void ContextCallBack(object obj)
    {
        if (obj.ToString().Equals("CN"))
        {
            Nodes.Add(new Rect(MousePos.x, MousePos.y, 80, 80));
        }
    }
#endif 
}