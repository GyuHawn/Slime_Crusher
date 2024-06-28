using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorForestScenery.Demo
{
    public class AdditionButton : InteractionButton
    {
        #region "Inspector"
        public SceneryItem _prefab;
        #endregion

        public override void OnClick()
        {
            base.OnClick();
            _demo.AdditionPrefab = _prefab;
        }
    }
}
