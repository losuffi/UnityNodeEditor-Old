using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ou.Support.Node
{
    public static class NodeInputControls
    {

        #region WindowPanel
        [Handle(EventType.MouseDrag, 10)]
        private static void HandleWindowPineDrag(NodeInputInfo inputInfo)
        {
            if (inputInfo.InputPos.x < 0 || inputInfo.InputPos.y < 0)
            {
                return;
            }
            NodeEditorState state = inputInfo.EdState;
            if (state.IsPineSetting)
            {
                Vector2 panChangeDragOffset = state.DragOffset;
                state.DragOffset = inputInfo.InputPos - state.DragStart;
                panChangeDragOffset = (state.DragOffset - panChangeDragOffset);
                state.PanOffset += panChangeDragOffset;
            }
        }

        [Handle(EventType.MouseDown, 100)]
        private static void HandleWindowPineDown(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            if (state.FocusNode != null)
            {
                return;
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
        #endregion

        #region NodePanel

        [Handle(EventType.Repaint)]
        private static void HandleNodeFocus(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            state.FocusNode = state.CurGraph.CheckFocus(inputInfo.InputPos);
        }

        [Handle(EventType.MouseDown, 110)]
        private static void HandleNodePanelDown(NodeInputInfo inputInfo)
        {
            NodeEditorState state = inputInfo.EdState;
            if (inputInfo.InputEvent.button==0&&state.FocusNode != null)
            {
                state.SelectedNode = state.FocusNode;
            }
        }

        [Handle(EventType.mouseDrag)]
        private static void HandleNodePanelDrag(NodeInputInfo inputInfo)
        {
            
        }
        [Handle(EventType.MouseUp)]
        private static void HandleNodePanelUp(NodeInputInfo inputInfo)
        {

        }

        #endregion
    }
}
