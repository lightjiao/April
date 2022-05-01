using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    HappenedEvent.Clear();
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
            HappenedEvent.Add(eventId);
            var eventData = EventConfigs[eventId];

            Properties.Add(eventData.AffectsProperties);

            foreach (var branchResult in EventBranchResult(eventData))
            {
                AddHappenedEvent(branchResult);
            }
        }

        private static readonly List<int> EventCache = new();

        private static List<int> EventBranchResult(in EventData eventData)
        {
            EventCache.Clear();

            if (eventData.Branches == null || eventData.Branches.Count == 0)
            {
                return EventCache;
            }

            foreach (var branch in eventData.Branches)
            {
                if (branch.Condition != null && !branch.Condition.Cal()) continue;
                if (branch.EventPool == null || branch.EventPool.Count == 0) continue;

                EventCache.Add(Random.Range(0, branch.EventPool.Count));
            }

            return EventCache;
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
                    _ => DailyEventPool2
                };

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
    }
}