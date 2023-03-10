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
        [SerializeField] private float _dangerousLine;
        [SerializeField] private Animator _dangerousAnimator;

        private Vector2 _startPosition;
        private bool _isDangerous = false;
        private readonly static int IsDangerous = Animator.StringToHash("IsDangerous");

        private void Start()
        {
            _startPosition = _bar.rectTransform.anchoredPosition;
            OnWaterChange(ResourcesService.Water.Value);
        }

        private void OnEnable() =>
            ResourcesService.Water.OnChange += OnWaterChange;

        private void OnDisable() =>
            ResourcesService.Water.OnChange -= OnWaterChange;

        private void OnWaterChange(float value)
        {
            float percent = Mathf.Clamp(value / _maxValue, 0, 1);
            _bar.fillAmount = percent;
            // _bar.rectTransform.anchoredPosition = _startPosition + Vector2.left * _maxOffset * (1 - percent);
            
            switch(_isDangerous)
            {
                case false when value < _dangerousLine:
                    _dangerousAnimator.SetBool(IsDangerous, _isDangerous = true);
                    break;
                case true when value > _dangerousLine:
                    _dangerousAnimator.SetBool(IsDangerous, _isDangerous = false);
                    break;
            }
        }
    }
}