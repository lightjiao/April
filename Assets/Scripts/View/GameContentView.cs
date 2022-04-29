using UnityEngine;

namespace DefaultNamespace
{
    public class GameContentView : MonoBehaviour
    {
        [SerializeField] private GameObject contentRoot;
        [SerializeField] private EventItem eventItem;

        private int _shownIdx = 0;

        private void Update()
        {
            if (GameManager.HappenedEvent.Count == 0)
            {
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
            Debug.Log($"{day}: {content}");

            // var a = Instantiate(eventItem, contentRoot.transform);
            // a.SetData(day, data);
        }
    }
}