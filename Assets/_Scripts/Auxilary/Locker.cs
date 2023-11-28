using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class Locker
    {
        public bool IsLocked { get; private set; }
        private HashSet<int> _activeLocks = new HashSet<int>();
        private int _nextLockId = 1;

        public int CreateLock()
        {
            int id = _nextLockId++;
            _activeLocks.Add(id);
            IsLocked = true;
            return id;
        }
        public void DeleteLock(int id)
        {
            _activeLocks.Remove(id);
            IsLocked = _activeLocks.Count > 0;
        }
    }
}
