using Modules.Roots.Scripts;
using UnityEngine;

namespace Modules.DeathZone.Scripts
{
    public class DeathZone : MonoBehaviour
    {
        private const string RootHeadTag = "RootHead";
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(RootHeadTag)) return;
            
            if (other.transform.parent.TryGetComponent(out RootHead rootHead))
            {
                rootHead.SetDied();
            }
        }
    }
}