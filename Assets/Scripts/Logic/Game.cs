using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Game : MonoBehaviour
    {
        private GameContentView _gameContentView;

        private const float IntervalTime = 0.7f;
        private float _intervalTimer;

        private readonly List<int> _intsCache = new();

        private void Awake()
        {
            _gameContentView = GetComponentInChildren<GameContentView>();
        }

        private void Update()
        {
            _intervalTimer += Time.deltaTime;
            if (_intervalTimer > IntervalTime)
            {
                _intervalTimer = 0f;
                UpdateOneDay();
            }
        }

        private void UpdateOneDay()
        {
            if (GameManager.CurrentState != GameState.Game) return;

            CheckGameOver();

            if (GameManager.CurrentState != GameState.Game) return;

            ShowDailyEvent(GameManager.DailyEventPool1);
            ShowDailyEvent(GameManager.DailyEventPool2);
            ShowDailyEvent(GameManager.SpecialEventPool);

            GameManager.Properties.Day++;
        }

        private void CheckGameOver()
        {
            var eventData = ChooseByConditionFirst(GameManager.GameOverPool);
            if (eventData == null) return;

            _gameContentView.AppendEvent(GameManager.Properties.Day.ToString(), eventData.Value);

            GameManager.CurrentState = GameState.GameOver;
        }

        private void ShowDailyEvent(IReadOnlyList<int> eventPools)
        {
            _gameContentView.AppendEvent(GameManager.Properties.Day.ToString(), ChooseWeightFirst(eventPools));
        }

        private EventData? ChooseByConditionFirst(HashSet<int> eventPool)
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