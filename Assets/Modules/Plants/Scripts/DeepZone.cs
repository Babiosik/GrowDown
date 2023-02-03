using UnityEngine;

namespace Modules.Plants.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class DeepZone : MonoBehaviour
    {
        [SerializeField] private PlantLive _plant;
        [SerializeField] private Texture _aliveTexture;
        [SerializeField] private Texture _deadTexture;
        [SerializeField] private uint _level;

        public Texture AliveTexture => _aliveTexture;
        public Texture DeadTexture => _deadTexture;
        public uint Level => _level;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("RootHead")) return;

            _plant.SetZone(this);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}