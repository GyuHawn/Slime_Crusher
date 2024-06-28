using System.Collections.Generic;
using UnityEngine;

namespace VectorForestScenery.Utils
{
    public class Snow : MonoBehaviour
    {
        #region "Inspector"
        public Scenery _scenery;
        #endregion

        public void SetSnow()
        {
            List<SceneryItem> items = _scenery.Items;
            foreach (var item in items)
            {
                item.SnowOn();
                item.Shake();
            }
        }

        public void RemoveSnow()
        {
            List<SceneryItem> items = _scenery.Items;
            foreach (var item in items)
            {
                item.SnowOff();
                item.Shake();
            }
        }
    }
}
