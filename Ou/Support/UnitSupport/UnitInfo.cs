using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Ou.Support.UnitSupport
{
    [Serializable]
    public class UnitInfo
    {
        public string unitName = string.Empty;
        public UnitBase curUnit = null;
    }
}
