using Modules.Services;
using UnityEngine;

namespace Modules.Plants.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class DeepZone : MonoBehaviour
    {
        [SerializeField] private int _level;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("RootHead")) return;

            AliveService.SetDeepLevel(_level);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}