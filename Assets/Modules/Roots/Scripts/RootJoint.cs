using Modules.Services;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootJoint : MonoBehaviour, IRootSegment
    {
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private Texture _diedTexture;

        private RootHead _rootHead;
        private IRootSegment _prevSegment;
        private IRootSegment _nextSegment;
        private readonly static int MainTex = Shader.PropertyToID("_MainTex");

        public void Init(RootHead head, IRootSegment prev, IRootSegment next)
        {
            _rootHead = head;
            _prevSegment = prev;
            _nextSegment = next;
        }

        public void Die()
        {
            _mesh.material.SetTexture(MainTex, _diedTexture);
            if (_prevSegment == null)
                AliveService.Die(_rootHead);
            else
                _prevSegment?.Die();
        }
    }
}