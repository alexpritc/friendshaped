using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }

    void Awake() {
        if(instance != null) {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    public event Action onLoopComplete;
    public void LoopComplete(){
        if (onLoopComplete != null) {
            onLoopComplete();
        }
    }
}
