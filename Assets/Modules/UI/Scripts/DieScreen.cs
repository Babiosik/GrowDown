using System;
using DG.Tweening;
using Modules.Services;
using UnityEngine;

namespace Modules.UI.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DieScreen : MonoBehaviour
    {
        private const float DieMoveDuration = 2;

        [SerializeField] private GameObject _hud;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;
        [SerializeField] private float _targetZoom;

        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable() =>
            AliveService.OnDied += OnDied;

        private void OnDisable() =>
            AliveService.OnDied -= OnDied;
        
        private void OnDied()
        {
            InputService.AllowControl = false;
            Vector3 target = _target.position;
            target.z = _camera.transform.position.z;

            _hud.SetActive(false);
            _canvasGroup
                .DOFade(1, DieMoveDuration)
                .OnComplete(() => _canvasGroup.interactable = true);

            float timeStart = Time.time;
            float startZoom = _camera.orthographicSize;
            _camera.transform
                .DOMove(target, DieMoveDuration)
                .OnUpdate(() =>
                {
                    float time = Time.time - timeStart;
                    float percent = time / DieMoveDuration;
                    _camera.orthographicSize = Mathf.Lerp(startZoom, _targetZoom, percent);
                });
        }
    }
}