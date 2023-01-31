using System;
using Modules.Roots.Scripts;
using Modules.Services;
using UnityEngine;

namespace Modules.ItemZone.Scripts
{
    enum ResourceType
    {
        Water
    }

    public class ResourceZoneTrigger : MonoBehaviour
    {
        private const string RootHeadTag = "RootHead";

        [SerializeField] private ResourceType _resource;
        [SerializeField] private float _amount;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(RootHeadTag)) return;

            if (!other.transform.parent.TryGetComponent(out RootHead rootHead))
                return;
            
            switch(_resource)
            {
                case ResourceType.Water:
                    ResourcesService.Water.Value += _amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}