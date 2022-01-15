using UnityEngine;

namespace Loops {
    public class TriggerLoop : MonoBehaviour {
        void Start() {
            GameManager.Instance.ActionCompleted();
            GameManager.Instance.ActionCompleted();
            GameManager.Instance.ActionCompleted();
        }
    }
}
