using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"树型","EditorType")]
    public class TreeNode:Node
    {
        protected internal virtual TreeNodeResult OnUpdate()
        {
            return TreeNodeResult.Idle;
        }

        protected internal virtual void OnStart()
        {
            
        }
        protected internal override void Evaluator()
        {
            if (outputKnobs.Any())
            {
                foreach (var output in outputKnobs.FindAll(T => T.outputType.Equals("Workstate")))
                {
                    output.SetValue<TreeNodeResult>(TreeNodeResult.Running);
                }
                
            }
        }

        protected internal virtual bool OnCheckCompelete()
        {
            if (IsCompelete)
            {
                return true;
            }
            if (inputKnobs.Any())
            {
                foreach (var input in inputKnobs.FindAll(T => T.InputType.Equals("Workstate")))
                {
                    state = input.GetValue<TreeNodeResult>();
                }
                if (state == TreeNodeResult.Running)
                {
                    OnStart();
                }
            }
            return false;
        }

        protected internal virtual bool IsCompelete
        {
            get
            {
                if (state == TreeNodeResult.Done || state == TreeNodeResult.Failed)
                {

                    return true;
                }
                return false;
            }
        }

        protected internal override void NodeGUI()
        {
            throw new NotImplementedException();
        }

        public override Node Create(Vector2 pos)
        {
            throw new NotImplementedException();
        }
        [SerializeField]
        protected internal TreeNodeResult state = TreeNodeResult.Idle;
        private const string nodeId = "树型";
        public override string GetId { get { return nodeId; } }
    }
    public enum TreeNodeResult
    {
        Idle,
        Done,
        Failed,
        Running,
    }

    public enum SettingType
    {
        填充,
        全局变量,
    }


    public class GlobalVariable
    {
        public Type type;
        public object obj;
        public string identity;

        public GlobalVariable(Type type, object obj,string id)
        {
            this.type = type;
            this.obj = obj;
            this.identity = id;
        }
    }
}
