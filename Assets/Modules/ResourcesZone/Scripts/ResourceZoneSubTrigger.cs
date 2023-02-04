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
        
        private readonly List<RootSegment> _rootSegmentColliders = new List<RootSegment>();
        private readonly HashSet<RootSegment> _rootSegmentCollidersHash = new HashSet<RootSegment>();
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

        private void OnDestroy()
        {
            foreach (RootSegment rootSegment in _rootSegmentCollidersHash)
                rootSegment.OnDie -= SegmentOnDie;
        }

        private void HeadEntered(Transform other)
        {
            if (!other.parent.TryGetComponent(out RootHead rootHead))
                return;

            if (rootHead.IsDied) return;

            RootSegment rootSegment = rootHead.GetCurrentSegment;
            
            if (_rootSegmentCollidersHash.Contains(rootSegment)) return;
            _rootSegmentCollidersHash.Add(rootSegment);
            _zone.OnSegmentEnter(rootSegment);
        }
        
        private void SegmentEntered(Transform other)
        {
            if (!other.parent.parent.TryGetComponent(out RootSegment rootSegment))
                return;

            if (!_rootSegmentColliders.Contains(rootSegment))
                rootSegment.OnDie += SegmentOnDie;
            _rootSegmentColliders.Add(rootSegment);
        }
        
        private void SegmentExited(Transform other)
        {
            if (!other.parent.parent.TryGetComponent(out RootSegment rootSegment))
                return;
            
            _rootSegmentColliders.Remove(rootSegment);
            if (_rootSegmentColliders.Contains(rootSegment)) return;

            rootSegment.OnDie -= SegmentOnDie;
            _rootSegmentCollidersHash.Remove(rootSegment);
            _zone.OnSegmentExit(rootSegment);
        }

        private void SegmentOnDie()
        {
            while(true)
            {
                RootSegment rootSegment = _rootSegmentColliders.Find(segment => segment.IsDied);

                if (rootSegment == null)
                    return;

                _rootSegmentColliders.RemoveAll(segment => segment == rootSegment);
                _rootSegmentCollidersHash.Remove(rootSegment);
                _zone.OnSegmentExit(rootSegment);
                rootSegment.OnDie -= SegmentOnDie;
            }
        }
    }
}