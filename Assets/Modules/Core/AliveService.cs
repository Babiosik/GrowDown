using System;
using System.Collections.Generic;
using Modules.RootChange.Scripts;
using Modules.Roots.Scripts;
using UnityEngine;

namespace Modules.Core
{
    public class AliveService : MonoBehaviour
    {
        private static AliveService _self;
        private List<RootHead> _aliveHeads;

        private void Awake()
        {
            if (_self == null)
                _self = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            _aliveHeads = new List<RootHead>();
        }

        static public void Spawn(RootHead rootHead) =>
            _self.SpawnInternal(rootHead);

        static public void Die(RootHead rootHead) =>
            _self.DieInternal(rootHead);

        private void SpawnInternal(RootHead rootHead) =>
            _aliveHeads.Add(rootHead);

        private void DieInternal(RootHead rootHead)
        {
            _aliveHeads.Remove(rootHead);
            if (_aliveHeads.Count > 0) return;

            Debug.Log("Die");
        }
    }
}