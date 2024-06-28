using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace VectorForestScenery.Demo
{
    public enum InteractionType
    {
        Wind,
        Light,
        Snow,
        Addition,
        Clear
    }

    public class Demo : MonoBehaviour
    {
        #region "Inspector"
        public InteractionType _interaction;
        public Scenery _scenery;
        [Header("Wind")]
        public Utils.Wind _wind;
        public float _windVelocity = 2;
        [Header("Light Variation")]
        public Utils.LightVariation _lightVariation;
        public float _maxLightDistance = 10;
        public float _maxLightVariation = 0.7f;
        [Header("Snow")]
        public Utils.Snow _snow;
        [Header("Addition")]
        public SceneryItem _prefab;
        #endregion

        private bool isLightVariationOn = false;
        private Vector2 lightVariationPoint = Vector2.zero;

        public SceneryItem AdditionPrefab
        {
            get => _prefab;
            set => _prefab = value;
        }

        public InteractionType CurrentInteraction
        {
            get => _interaction;
            set => _interaction = value;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsOverInterface())
            {
                Interaction(true);
            }
            else if (Input.GetMouseButtonDown(1) && !IsOverInterface())
            {
                Interaction(false);
            }
        }

        private void Interaction(bool operation)
        {
            Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            switch (_interaction)
            {
                case InteractionType.Wind:
                    Wind(origin, operation);
                    break;

                case InteractionType.Light:
                    LightVariation(origin, operation);
                    break;

                case InteractionType.Snow:
                    Snow(origin, operation);
                    break;

                case InteractionType.Addition:
                    Addition(origin, operation);
                    break;
            }
        }

        private void Wind(Vector2 origin, bool operation)
        {
            _wind.StartWind(origin, _windVelocity, 30);
        }

        private void Snow(Vector2 origin, bool operation)
        {
            if (operation)
            {
                _snow.SetSnow();
            }
            else
            {
                _snow.RemoveSnow();
            }
        }

        private void LightVariation(Vector2 origin, bool operation)
        {
            isLightVariationOn = operation;
            lightVariationPoint = origin;

            if (operation)
            {
                _lightVariation.SetVariation(_maxLightVariation, origin, _maxLightDistance);
            }
            else
            {
                _lightVariation.RemoveVariation();
            }

        }

        private void Addition(Vector2 origin, bool operation)
        {
            SceneryItem item = _scenery.Add(origin, _prefab);
            if (isLightVariationOn)
            {
                _lightVariation.SetVariation(_maxLightVariation, lightVariationPoint, _maxLightDistance, item);
            }
        }

        private bool IsOverInterface()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}