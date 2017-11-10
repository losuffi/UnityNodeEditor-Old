using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    public class TreeNrunTimeMac : runTimeMac
    {
        void Start()
        {
            graph.InitialzationVariable();
            graph.nodes.ForEach(z =>
            {
                ((TreeNode)z).state = TreeNodeResult.Idle;
                ((TreeNode)z).feedback = TreeNodeResult.Idle;
                ((TreeNode)z).ClearKnobMessage();
            });
            TreeNode init =graph.nodes.Find(res => res.GetId.Equals("初始化")) as TreeNode;
            if(init!=null)
                init.state = TreeNodeResult.Running;
        }
        void OnApplicationQuit()
        {
            graph.EndRuntimeVariable();
        }
        void Update()
        {
            foreach (TreeNode node in graph.nodes)
            {
                if (node.state == TreeNodeResult.End)
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
    }
}
