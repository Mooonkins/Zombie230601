using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("GameManger");
                    instance = instanceContainer.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    public static GameManager instance;

    public bool isGameover = false;
}
