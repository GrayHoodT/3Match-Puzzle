using UnityEngine;
using UnityEngine.UI;
using Framework;
using Anipang.Common;
using System.Collections;
using System;

namespace Anipang
{
    public class Block : Poolable, IEquatable<Block>
    {
        [field: SerializeField]
        public Board Board { get; private set; }
        [field: SerializeField]
        public Image Image { get; private set; }
        [field: SerializeField]
        public RectTransform RectTransform { get; private set; }
        [field: SerializeField]
        public Vector2 Position { get; private set; }
        [field: SerializeField]
        public bool IsMatched { get; private set; } = false;

        public void Initialize(Board board)
        {
            Image = GetComponent<Image>();
            RectTransform = GetComponent<RectTransform>();

            Board = board;
            var boardRectTransform = Board.RectTransform;
            var boardWidth = boardRectTransform.rect.width;
            var boardHeight = boardRectTransform.rect.height;
            RectTransform.SetParent(Board.transform);
            var blockWidth = boardWidth / Board.Row;
            var blockHeight = boardHeight / Board.Column;
            RectTransform.sizeDelta = new Vector2(blockWidth, blockHeight);
        }

        public void SetRandomColor()
        {
            Image.color = Board.BlockConfigs.Colors[UnityEngine.Random.Range(0, (int)Enums.BlockType.Count)];
        }

        public void SetMatched(bool isMatched) => IsMatched = isMatched;

        public bool Equals(Block other)
        {
            if (Image.color == other.Image.color)
                return true;

            return false;
        }

        public void Move(float x, float y)
        {
            Position = new Vector2(x, y);

            var boardRect = Board.RectTransform.rect;
            var zeroX = boardRect.width * -0.5f;
            var zeroY = boardRect.height * -0.5f;
            var blockRect = RectTransform.rect;
            var blockWidth = blockRect.width;
            var blockHeight = blockRect.height;
            var blockHalfWidth = blockWidth * 0.5f;
            var blockHalfHeight = blockHeight * 0.5f;
            var calculatedX = zeroX + blockHalfWidth + (Position.x * blockWidth);
            var calculatedY = zeroY + blockHalfHeight + (Position.y * blockHeight);

            RectTransform.anchoredPosition = new Vector2(calculatedX, calculatedY);
        }

        public void MoveTo(float x, float y, float duration)
        {
            StartCoroutine(MoveToCoroutine(x, y, duration));
        }

        public void Destroy()
        {
            StartCoroutine(DestroyCoroutine(Defines.BLOCK_DESTROY_SCALE, 2f));
        }

        private IEnumerator MoveToCoroutine(float x, float y, float duration)
        {
            var boardRect = Board.RectTransform.rect;
            var startedX = boardRect.width * -0.5f;
            var startedY = boardRect.height * -0.5f;
            var blockRect = RectTransform.rect;
            var blockWidth = blockRect.width;
            var blockHeight = blockRect.height;
            var blockHalfWidth = blockWidth * 0.5f;
            var blockHalfHeight = blockHeight * 0.5f;
            var calculatedX = startedX + blockHalfWidth + (x * blockWidth);
            var calculatedY = startedY + blockHalfHeight + (y * blockHeight);

            var startPos = new Vector2(transform.position.x, transform.position.y);
            var to = new Vector2(calculatedX, calculatedY);
            var elapsed = 0.0f;

            while (elapsed < duration)
            {
                elapsed += Time.smoothDeltaTime;
                transform.position = Vector2.Lerp(startPos, to, elapsed / duration);
                yield return null;
            }

            transform.position = to;
            Position = new Vector2(x, y);
        }

        private IEnumerator DestroyCoroutine(float toScale, float speed)
        {
            float factor;

            while (RectTransform.localScale.x <= toScale)
            {
                factor = Time.deltaTime * speed * -1f;
                RectTransform.localScale = new Vector3(RectTransform.localScale.x + factor, RectTransform.localScale.y + factor, RectTransform.localScale.z);

                yield return null;
            }

            //TODO: 이펙트 및 사운드 추가할 것.

            ReturnToPool();
        }
    }
}
