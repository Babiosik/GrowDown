using System.Collections.Generic;
using System.Linq;
using Modules.Roots.Scripts;
using Modules.Services;
using UnityEngine;

namespace Modules.ResourcesZone.Scripts
{
    enum ResourceType
    {
        Water
    }

    public class ResourceZoneTrigger : MonoBehaviour
    {
        [SerializeField] private ResourceType _resource;
        [SerializeField] private float _amount;
        [SerializeField] private bool _randomAmount;
        [SerializeField] private Vector2 _randomAmountSize;
        [SerializeField] private float _speedEat;
        [SerializeField] private bool _randomRotation;
        [SerializeField] private bool _randomScale;
        [SerializeField] private Vector2 _randomScaleSize;

        [SerializeField] private List<RootSegment> _rootSegments = new List<RootSegment>();
        private Vector3 _fullScale;
        private float _fullAmount;

        private void Start()
        {
            if (_randomRotation)
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            if (_randomScale)
                transform.localScale = Vector3.one * Random.Range(_randomScaleSize.x, _randomScaleSize.y);

            transform.position += Vector3.forward * 0.0001f;
            _fullScale = transform.localScale;
            _fullAmount = _amount;
        }

        private void Update()
        {
            if (_rootSegments.Count == 0 || _amount <= 0) return;

            _rootSegments.RemoveAll(segment => segment.IsDied);

            if (_rootSegments.Count == 0) return;

            float diff = _speedEat * Time.deltaTime * _rootSegments.Count;
            if (_amount < diff) diff = _amount;

            ResourcesService.Water.Value += diff;
            _amount -= diff;
            transform.localScale = Vector3.Lerp(Vector3.zero, _fullScale, _amount / _fullAmount);

            if (_amount > 0) return;
            
            Destroy(gameObject);
        }

        public void OnSegmentEnter(RootSegment rootSegment) =>
            _rootSegments.Add(rootSegment);

        public void OnSegmentExit(RootSegment rootSegment) =>
            _rootSegments.Remove(rootSegment);


    }
}