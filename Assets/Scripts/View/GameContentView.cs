using UnityEngine;

namespace DefaultNamespace
{
    public class GameContentView : MonoBehaviour
    {
        [SerializeField] private GameObject contentRoot;
        [SerializeField] private EventItem eventItem;

        public void AppendEvent(string day, EventData data)
        {
            Debug.Log($"{day}: {data.Content}");
            
            // var a = Instantiate(eventItem, contentRoot.transform);
            // a.SetData(day, data);
        }
    }
}