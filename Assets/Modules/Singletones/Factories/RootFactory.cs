using Modules.Roots.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Singletones.Factories
{
    public class RootFactory : MonoBehaviour
    {
        private static RootFactory self;
        
        [SerializeField] private GameObject _rootHead;
        [SerializeField] private GameObject _rootSegment;
        [SerializeField] private GameObject[] _rootSegmentMeshes;
        [SerializeField] private GameObject _rootJoint;

        private void Awake()
        {
            if (self != null)
                Destroy(this);
            else
                self = this;
        }

        static public RootHead CreateRootHead() =>
            self.CreateRootHeadInternal();
        static public RootSegment CreateRootSegment() =>
            self.CreateRootSegmentInternal();
        static public RootSegmentMesh CreateRootSegmentMesh(RootSegment parent) =>
            self.CreateRootSegmentMeshInternal(parent);
        static public RootJoint CreateRootJoint(Vector3 position) =>
            self.CreateRootJointInternal(position);
        
        private RootHead CreateRootHeadInternal()
        {
            var head = Instantiate(
                _rootHead,
                Vector3.up * 1000,
                Quaternion.identity
            ).GetComponent<RootHead>();
            
            return head;
        }
        
        private RootSegment CreateRootSegmentInternal()
        {
            var segment = Instantiate(
                _rootSegment,
                Vector3.up * 1000,
                Quaternion.identity
            ).GetComponent<RootSegment>();
            
            return segment;
        }
        
        private RootSegmentMesh CreateRootSegmentMeshInternal(RootSegment parent)
        {
            var mesh = Instantiate(
                _rootSegmentMeshes[Random.Range(0, _rootSegmentMeshes.Length)],
                parent.transform
            ).GetComponent<RootSegmentMesh>();
            mesh.Init();
            parent.SetMesh(mesh);
            
            return mesh;
        }
        
        private RootJoint CreateRootJointInternal(Vector3 position)
        {
            var joint = Instantiate(
                _rootJoint,
                position,
                Quaternion.Euler(0,0,Random.value * 360)
            ).GetComponent<RootJoint>();
            
            return joint;
        }
    }
}