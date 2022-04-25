using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameStartButton : MonoBehaviour
    {
        private Button _startBtn;

        private void Awake()
        {
            _startBtn = GetComponent<Button>();
            _startBtn.onClick.AddListener(GameStartClick);
        }

        private void GameStartClick()
        {
            GameManager.CurrentState = GameState.Game;
        }
    }
}