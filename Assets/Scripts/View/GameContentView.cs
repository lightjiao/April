using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameContentView : MonoBehaviour
    {
        [SerializeField] private GameObject contentRoot;
        [SerializeField] private EventItem eventItem;

        private int _shownIdx;

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

            while (_shownIdx < GameManager.HappenedEvent.Count - 1)
            {
                _shownIdx++;

                var eventId = GameManager.HappenedEvent[_shownIdx];
                var eventData = GameManager.EventConfigs[eventId];
                ShowEvent(GameManager.Properties.Day.ToString(), eventData.Content);
            }
        }

        private void ShowEvent(string day, string content)
        {
            var a = Instantiate(eventItem, contentRoot.transform);
            a.SetData(day, content);
        }

        private void ClearShowContent()
        {
            if (_shownIdx < 0) return;

            StartCoroutine(CoroutineDestroy());
        }

        private IEnumerator CoroutineDestroy()
        {
            while (contentRoot.transform.childCount > 0)
            {
                var count = 0;
                foreach (Transform child in contentRoot.transform)
                {
                    Destroy(child.gameObject);
                    count++;
                    if (count > 10) break;
                }

                yield return null;
            }
        }
    }
}