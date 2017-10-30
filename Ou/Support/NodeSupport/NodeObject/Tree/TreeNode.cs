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
        protected internal TreeNodeResult CheckResult(string outputName)
        {
            var output = outputKnobs.Find(T => T.Name.Equals(outputName));
            var res = TreeNodeResult.Done;
            if (output == null||!output.connections.Any())
            {
                return res;
            }
            foreach (NodeInput input in output.connections)
            {
                var nextnode = input.Body as TreeNode;
                if (nextnode.state == TreeNodeResult.Failed)
                {
                    res = TreeNodeResult.Failed;
                    return res;
                }
                else if (nextnode.state == TreeNodeResult.Break)
                {
                    res = TreeNodeResult.Break;
                    return res;
                }
                else if (nextnode.state == TreeNodeResult.Running)
                {
                    res = TreeNodeResult.Running;
                    return res;
                }
                else
                {
                    res = nextnode.CheckResult("Nextout");
                }
            }
            return res;
        }

        protected internal void StateReset(string outputName)
        {
            var outputs = outputKnobs.FindAll(T => T.Name.Equals(outputName));
            ClearKnobMessage();
            if (!outputs.Any())
            {
                return;
            }
            foreach (NodeOutput output in outputs)
            {
                if(!output.connections.Any())
                    continue;
                foreach (NodeInput input in output.connections)
                {
                    (input.Body as TreeNode).state = TreeNodeResult.Idle;
                    (input.Body as TreeNode).StateReset("Nextout");
                }
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
                        output.SetValue<TreeNodeResult>(TreeNodeResult.Start);
                    }

                }
            }
            else if(type== GotoType.Single)
            {
                if (outputKnobs.Any())
                {
                    var output = outputKnobs.Find(T => T.Name.Equals(des));
                    output.SetValue<TreeNodeResult>(TreeNodeResult.Start);
                }
            }
        }

        protected void Goto(int index)
        {
            var output = outputKnobs[index];
            output.SetValue<TreeNodeResult>(TreeNodeResult.Running);
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
                if (state == TreeNodeResult.Start)
                {
                    OnStart();
                    state = TreeNodeResult.Running;
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
        }

        protected internal void ClearKnobMessage()
        {
            foreach (NodeOutput knob in outputKnobs)
            {
                knob.SetValue<TreeNodeResult>(TreeNodeResult.Idle);
            }
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
        Start,
        Done,
        Failed,
        Running,
        Break,
    }

    public enum SettingType
    {
        填充,
        全局变量,
    }

    public enum DataModel
    {
        Editor,
        Runtime,
    }

    [Serializable]
    public class GlobalVariable:ISerializationCallbackReceiver
    { 


        public Type type;
        public object obj;
        public string identity;
        public string name;

        // [SerializeField] private string objMessage;
        [SerializeField] public string objMessage;

        public bool isFromGlobaldatas = false;
        public GlobalVariable(Type type, object obj,string id,string name)
        {
            this.type = type;
            this.obj = obj;
            this.identity = id;
            this.name = name;

        }

        public GlobalVariable(GlobalVariable variable)
        {
            type = variable.type;
            obj = variable.obj;
            identity = variable.identity;
            name = variable.name;
            objMessage = variable.objMessage;
        }

        public GlobalVariable()
        {
            this.type = typeof(string);
            this.obj = string.Empty;
            this.identity = "字符串";
            this.name = string.Empty;
        }

        public void ConvertString()
        {
            objMessage = ConnectionType.types[this.identity].ObjtoString(this.obj);
        }

        public void ConvertObject()
        {
            obj = ConnectionType.types[this.identity].StringtoObj(objMessage);
        }

        public void OnBeforeSerialize()
        {
            if(this.obj==null||this.obj.Equals(string.Empty))
                return;
            objMessage= ConnectionType.types[this.identity].ObjtoString(this.obj);
        }

        public void OnAfterDeserialize()
        {
            if(objMessage.Equals(string.Empty))
                return;
            TriggerEditorUtility.TrigInit += delegate
            {
                obj = ConnectionType.types[this.identity].StringtoObj(this.objMessage);
            };
        }
    }

}
