using System;
using System.Collections.Generic;
using Modules.Roots.Scripts;
using UnityEngine;

namespace Modules.Services
{
    public class AliveService
    {
        #region Static
        
        private static AliveService Inst => _self ??= new AliveService();
        private static AliveService _self;
        
        static public event Action OnDied;
        static public void Spawn(RootHead rootHead) =>
            Inst.AddHead(rootHead);
        
        static public void Die(RootHead rootHead) =>
            Inst.RemoveHead(rootHead);
        
        #endregion
        #region Instance
        
        private readonly List<RootHead> _aliveHeads = new List<RootHead>();
        
        private void AddHead(RootHead rootHead) =>
            _aliveHeads.Add(rootHead);
        
        private void RemoveHead(RootHead rootHead)
        {
            _aliveHeads.Remove(rootHead);
            if (_aliveHeads.Count > 0) return;
        
            OnDied?.Invoke();
            Debug.Log("Die");
        }
        
        #endregion
    }
}