using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public class TreeNodeManager:NodeManager
    {
        public static TreeNodeManager Instance;
        void Awake()
        {
            TriggerEditorUtility.Init();
            Instance = this;
        }

        protected internal override void RegisterGraph(NodeGraph graph)
        {
            base.RegisterGraph(graph);
            if (!graphs.Contains(graph))
            {
                graphs.Add(graph);
                var obj = new GameObject("macs" + graphs.Count);
                var mac = obj.AddComponent<TreeNrunTimeMac>();
                mac.set(graph);
                macs.Add(mac);
                obj.transform.SetParent(this.transform);
            }
        }
#if OldCode

        void Start()
        {
            for (int i = 0; i < graphs.Count; i++)
            {
                if (graphs[i] == null)
                {
                    graphs.RemoveAt(i);
                    i--;
                    continue;  //查是否空缺
                }
                graphs[i].InitialzationVariable();
                graphs[i].nodes.ForEach(z =>
                {
                    ((TreeNode) z).state = TreeNodeResult.Idle;
                    ((TreeNode) z).feedback = TreeNodeResult.Idle;
                    ((TreeNode) z).ClearKnobMessage();
                }); //重置状态
                TreeNode initNode = graphs[i].nodes.Find(res => res.GetId.Equals("初始化")) as TreeNode;
                if (initNode!=null)
                {
                    initNode.state = TreeNodeResult.Running;
                }
            }
            StartCoroutine(OnUpdate());
        }
        void OnApplicationQuit()
        {
            for (int i = 0; i < graphs.Count; i++)
            {
                graphs[i].EndRuntimeVariable();
            }
        }
        IEnumerator OnUpdate()
        {
            while (true)
            {
                for (int i = 0; i < graphs.Count; i++)
                {
                    TreeRun(graphs[i]);
                    yield return 0;
                }
            }
        }
        void TreeRun(NodeGraph graph)
        {
            foreach (TreeNode node in graph.nodes)
            {
                if(node.state== TreeNodeResult.End)
                    continue;
                node.CheckStart();
                if (node.state == TreeNodeResult.Running)
                {
                    node.state = node.OnUpdate();
                }
                if (node.IsCompelete)
                {
                    node.Evaluator();
                    node.SetEnd();
                }
            }
        }
#endif
    }
}
