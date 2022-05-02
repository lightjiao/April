using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public enum GameState
    {
        GameInit,
        GameStart,
        Game,
        GameOver
    }

    public static class GameManager
    {
        private static GameState _currentState;

        public static GameState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;

                _gameViewManager.gameInitPanel.SetActiveEx(_currentState == GameState.GameInit);
                _gameViewManager.gameStartPanel.SetActiveEx(_currentState == GameState.GameStart);
                _gameViewManager.gamePanel.SetActiveEx(_currentState is GameState.Game or GameState.GameOver);
                _gameViewManager.gameOverBtn.SetActive(_currentState == GameState.GameOver);

                if (value == GameState.GameStart)
                {
                    GameStart();
                }
            }
        }

        private static GameViewManager _gameViewManager;

        public static PropertyCollection Properties;

        public static Dictionary<int, EventData> EventConfigs = new();

        public static readonly List<int> DailyEventPool1 = new();
        public static readonly List<int> DailyEventPool2 = new();
        public static readonly List<int> SpecialEventPool = new();

        public static readonly HashSet<int> GameStartPool = new();
        public static readonly HashSet<int> GameOverPool = new();

        public static readonly List<int> HappenedEvent = new();

        public static void AddHappenedEvent(int eventId)
        {
            // 不知道为什么有时候会出一些不存在的ID，这里做一下粗暴的检查
            if (!EventConfigs.ContainsKey(eventId))
            {
                Debug.LogError("出现了很奇怪的事件ID: " + eventId);
                return;
            }

            HappenedEvent.Add(eventId);
            var eventData = EventConfigs[eventId];
            Properties.Add(eventData.AffectsProperties);

            Debug.Log($"当前属性: {Properties}");

            var branch = EventBranchResult(eventData);
            foreach (var branchResult in branch)
            {
                AddHappenedEvent(branchResult);
            }
        }

        private static readonly List<int> EventCache = new();

        private static Span<int> EventBranchResult(in EventData eventData)
        {
            EventCache.Clear();

            if (eventData.Branches == null || eventData.Branches.Count == 0)
            {
                return new Span<int>();
            }

            foreach (var branch in eventData.Branches)
            {
                if (branch.Condition != null && !branch.Condition.Cal()) continue;
                if (branch.EventPool == null || branch.EventPool.Count == 0) continue;

                EventCache.Add(branch.EventPool[Random.Range(0, branch.EventPool.Count)]);
            }

            unsafe
            {
                var result = stackalloc int[EventCache.Count];
                for (var i = 0; i < EventCache.Count; i++)
                {
                    result[i] = EventCache[i];
                }

                return new Span<int>(result, EventCache.Count);
            }
        }

        public static IEnumerator Init()
        {
            // 初始化游戏
            while (_gameViewManager == null)
            {
                _gameViewManager = GameObject.Find("GameViewManager").GetComponent<GameViewManager>();
                yield return null;
            }

            CurrentState = GameState.GameInit;

            yield return null;

            var request = Resources.LoadAsync<TextAsset>("Event");
            yield return request;
            InitConfig(((TextAsset) request.asset).text);

            CurrentState = GameState.GameStart;
            yield return null;
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private static void InitConfig(string dataStr)
        {
            var dataList = CsvParser.ParseData<EventData>(dataStr);
            var dataDict = new Dictionary<int, EventData>();
            foreach (var oneData in dataList)
            {
                dataDict[oneData.Id] = oneData;

                ICollection<int> pool = oneData.Type switch
                {
                    ConstStr.出生事件 => GameStartPool,
                    ConstStr.固定事件 => DailyEventPool1,
                    ConstStr.随机事件 => DailyEventPool2,
                    ConstStr.特殊事件 => SpecialEventPool,
                    ConstStr.结局事件 => GameOverPool,
                    _ => null
                };
                if (pool == null) continue;

                // 以权重的方式将事件添加到随即池, 但 set 类型的池就不添加权重了
                var weight = pool is IList ? oneData.Weight : 1;
                for (var i = 0; i < weight; i++)
                {
                    pool.Add(oneData.Id);
                }
            }

            EventConfigs = dataDict;

            Debug.Log(EventConfigs.ToString());
        }

        private static void GameStart()
        {
            HappenedEvent.Clear();
            Properties.Day = 0;
            Properties.Money = Random.Range(1, 12);
            Properties.Food = Random.Range(1, 11);
            Properties.San = Random.Range(1, 11);
            Properties.ChanceOfQiangCai = Random.Range(1, 100);
            Properties.ChanceOfTuanGou = Random.Range(1, 100);
            Properties.ChanceOfGongsiKongTou = Random.Range(1, 100);
            Properties.ChanceOfSick = Random.Range(1, 100);
        }
    }
}