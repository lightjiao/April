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
            var eventData = ChooseConditionFirst(GameManager.GameOverPool);
            if (eventData == null) return;

            GameManager.HappenedEvent.Add(eventData.Value.Id);
            foreach (var branchResult in EventBranchResult(eventData.Value))
            {
                GameManager.HappenedEvent.Add(branchResult);
            }

            GameManager.CurrentState = GameState.GameOver;
        }

        private void ShowDailyEvent(IReadOnlyList<int> eventPools)
        {
            var eventData = ChooseWeightFirst(eventPools);

            GameManager.HappenedEvent.Add(eventData.Id);
            foreach (var branchResult in EventBranchResult(eventData))
            {
                GameManager.HappenedEvent.Add(branchResult);
            }
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

        private readonly List<int> _eventCache = new();

        private List<int> EventBranchResult(EventData eventData)
        {
            _eventCache.Clear();

            if (eventData.Branches == null || eventData.Branches.Count == 0)
            {
                return _eventCache;
            }

            foreach (var branch in eventData.Branches)
            {
                if (branch.Condition != null && !branch.Condition.Cal()) continue;
                if (branch.EventPool == null || branch.EventPool.Count == 0) continue;

                _eventCache.Add(UnityEngine.Random.Range(0, branch.EventPool.Count));
            }

            return _eventCache;
        }
    }
}