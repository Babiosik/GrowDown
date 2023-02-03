using Modules.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Scripts
{
    public class WaterBar : MonoBehaviour
    {
        [SerializeField] private float _maxOffset;
        [SerializeField] private Image _bar;
        [SerializeField] private float _maxValue;

        private Vector2 _startPosition;

        private void Start()
        {
            _startPosition = _bar.rectTransform.anchoredPosition;
        }

        private void OnEnable() =>
            ResourcesService.Water.OnChange += OnWaterChange;

        private void OnDisable() =>
            ResourcesService.Water.OnChange -= OnWaterChange;

        private void OnWaterChange(float value)
        {
            float percent = Mathf.Clamp(value / _maxValue, 0, 1);
            _bar.fillAmount = percent;
            _bar.rectTransform.anchoredPosition = _startPosition + Vector2.left * _maxOffset * (1 - percent);
        }
    }
}