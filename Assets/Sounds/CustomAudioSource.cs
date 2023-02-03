using System;
using UnityEngine;

namespace Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class CustomAudioSource : MonoBehaviour
    {
        private const float CamZoomDistanceModifier = 0.5f;
        private const float CamZoomDistanceAdditive = -5f;
        
        [SerializeField] private float _maxDistance;
        // [SerializeField] private AnimationCurve _graphVolumeDistance;

        private AudioSource _audioSource;
        private Camera _camera;
        private float _volumeMax;

        private void Start()
        {
            _camera = Camera.main;
            _audioSource = GetComponent<AudioSource>();
            _volumeMax = _audioSource.volume;
            _audioSource.volume = 0;
        }

        private void LateUpdate()
        {
            float distance = DistanceToCamera;
            float percentDistance = Mathf.Clamp01(distance / _maxDistance);
            _audioSource.volume = (1 - percentDistance) * _volumeMax;
        }
        
        private float DistanceToCamera {
            get
            {
                float distance = Vector2.Distance(_camera.transform.position, transform.position);
                distance += (_camera.orthographicSize + CamZoomDistanceAdditive) * CamZoomDistanceModifier;
                return distance;
            }
        }
    }
}