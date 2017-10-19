using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.OuUtility;
using UnityEditor;
using UnityEngine;

namespace Ou.Support.Node
{
    public static class NodeInputControls
    {

        #region WindowPanel
        [Handle(EventType.MouseDrag, 10)]
        private static void HandleWindowPineDrag(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            if (inputInfo.InputPos.x < 0 || inputInfo.InputPos.y < 0||state.SelectedNode!=null||state.SelectedKnob!=null)
            {
                return;
            }
            if (state.IsPineSetting)
            {
                Vector2 panChangeDragOffset = state.DragOffset;
                state.DragOffset = inputInfo.InputPos - state.DragStart;
                panChangeDragOffset = (state.DragOffset - panChangeDragOffset);
                state.PanOffset += panChangeDragOffset / state.GraphZoom;
            }
        }

        [Handle(EventType.MouseDown, 100)]
        private static void HandleWindowPineDown(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            if (state.FocusNode != null&&state.FocusKnob!=null)
            {
                return;
            }
            else
            {
                state.SelectedNode = null;
            }
            if (!state.IsPineSetting)
            {
                state.IsPineSetting = true;
                state.DragStart = inputInfo.InputPos;
                state.DragOffset = Vector2.zero;

            }
            else
            {
                state.DragStart = inputInfo.InputPos;
                state.DragOffset = Vector2.zero;
            }

        }

        [Handle(EventType.MouseUp)]
        private static void HandleWindowPineUp(NodeInputInfo inputInfo)
        {
            if (inputInfo.InputPos.x < 0 || inputInfo.InputPos.y < 0)
            {
                return;
            }
            NodeEditorState state = inputInfo.EdState;
            state.IsPineSetting = false;
        }

        [Handle(EventType.scrollWheel)]
        private static void HandleWindowScroll(NodeInputInfo inputInfo)
        {
            if (inputInfo.InputPos.x < 0 || inputInfo.InputPos.y < 0)
            {
                return;
            }
            NodeEditorState state = inputInfo.EdState;
            state.ZoomPos = (inputInfo.InputPos - NodeEditor.curNodeEditorState.PanOffset) /
                            NodeEditor.curNodeEditorState.GraphZoom;
            Vector2 scalePos = state.ZoomPos;
            float Scale = 0.01f * inputInfo.InputEvent.delta.y;
            state.GraphZoom += Scale;
            if (state.GraphZoom <= 0.8f)
            {
                state.GraphZoom = 0.8f;
                return;
            }
            Vector2 scalePosCurrent = scalePos * state.GraphZoom;
            state.PanAdjust = (scalePos - scalePosCurrent);
        }
        #endregion

        #region NodePanel

        [Handle(EventType.Repaint)]
        private static void HandleCheckFocus(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            state.FocusNode = state.CurGraph.CheckFocus(inputInfo.InputPos);
            state.FocusKnob = state.CurGraph.CheckFocusKnob(inputInfo.InputPos);
        }

        [Handle(EventType.MouseDown, 110)]
        private static void HandleNodePanelDown(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            if (inputInfo.InputEvent.button == 0 && state.FocusNode != null)
            {
                state.SelectedNode = state.FocusNode;
                Selection.activeObject = state.FocusNode;
                if (!state.IsPineSetting)
                {
                    state.IsPineSetting = true;
                    state.DragStart = inputInfo.InputPos;
                    state.DragOffset = Vector2.zero;
                }
                else
                {
                    state.DragStart = inputInfo.InputPos;
                    state.DragOffset = Vector2.zero;
                }
            }
        }

        [Handle(EventType.mouseDrag)]
        private static void HandleNodePanelDrag(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            if (inputInfo.InputPos.x < 0 || inputInfo.InputPos.y < 0||state.SelectedNode==null)
            {
                return;
            }
            if (state.IsPineSetting)
            {
                Vector2 panChangeDragOffset = state.DragOffset;
                state.DragOffset = inputInfo.InputPos - state.DragStart;
                panChangeDragOffset = (state.DragOffset - panChangeDragOffset);
                state.SelectedNode.rect.position += panChangeDragOffset / state.GraphZoom;
            }
        }
        [Handle(EventType.MouseUp)]
        private static void HandleNodePanelUp(NodeInputInfo inputInfo)
        {
            if (inputInfo.InputPos.x < 0 || inputInfo.InputPos.y < 0)
            {
                return;
            }
            NodeEditorState state = inputInfo.EdState;
            state.IsPineSetting = false;
        }
        #endregion

        #region KnopPanel

        [Handle(EventType.MouseDown, 120)]
        private static void HandleKnobMouseDown(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            if (inputInfo.InputEvent.button == 0 && state.FocusKnob != null)
            {
                state.SelectedKnob = state.FocusKnob;
                state.IsLinkSetting = true;
            }
        }

        [Handle(EventType.MouseDrag)]
        private static void HandleKnobMouseDrag(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            if (inputInfo.InputPos.x < 0 || inputInfo.InputPos.y < 0 || state.SelectedKnob == null)
            {
                return;
            }
        }

        [Handle(EventType.MouseUp)]
        private static void HandleKnobMouseUp(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            state.IsLinkSetting = false;
            if (state.SelectedKnob==null||inputInfo.InputPos.x < 0 || inputInfo.InputPos.y < 0||state.SelectedKnob==state.FocusKnob)
            {                
                return;
            }
            if (state.FocusKnob != null)
            {
                if (state.SelectedKnob.GetType().Name.Contains("Output"))
                {
                    NodeOutput output = state.SelectedKnob as NodeOutput;
                    NodeInput input = state.FocusKnob as NodeInput;
                    NodeKnob.Linking(output, input);
                }
                else
                {
                    NodeOutput output = state.FocusKnob as NodeOutput;
                    NodeInput input = state.SelectedKnob as NodeInput;
                    NodeKnob.Linking(output, input);
                }
            }
            else
            {
                state.SelectedKnob = null;
            }
        }
        #endregion
    }
}
