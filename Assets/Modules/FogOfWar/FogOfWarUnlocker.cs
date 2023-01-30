using UnityEngine;

namespace Modules.FogOfWar
{
    public class FogOfWarUnlocker : MonoBehaviour
    {
        [SerializeField] private LayerMask _fogLayer;
        [SerializeField] private float _width = 1f;
        [SerializeField] private float _length = 1f;
        [SerializeField] private int _lengthReCheck = 2;

        private Mesh _mesh;
        private Color[] _colors;
        private Vector3[] _vertices;
        private Transform _lastFoWPlane;

        private float RadiusSqr => _width * _width;

        private void Update()
        {
            var r = new Ray(transform.position, Vector3.back);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, 10, _fogLayer, QueryTriggerInteraction.Collide))
            {
                Transform fogOfWarPlane = hit.transform;

                if (fogOfWarPlane != _lastFoWPlane)
                {
                    _lastFoWPlane = fogOfWarPlane;
                    _mesh = _lastFoWPlane.GetComponent<MeshFilter>().mesh;
                    _vertices = _mesh.vertices;
                    _colors = _mesh.colors;
                    if (_colors.Length < _vertices.Length)
                    {
                        _colors = new Color[_vertices.Length];
                        for(var i = 0; i < _colors.Length; i++)
                            _colors[i] = Color.white;
                    }
                }
                else
                {
                    _colors = _mesh.colors;
                }

                for (var i = 0; i < _vertices.Length; i++)
                {
                    Vector3 v = _lastFoWPlane.transform.TransformPoint(_vertices[i]) - hit.point;

                    for (float j = 0; j < _lengthReCheck; j++)
                    {
                        float dist = Vector3.SqrMagnitude(v - transform.right * _length * j) - 2;
                        if (dist < 0) dist = 0;
                        if (dist < RadiusSqr)
                        {
                            float alpha = Mathf.Min(_colors[i].a, dist / RadiusSqr);
                            _colors[i].a = alpha < 0.9f ? 0 : alpha;
                        }
                    }
                }
                _mesh.colors = _colors;
            }
        }
    }
}