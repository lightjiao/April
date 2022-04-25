﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Game : MonoBehaviour
    {
        private GameContentView _gameContentView;

        /// <summary>
        /// 临时用来展示效果的事件下标
        /// </summary>
        private int _tempEventIndex;

        private const float IntervalTime = 0.7f;
        private float _intervalTimer;

        private void Awake()
        {
            _gameContentView = GetComponentInChildren<GameContentView>();
        }

        private void Update()
        {
            if (GameManager.CurrentState != GameState.Game)
            {
                return;
            }

            _intervalTimer += Time.deltaTime;
            if (_intervalTimer > IntervalTime)
            {
                _intervalTimer = 0f;
                ChooseEvent();
            }
        }

        private void ChooseEvent()
        {
            _tempEventIndex++;

            if (!GameManager.EventConfigs.ContainsKey(_tempEventIndex))
            {
                GameManager.CurrentState = GameState.GameOver;
                return;
            }

            GameManager.Properties.Day++;

            var data = GameManager.EventConfigs[_tempEventIndex];
            _gameContentView.AppendEvent(GameManager.Properties.Day.ToString(), data);
        }
    }
}