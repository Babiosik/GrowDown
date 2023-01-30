using System.Collections;
using UnityEngine;

namespace Modules.FogOfWar
{
    [RequireComponent(typeof(FogOfWarUnlocker))]
    public class StartUnlocker : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DoDestroy());
        }
        
        private IEnumerator DoDestroy()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }
}