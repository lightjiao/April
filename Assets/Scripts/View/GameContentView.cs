using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameContentView : MonoBehaviour
    {
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private EventView eventView;

        private int _shownIdx;
        private readonly StringBuilder _sb = new();

        private void Update()
        {
            if (GameManager.HappenedEvent.Count == 0)
            {
                ClearShowContent();
                _shownIdx = -1;
            }

            if (_shownIdx >= GameManager.HappenedEvent.Count - 1)
            {
                return;
            }

            _sb.Clear();
            while (_shownIdx < GameManager.HappenedEvent.Count - 1)
            {
                _shownIdx++;
                var eventId = GameManager.HappenedEvent[_shownIdx];
                var eventData = GameManager.EventConfigs[eventId];
                _sb.AppendLine(eventData.Content);
            }
            ShowEvent($"第{GameManager.Properties.Day.ToString()}天", _sb.ToString());
        }

        private void ShowEvent(string day, string content)
        {
            var a = Instantiate(eventView, contentRoot);
            a.SetData(day, content);
        }

        private void ClearShowContent()
        {
            if (_shownIdx < 0) return;

            StartCoroutine(ClearShowContentCoroutine());
        }

        private IEnumerator ClearShowContentCoroutine()
        {
            while (contentRoot.childCount > 0)
            {
                var count = 0;
                foreach (Transform child in contentRoot)
                {
                    Destroy(child.gameObject);
                    count++;
                    if (count > 10) break;
                }

                yield return null;
            }
            
            contentRoot.sizeDelta = new Vector2(contentRoot.rect.width, 0);
        }
    }
}