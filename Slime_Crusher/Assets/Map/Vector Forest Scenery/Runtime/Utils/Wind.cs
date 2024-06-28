using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorForestScenery.Utils
{
    public class Wind : MonoBehaviour
    {
        #region "Inspector"
        public Scenery _scenery;
        #endregion

        public void StartWind(Vector2 origin, float velocity, float maxDistance)
        {
            StartCoroutine(WindCycle(origin, velocity, maxDistance));
        }

        private IEnumerator WindCycle(Vector2 origin, float velocity, float maxDistance)
        {
            float distanceFromOrigin = 0;
            List<SceneryItem> alreadyInteracted = new List<SceneryItem>();
            List<SceneryItem> items = _scenery.Items;

            while (distanceFromOrigin < maxDistance && items.Count != alreadyInteracted.Count)
            {
                yield return new WaitForFixedUpdate();
                distanceFromOrigin += (Time.fixedDeltaTime * velocity);

                foreach (var item in items)
                {
                    bool interacted = alreadyInteracted.Contains(item);
                    bool isInsideDistance = Vector2.Distance(item.transform.position, origin) < distanceFromOrigin;
                    if (!interacted && isInsideDistance)
                    {
                        alreadyInteracted.Add(item);
                        WindItem(item, origin);
                    }
                }
            }
        }

        private void WindItem(SceneryItem item, Vector2 origin)
        {
            if (item.transform.position.x < origin.x)
            {
                item.WindLeft();
            }
            else
            {
                item.WindRight();
            }
        }
    }
}