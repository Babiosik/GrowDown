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
        static public event Action<RootHead> OnFinish;
        static public event Action<int> OnLevelUp;
        static public int GetCurrentLevel => Inst._deepLevel;
        
        static public void Spawn(RootHead rootHead) =>
            Inst.AddHead(rootHead);
        
        static public void Die(RootHead rootHead) =>
            Inst.RemoveHead(rootHead);

        static public void SetDeepLevel(int level) =>
            Inst.SetLevel(level);

        static public void SetFinish(RootHead rootHead) =>
            Inst.SetFinishInternal(rootHead);

        static public void Dispose() =>
            _self = null;
        
        #endregion
        #region Instance
        
        private readonly List<RootHead> _aliveHeads = new List<RootHead>();
        private int _deepLevel = -1;
        
        private void AddHead(RootHead rootHead) =>
            _aliveHeads.Add(rootHead);
        
        private void RemoveHead(RootHead rootHead)
        {
            _aliveHeads.Remove(rootHead);
            if (_aliveHeads.Count > 0) return;
        
            OnDied?.Invoke();
            Debug.Log("Die");
        }

        private void SetLevel(int level)
        {
            if (level <= _deepLevel) return;
            _deepLevel = level;
            OnLevelUp?.Invoke(_deepLevel);
        }
        
        private void SetFinishInternal(RootHead rootHead)
        {
            foreach (RootHead aliveHead in _aliveHeads)
                aliveHead.GetCurrentSegment.SetPauseGross(true);
            OnFinish?.Invoke(rootHead);
        }
        
        #endregion
    }
}