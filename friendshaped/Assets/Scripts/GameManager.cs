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
    
    public event Action onAllActionsComplete;
    public void AllActionsComplete(){
        if (onAllActionsComplete != null) {
            onAllActionsComplete();
        }
    }
    
    public event Action onActionCompleted;
    public void ActionCompleted(){
        if (onActionCompleted != null) {
            onActionCompleted();
        }
    }
}
