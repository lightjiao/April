using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Game : MonoBehaviour
    {
        private readonly SimpleTimer _timer = new(1);

        private readonly List<int> _intsCache = new();

        private void Update()
        {
            if (_timer.UpdateCheck(Time.deltaTime))
            {
                UpdateOneDay();
            }
        }

        private void UpdateOneDay()
        {
            if (GameManager.CurrentState != GameState.Game) return;

            ChooseDailyEvent(GameManager.DailyEventPool1);
            ChooseDailyEvent(GameManager.DailyEventPool2);
            ChooseDailyEvent(GameManager.SpecialEventPool);
            CheckGameOver();
            GameManager.Properties.Day++;
        }

        private void CheckGameOver()
        {
            var eventData = ChooseConditionFirst(GameManager.GameOverPool);
            if (eventData == null) return;

            GameManager.AddHappenedEvent(eventData.Value.Id);
            GameManager.CurrentState = GameState.GameOver;
        }

        private void ChooseDailyEvent(IReadOnlyList<int> eventPools)
        {
            var eventData = ChooseWeightFirst(eventPools);
            GameManager.AddHappenedEvent(eventData.Id);
        }

        private EventData? ChooseConditionFirst(HashSet<int> eventPool)
        {
            _intsCache.Clear();
            foreach (var eventId in eventPool)
            {
                var eventData = GameManager.EventConfigs[eventId];
                if (eventData.Condition != null && !eventData.Condition.Cal()) continue;

                for (var i = 0; i < eventData.Weight; i++)
                {
                    _intsCache.Add(eventId);
                }
            }

            if (_intsCache.Count == 0) return null;

            var index = UnityEngine.Random.Range(0, _intsCache.Count);
            return GameManager.EventConfigs[_intsCache[index]];
        }

        private EventData ChooseWeightFirst(IReadOnlyList<int> eventPool)
        {
            var tryCount = 1000;
            while (tryCount-- > 0)
            {
                var index = UnityEngine.Random.Range(0, eventPool.Count);

                var eventData = GameManager.EventConfigs[eventPool[index]];
                if (eventData.Condition?.Cal() ?? true)
                {
                    return eventData;
                }
            }

            throw new Exception("没有可以选择的事件");
        }
    }
}