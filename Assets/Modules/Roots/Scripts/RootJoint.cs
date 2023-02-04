using Modules.Services;
using UnityEngine;

namespace Modules.Roots.Scripts
{
    public class RootJoint : MonoBehaviour, IRootSegment
    {
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private Texture _diedTexture;

        private readonly static int MainTex = Shader.PropertyToID("_MainTex");
        private IRootSegment _prevSegment;
        private IRootSegment _nextSegment;
        private RootHead _rootHead;
        private bool _isInMiddle;
        
        public bool IsDied { get; private set; }

        public void Init(RootHead head, IRootSegment prev, IRootSegment next)
        {
            _rootHead = head;
            _prevSegment = prev;
            _nextSegment = next;
        }
        public void InitMiddle(RootHead head, IRootSegment prev, IRootSegment next)
        {
            Init(head, prev, next);
            _isInMiddle = true;
        }

        public void Die(IRootSegment from)
        {
            IsDied = true;
            _mesh.material.SetTexture(MainTex, _diedTexture);
            _prevSegment.Die(this);
        }
    }
}