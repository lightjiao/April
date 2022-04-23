using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(GameManager.Init());
    }
}