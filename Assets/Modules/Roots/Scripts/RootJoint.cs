using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootJoint : MonoBehaviour, IRootSegment
    {
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private Texture _diedTexture;

        private IRootSegment _prevSegment;
        private IRootSegment _nextSegment;
        private readonly static int MainTex = Shader.PropertyToID("_MainTex");

        public void Init(IRootSegment prev, IRootSegment next)
        {
            _prevSegment = prev;
            _nextSegment = next;
        }
        
        public void Die()
        {
            _mesh.material.SetTexture(MainTex, _diedTexture);
            _prevSegment?.Die();
        }
    }
}