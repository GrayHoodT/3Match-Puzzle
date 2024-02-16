using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anipang
{
    public class GameEvents : EventArgs
    {
        public event Action Initialized;
        public event Action Started;
        public event Action Paused;
        public event Action Ended;

        public BoardEvents BoardEvents { get; private set; }

        public GameEvents(BoardEvents boardEvents)
        {
            BoardEvents = boardEvents;
        }

        public void NotifyGameInitialized() => Initialized?.Invoke();
        public void NotifyStarted() => Started?.Invoke();
        public void NotifyPaused() => Paused?.Invoke();
        public void NotifyEnded() => Ended?.Invoke();
    }
}
