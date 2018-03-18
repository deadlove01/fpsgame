using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIngameMenu : MonoBehaviour
{


    [SerializeField]
    private GameObject pauseGO;
    void Start()
    {
        pauseGO = GameObject.Find("PauseMenu");
    }
    
}
