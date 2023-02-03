using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Modules.Roots.Scripts;
using Modules.Services;
using UnityEngine;

namespace Modules.UI.Scripts
{
    public class WinScreen : MonoBehaviour
    {
        private const int WinScreenDelay = 1000;
        private const float WinMoveDuration = 2;

        [SerializeField] private GameObject[] _enableItems;
        [SerializeField] private GameObject _hud;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;
        [SerializeField] private float _targetZoom;
        [SerializeField] private float _targetZoomRoot;

        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable() =>
            AliveService.OnFinish += OnFinish;

        private void OnDisable() =>
            AliveService.OnFinish -= OnFinish;

        private async void OnFinish(RootHead rootHead)
        {
            _hud.SetActive(false);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            InputService.AllowControl = false;

            await MoveToFinish(rootHead);
            await MoveToStart();
            
            await UniTask.Delay(WinScreenDelay);
            _canvasGroup
                .DOFade(1, WinMoveDuration)
                .OnComplete(() => _canvasGroup.interactable = true);
        }

        private async UniTask MoveToFinish(RootHead rootHead)
        {
            Vector3 target = rootHead.transform.position;
            target.z = _camera.transform.position.z;

            float timeStart = Time.time;
            float startZoom = _camera.orthographicSize;
            TweenerCore<Vector3, Vector3, VectorOptions> moveAnimation = _camera.transform.DOMove(target, WinMoveDuration);
            moveAnimation.OnUpdate(() =>
            {
                float time = Time.time - timeStart;
                float percent = time / WinMoveDuration;
                _camera.orthographicSize = Mathf.Lerp(startZoom, _targetZoomRoot, percent);
            });
            
            await moveAnimation.AsyncWaitForCompletion();
            
            foreach (GameObject enableItem in _enableItems)
                enableItem.SetActive(true);
        }
        
        private async UniTask MoveToStart()
        {
            Vector3 target = _target.position;
            target.z = _camera.transform.position.z;

            float timeStart = Time.time;
            float startZoom = _camera.orthographicSize;
            TweenerCore<Vector3, Vector3, VectorOptions> moveAnimation = _camera.transform.DOMove(target, WinMoveDuration);
            moveAnimation.OnUpdate(() =>
            {
                float time = Time.time - timeStart;
                float percent = time / WinMoveDuration;
                _camera.orthographicSize = Mathf.Lerp(startZoom, _targetZoom, percent);
            });
            
            await moveAnimation.AsyncWaitForCompletion();
        }
    }
}