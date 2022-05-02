using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameRestartButton : MonoBehaviour
    {
        private Button _startBtn;

        private void Awake()
        {
            _startBtn = GetComponent<Button>();
            _startBtn.onClick.AddListener(GameRestartClick);
        }

        private void GameRestartClick()
        {
            GameManager.CurrentState = GameState.GameStart;
        }
    }
}