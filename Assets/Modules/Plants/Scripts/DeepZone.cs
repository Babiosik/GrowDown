using Modules.Roots.Scripts;
using Modules.Services;
using UnityEngine;

namespace Modules.Plants.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class DeepZone : MonoBehaviour
    {
        [SerializeField] private int _level;
        [SerializeField] private bool _isFinish;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("RootHead")) return;

            AliveService.SetDeepLevel(_level);
            GetComponent<BoxCollider>().enabled = false;
            if (!_isFinish)
                return;
            if (other.transform.parent.TryGetComponent(out RootHead rootHead))
                AliveService.SetFinish(rootHead);
        }
    }
}