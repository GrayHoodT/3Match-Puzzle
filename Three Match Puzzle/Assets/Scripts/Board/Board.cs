using Anipang.Common;
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anipang
{
    public class Board : MonoBehaviour
    {
        [field: SerializeField]
        public int Row { get; private set; }
        [field: SerializeField]
        public int Column { get; private set; }
        [field: SerializeField]
        public RectTransform RectTransform { get; private set; }
        [field: SerializeField]
        public BlockConfigs BlockConfigs { get; private set; }
        [field: SerializeField]
        public BlockPool BlockPool { get; private set; }
        private PoolEvents blockPoolEvents;
        
        private Block[,] blocks;

        private BoardEvents events;
        public BoardEvents Events => events;

        public IEnumerator Initialize(int row, int column, Action<BoardEvents> boardEvents)
        {
            Row = row;
            Column = column;
            blocks = new Block[Row, Column];

            // 해상도 임시 대응.
            RectTransform = gameObject.GetComponent<RectTransform>();
            RectTransform.sizeDelta = new Vector2(Screen.width, Screen.width);
            RectTransform.anchoredPosition = new Vector2(0f, Screen.height * 0.5f);

            // 블럭 Configs 로드.
            var blockConfigsResourceRequest = Resources.LoadAsync<BlockConfigs>(Defines.BLOCK_CONFIGS_PATH);
            yield return blockConfigsResourceRequest;
            BlockConfigs = blockConfigsResourceRequest.asset as BlockConfigs;

            // 블럭 프리팹 로드 및 초기화.
            var blockPrefabResourceRequest = Resources.LoadAsync<Block>(Defines.BLOCK_PREFAB_PATH);
            yield return blockPrefabResourceRequest;
            var blockPrefab = blockPrefabResourceRequest.asset as Block;

            // 블럭 풀 프리팹 로드 및 초기화.
            var blockPoolPrefabResourceRequest = Resources.LoadAsync<GameObject>(Defines.BLOCK_POOL_PREFAB_PATH);
            yield return blockPoolPrefabResourceRequest;
            var blockPoolPrefab = blockPoolPrefabResourceRequest.asset as GameObject;
            BlockPool = blockPoolPrefab.GetComponent<BlockPool>();
            BlockPool.Initialize(blockPrefab, Row, Column, (poolEvents) => { blockPoolEvents = poolEvents; });

            this.events = new BoardEvents(blockPoolEvents);
            boardEvents.Invoke(events);
        }

        public IEnumerator ComposeBoard()
        {
            for(var i = 0; i < Row; i++)
            {
                for(var j = 0; j < Column; j++)
                {
                    var block = BlockPool.Get();
                    // 초기화.
                    block.Initialize(this);
                    block.Move(i, j);
                    block.SetRandomColor();

                    blocks[i, j] = block;

                    yield return null;
                }
            }

            //TODO: 매칭된 블럭을 다시 설정하는 과정 추가해야 됨.

        }

        public IEnumerator CheckMatchedBlocks()
        {
            for(var x = 0; x < Row; x++)
            {
                for(var y = 0; y < Column; y++)
                {
                    yield return CheckMatchedBlock(x, y);
                }
            }
        }

        public IEnumerator RemoveMatchedBlocks()
        {
            for (var x = 0; x < Row; x++)
            {
                for (var y = 0; y < Column; y++)
                {
                    var block = blocks[x, y];
                    if (block.IsMatched == false)
                        continue;

                    block.Destroy();
                    blocks[x, y] = null;
                    yield return null;
                }
            }
        }

        private IEnumerator CheckMatchedBlock(int x, int y)
        {
            Block baseBlock = this.blocks[x, y];

            if (baseBlock == null)
                yield break;

            var matchedBlocks = new List<Block>();

            matchedBlocks.Add(baseBlock);

            // 가로 탐색.
            Block block;

            // 오른쪽 탐색.
            for (int i = y + 1; i < this.Column; i++)
            {
                block = this.blocks[x, i];
                if (!block.Equals(baseBlock))
                    break;

                matchedBlocks.Add(block);
            }

            // 왼쪽 탐색.
            for (int i = y - 1; i >= 0; i--)
            {
                block = this.blocks[x, i];
                if (!block.Equals(baseBlock))
                    break;

                matchedBlocks.Insert(0, block);
            }

            // 매칭된 블럭 설정.
            if (matchedBlocks.Count >= 3)
            {
                for(var i = 0; i < matchedBlocks.Count; i++)
                {
                    var matchedBlock = matchedBlocks[i];
                    matchedBlock.SetMatched(true);
                }
            }

            matchedBlocks.Clear();

            // 세로 탐색
            matchedBlocks.Add(baseBlock);

            //위쪽 탐색
            for (int i = x + 1; i < Row; i++)
            {
                block = this.blocks[i, y];
                if (!block.Equals(baseBlock))
                    break;

                matchedBlocks.Add(block);
            }

            //아래쪽 탐색
            for (int i = x - 1; i >= 0; i--)
            {
                block = this.blocks[i, y];
                if (!block.Equals(baseBlock))
                    break;

                matchedBlocks.Insert(0, block);
            }

            // 매칭된 블럭 설정.
            if (matchedBlocks.Count >= 3)
            {
                for (var i = 0; i < matchedBlocks.Count; i++)
                {
                    var matchedBlock = matchedBlocks[i];
                    matchedBlock.SetMatched(true);
                }
            }

            matchedBlocks.Clear();
        }

        public IEnumerator ClearCoroutine()
        {
            for (var i = 0; i < Row; i++)
            {
                for( var j = 0; j < Column; j++)
                {
                    var block = blocks[i, j];
                    block.Destroy();
                    blocks[i, j] = null;
                    yield return null;
                }
            }
        }
    }
}

