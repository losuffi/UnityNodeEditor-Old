using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ou.Support.NodeSupport;
using Ou.Support.UnitSupport;
using UnityEngine;

namespace Assets.Scripts
{
    public static class NodeReflectEvent
    {
        [NodeMethod("Test")]
        private static bool Test(UnitBase t)
        {
            Debug.Log("Test");
            return true;
        }
    }
}
