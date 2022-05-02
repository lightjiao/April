using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EventView : MonoBehaviour
    {
        [SerializeField] private Text dayText;
        [SerializeField] private Text contentText;

        private RectTransform _parent;

        private readonly SimpleTimer _delayScrollViewTimer = new(0.2f, 1);

        private bool _updateScrollView;

        private void Awake()
        {
            _parent = transform.parent.GetComponent<RectTransform>();
        }

        public void SetData(string day, string content)
        {
            dayText.text = day;
            contentText.text = content;

            StartCoroutine(DelayUpdateScrollView());
        }

        private IEnumerator DelayUpdateScrollView()
        {
            // 不知道为什么延迟3帧才能先获取 TMP 扩展后的 Text 的高度
            yield return null;
            yield return null;
            yield return null;
            UpdateScrollView();
        }
        

        private void UpdateScrollView()
        {
            var contentHeight = contentText.GetComponent<RectTransform>().rect.height;

            var rectTrans = GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, contentHeight + 20);

            var parentTrans = _parent.GetComponent<RectTransform>();
            var sizeDelta = parentTrans.sizeDelta;
            sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + contentHeight + 20);
            parentTrans.sizeDelta = sizeDelta;

            var parentPos = _parent.position;
            parentPos.y += contentHeight * 3;
            _parent.position = parentPos;
        }
    }
}