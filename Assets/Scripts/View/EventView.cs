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

        private void Awake()
        {
            _parent = transform.parent.GetComponent<RectTransform>();
        }

        public void SetData(string day, string content)
        {
            dayText.text = day;
            contentText.text = content;

            StartCoroutine(DelayUpdateView());
        }


        private IEnumerator DelayUpdateView()
        {
            // 我不管，我要延迟3帧
            yield return null;
            yield return null;
            yield return null;

            var contentHeight = contentText.GetComponent<RectTransform>().rect.height;
            var rectTrans = GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, contentHeight + 20);

            var parentRect = _parent.rect;
            parentRect.height += contentHeight;

            var position = _parent.position;
            position.y += contentHeight * 3;
            _parent.position = position;
        }
    }
}