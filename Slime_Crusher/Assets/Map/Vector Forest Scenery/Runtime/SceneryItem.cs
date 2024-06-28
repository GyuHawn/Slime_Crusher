using UnityEngine;

namespace VectorForestScenery
{
    public class SceneryItem : MonoBehaviour
    {
        #region "Inspector"
        public ParticleSystem _particle;
        public Animator _animator;
        public bool _randomizeSize;
        public float _minSize = 0.85f;
        public float _maxSize = 1.15f;
        public bool _shakeOnStart;
        public SpriteRenderer[] _lightAffectedRenderers;
        #endregion

        public virtual void Start()
        {
            if (_particle != null)
            {
                _particle.gameObject.SetActive(false);
            }

            if (_randomizeSize)
            {
                RandomizeSize();
            }

            if (_shakeOnStart)
            {
                Shake();
            }
        }

        public void RandomizeSize()
        {
            float size = Random.Range(_minSize, _maxSize);
            transform.localScale = new Vector2(size, size);
        }

        public void WindLeft()
        {
            _animator.SetTrigger("WindLeft");
        }

        public void WindRight()
        {
            _animator.SetTrigger("WindRight");
        }

        public void Shake()
        {
            _animator.SetTrigger("Shake");
        }

        public void SnowOn()
        {
            _animator.SetBool("Snow", true);
        }

        public void SnowOff()
        {
            _animator.SetBool("Snow", false);
        }

        public void TriggerParticle()
        {
            if (_particle != null)
            {
                _particle.gameObject.SetActive(true);
                _particle.Stop();
                _particle.Play();
            }
        }

        public void SetRendererColor(Color color)
        {
            foreach (var renderer in _lightAffectedRenderers)
            {
                renderer.color = color;
            }
        }
    }
}