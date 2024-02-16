using Framework;
using System;

namespace Anipang
{
    public class BoardEvents : EventArgs
    {
        public event Action<Board> OnInitialized;
        public void NotifyInitialized(Board board) => OnInitialized?.Invoke(board);

        public event Action<Board> OnComposed;
        public void NotifyComposed(Board board) => OnComposed?.Invoke(board);

        public PoolEvents BlockPoolEvents { get; private set; }

        public BoardEvents(PoolEvents blockPoolEvents)
        {
            BlockPoolEvents = blockPoolEvents;
        }
    }
}

