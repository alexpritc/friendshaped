using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TriggerLoop : MonoBehaviour {

    void Start() {
        GameManager.Instance.LoopComplete();
    }
}
