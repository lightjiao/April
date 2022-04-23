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
                _gameViewManager.gamePanel.SetActiveEx(_currentState == GameState.Game || _currentState == GameState.GameOver);
                _gameViewManager.gameOverBtn.SetActive(_currentState == GameState.GameOver);
            }
        }

        private static GameViewManager _gameViewManager;

        private static Player _player;
        public static Dictionary<int, EventData> AllEvent;
        public static List<int> CurrentEvent = new List<int>();


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

            // 初始化资源
            var request = Resources.LoadAsync<TextAsset>("Event");
            yield return request;
            var textAsset = ((TextAsset) request.asset).text;

            var dataList = CsvParser.ParseData<EventData>(textAsset);
            var dataDict = new Dictionary<int, EventData>();
            foreach (var oneData in dataList)
            {
                dataDict[oneData.Id] = oneData;
            }

            AllEvent = dataDict;
            
            Debug.Log(AllEvent.ToString());

            CurrentState = GameState.GameStart;
            yield return null;
        }
    }
}