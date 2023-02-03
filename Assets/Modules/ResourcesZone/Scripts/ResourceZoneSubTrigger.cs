using System;
using System.Collections.Generic;
using Modules.Roots.Scripts;
using UnityEngine;

namespace Modules.ResourcesZone.Scripts
{
    public class ResourceZoneSubTrigger : MonoBehaviour
    {
        private const string RootHeadTag = "RootHead";
        private const string RootSegmentTag = "RootSegment";
        
        [SerializeField] private List<RootSegment> _rootSegmentColliders = new List<RootSegment>();
        private ResourceZoneTrigger _zone;

        private void Start()
        {
            _zone = GetComponentInParent<ResourceZoneTrigger>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(RootSegmentTag))
                SegmentEntered(other.transform);

            if (other.CompareTag(RootHeadTag))
                HeadEntered(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(RootSegmentTag))
                SegmentExited(other.transform);
        }

        private void HeadEntered(Transform other)
        {
            if (!other.parent.TryGetComponent(out RootHead rootHead))
                return;

            if (rootHead.IsDied) return;
            _zone.OnSegmentEnter(rootHead.GetCurrentSegment);
        }
        
        private void SegmentEntered(Transform other)
        {
            if (!other.parent.parent.TryGetComponent(out RootSegment rootSegment))
                return;

            _rootSegmentColliders.Add(rootSegment);
        }
        
        private void SegmentExited(Transform other)
        {
            if (!other.parent.parent.TryGetComponent(out RootSegment rootSegment))
                return;
            
            _rootSegmentColliders.Remove(rootSegment);
            RootSegment segment = _rootSegmentColliders.Find(segment => segment == rootSegment);
            if (segment != null) return;
            
            _zone.OnSegmentExit(rootSegment);
        }
    }
}