using System;
using System.Collections.Generic;
using Modules.Roots.Scripts;
using UnityEngine;

namespace Modules.Services
{
    static public class AliveService
    {
        static public event Action OnDied;
        
        private readonly static List<RootHead> AliveHeads = new List<RootHead>();

        static public void Spawn(RootHead rootHead) =>
            AliveHeads.Add(rootHead);

        static public void Die(RootHead rootHead) 
        {
            AliveHeads.Remove(rootHead);
            if (AliveHeads.Count > 0) return;

            OnDied?.Invoke();
            Debug.Log("Die");
        }
    }
}