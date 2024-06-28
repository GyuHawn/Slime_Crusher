using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorForestScenery
{
    public class CreatureItem : SceneryItem
    {
        #region "Inspector"
        public bool _randomAnimationStart;
        #endregion

        public override void Start()
        {
            base.Start();

            if (_randomAnimationStart)
            {
                float random = Random.Range(0f, 1f);
                _animator.Play("Movementation.Idle", 0, random);
                _animator.Play("Route.Route", 1, random);
            }
        }
    }
}
