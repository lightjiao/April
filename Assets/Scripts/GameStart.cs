using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameStart : MonoBehaviour
    {
        private Button _startBtn;

        private void Awake()
        {
            _startBtn = GetComponent<Button>();
            _startBtn.onClick.AddListener(GameStartClick);
        }

        private void GameStartClick()
        {
            GameManager.CurrentProcedure = GameProcedure.Game;
        }
    }
}