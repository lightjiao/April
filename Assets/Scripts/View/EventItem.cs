using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EventItem : MonoBehaviour
    {
        [SerializeField] private Text dayText;
        [SerializeField] private Text contentText;

        public void SetData(string day, string content)
        {
            dayText.text = day;
            contentText.text = content;
        }
    }
}