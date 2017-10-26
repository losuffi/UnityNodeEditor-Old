using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.NodeSupport
{
    [Node(false,"树型","EditorType")]
    public class TreeNode:Node
    {
        protected internal TreeNodeResult nextIsCompleted
        {
            get
            {
                TreeNodeResult determineComplete = TreeNodeResult.Done;
                foreach (NodeOutput output in outputKnobs)
                {
                    foreach (NodeInput input in output.connections)
                    {
                        if ((input.node as TreeNode).OnUpdate() == TreeNodeResult.Failed)
                        {
                            determineComplete = TreeNodeResult.Failed;
                            return TreeNodeResult.Failed;
                        }
                        else if((input.node as TreeNode).OnUpdate()!= TreeNodeResult.Done)
                        {
                            determineComplete = TreeNodeResult.Running;
                        }
                    }
                }
                return determineComplete;
            }
        }

        [SerializeField]

        protected PopupStructer popupStructer;

        protected internal virtual TreeNodeResult OnUpdate()
        {
            return TreeNodeResult.Idle;
        }

        protected enum GotoType
        {
            Single,
            All,
        }

        protected void Goto(GotoType type = GotoType.All, string des ="")
        {
            if (type == GotoType.All)
            {
                if (outputKnobs.Any())
                {
                    foreach (var output in outputKnobs.FindAll(T => T.outputType.Equals("工作状态")))
                    {
                        output.SetValue<TreeNodeResult>(TreeNodeResult.Running);
                    }

                }
            }
            else if(type== GotoType.Single)
            {
                if (outputKnobs.Any())
                {
                    var output = outputKnobs.Find(T => T.Name.Equals(des));
                    output.SetValue<TreeNodeResult>(TreeNodeResult.Running);
                }
            }
        }
        protected internal virtual void OnStart()
        {
            
        }
        protected internal override void Evaluator()
        {
            Goto();
        }

        protected internal virtual bool OnCheckCompelete()
        {
            if (IsCompelete)
            {
                return true;
            }
            if (inputKnobs.Any())
            {
                foreach (var input in inputKnobs.FindAll(T => T.InputType.Equals("工作状态")))
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

    [Serializable]
    public class GlobalVariable:ISerializationCallbackReceiver
    {
        public Type type;
        public object obj;
        public string identity;
        public string name;

        // [SerializeField] private string objMessage;
        [SerializeField] private string objMessage;

        private bool flag = false;
        public GlobalVariable(Type type, object obj,string id,string name)
        {
            this.type = type;
            this.obj = obj;
            this.identity = id;
            this.name = name;

        }

        public void Update()
        {
            flag = false;
        }
        public void OnBeforeSerialize()
        {
            TriggerEditorUtility.CheckInit();
            if (!flag)
            {
                if (this.identity != null)
                {
                    if (ConnectionType.types.ContainsKey(this.identity))
                    {
                        objMessage = ConnectionType.types[this.identity].ObjtoString(this.obj);
                        flag = true;
                    }
                }
            }
        }

        public void OnAfterDeserialize()
        {
            if (flag && objMessage.Length > 0)
            {
                TriggerEditorUtility.TrigInit += () =>
                {
                    if (name.Equals(string.Empty))
                    {
                        Debug.Log(this.identity);
                    }
                    this.obj = ConnectionType.types[this.identity].StringtoObj(this.objMessage);
                    this.type = this.obj.GetType();
                };
                flag = true;
            }
        }
    }
}
