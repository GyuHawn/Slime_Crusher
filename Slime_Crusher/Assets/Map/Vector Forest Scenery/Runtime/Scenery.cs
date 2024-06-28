using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorForestScenery
{
    public class Scenery : MonoBehaviour
    {
        public List<SceneryItem> Items { get; private set; }

        private void Awake()
        {
            Items = new List<SceneryItem>(GetComponentsInChildren<SceneryItem>());
        }

        public SceneryItem Add(Vector2 origin, SceneryItem prefab)
        {
            SceneryItem item = Instantiate<SceneryItem>(prefab, origin, Quaternion.identity);
            item.transform.SetParent(transform);
            Items.Add(item);
            return item;
        }

        public void Clear()
        {
            foreach (var item in Items)
            {
                Destroy(item.gameObject);
            }

            Items.Clear();
        }
    }
}