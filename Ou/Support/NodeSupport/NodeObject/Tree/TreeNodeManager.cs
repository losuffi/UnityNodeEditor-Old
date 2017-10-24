using System;
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
                graphs[i].nodes.ForEach(z => ((TreeNode) z).state = TreeNodeResult.Idle); //重置状态
                TreeNode initNode = graphs[i].nodes.Find(res => res.GetId.Equals("初始化")) as TreeNode;
                if (initNode!=null)
                {
                    initNode.state = TreeNodeResult.Running;
                }
            }
        }
        void Update()
        {
            for (int i = 0; i < graphs.Count; i++)
            {
                graphs[i].TreeRun();
            }
        }
    }
}
