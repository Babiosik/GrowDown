using UnityEngine;

namespace Modules.FogOfWar
{
    public class FogOfWarUnlocker : MonoBehaviour
    {
        private const float UnlockerRateSec = 1;

        [SerializeField] private LayerMask _fogLayer;
        [SerializeField] private float _width = 1f;
        [SerializeField] private float _length = 1f;
        [SerializeField] private int _lengthReCheck = 2;

        private Mesh _mesh;
        private Color[] _colors;
        private Vector3[] _vertices;
        private Transform _lastFoWPlane;

        private float RadiusSqr => _width * _width;

        private void Start()
        {
            InvokeRepeating(nameof(Unlocker), 0, UnlockerRateSec);
        }

        private void Unlocker()
        {
            if (!IsHitted(out RaycastHit hit)) return;

            Transform fogOfWarPlane = hit.transform;
            if (fogOfWarPlane != _lastFoWPlane)
                SaveFog(fogOfWarPlane);
            else
                _colors = _mesh.colors;

            for (var i = 0; i < _vertices.Length; i++)
            {
                float alpha = GetColorVertices(_vertices[i], hit.point, _colors[i].a);
                for (var j = 1; j <= _lengthReCheck && alpha > 0; j++)
                {
                    Vector3 hitPoint = hit.point + transform.right * _length * j;
                    alpha = GetColorVertices(_vertices[i], hitPoint, alpha);
                }
                _colors[i].a = alpha;
            }
            _mesh.colors = _colors;
        }

        private bool IsHitted(out RaycastHit hit)
        {
            var r = new Ray(transform.position, Vector3.back);
            bool isHitted = Physics.Raycast(r, out hit, 10, _fogLayer, QueryTriggerInteraction.Collide);
            return isHitted;
        }

        private void SaveFog(Transform fog)
        {
            _lastFoWPlane = fog;
            _mesh = _lastFoWPlane.GetComponent<MeshFilter>().mesh;
            _vertices = _mesh.vertices;
            _colors = _mesh.colors;

            if (_colors.Length >= _vertices.Length) return;

            _colors = new Color[_vertices.Length];
            for (var i = 0; i < _colors.Length; i++)
                _colors[i] = Color.white;
        }

        private float GetColorVertices(Vector3 v, Vector3 hitPoint, float alphaInPoint)
        {
            Vector3 vv = _lastFoWPlane.transform.TransformPoint(v) - hitPoint;
            float dist = Vector3.SqrMagnitude(vv) - 2;
            if (dist < 0) dist = 0;
            float alpha = Mathf.Min(alphaInPoint, dist / RadiusSqr);
            
            return alpha < 0.9f ? 0 : alpha;
        }
    }
}