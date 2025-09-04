using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dinhvt.Level18
{
    public interface ISelectableObject
    {
        public void OnSelect();
        public void OnDeselect();
    }
}

