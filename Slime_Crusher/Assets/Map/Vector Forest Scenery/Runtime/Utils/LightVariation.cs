using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorForestScenery.Utils
{
    public class LightVariation : MonoBehaviour
    {
        #region "Inspector"
        public Scenery _scenery;
        #endregion

        public void SetVariation(float maxVariation, Vector2 origin, float maxDistance)
        {
            List<SceneryItem> items = _scenery.Items;

            foreach (var item in items)
            {
                SetVariation(maxVariation, origin, maxDistance, item);
            }
        }

        public void SetVariation(float maxVariation, Vector2 origin, float maxDistance, SceneryItem item)
        {
            float distanceFromOrigin = Vector2.Distance(item.transform.position, origin);
            float distancePercentage = Mathf.InverseLerp(0, maxDistance, distanceFromOrigin);
            float lightDecrease = Mathf.Lerp(0, maxVariation, distancePercentage);
            var color = new Color(1 - lightDecrease, 1 - lightDecrease, 1 - lightDecrease, 1);
            item.SetRendererColor(color);
        }

        public void RemoveVariation()
        {
            List<SceneryItem> items = _scenery.Items;

            foreach (var item in items)
            {
                item.SetRendererColor(new Color(1, 1, 1, 1));
            }
        }
    }
}
