using UnityEngine;

public static class UnityEx
{
    public static void SetActiveEx(this GameObject self, bool active)
    {
        if (self.activeInHierarchy == active) return;

        self.SetActive(active);
    }

    
}