using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{

    [SerializeField] private GameObject btnprefab;
    public  Dictionary<string,Button> groupButton=new Dictionary<string, Button>();
	void Start () {
        groupButton.Clear();
	    for (int i = 0; i < transform.Find("List").childCount; i++)
	    {
	        var btn=transform.Find("List").GetChild(i).GetComponent<Button>();
            groupButton.Add(btn.transform.name,btn);
	    }
	}
	void Update () {
	
	}

    public Button AddButton(string name, string content)
    {

        var btn = Instantiate<GameObject>(btnprefab);
        btn.transform.parent = transform.Find("List");
        btn.name = name;
        btn.transform.Find("Text").GetComponent<Text>().text = content;
        if (!groupButton.ContainsKey(name))
        {
            groupButton.Add(name, btn.GetComponent<Button>());
        }
        else
        {
            Debug.Log(name);
            Selection.activeObject = groupButton[name];
        }
        return btn.GetComponent<Button>();
    }
}
