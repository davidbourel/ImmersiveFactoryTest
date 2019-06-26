using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour {

    private void OnCollisionEnter(Collision col)
    {
        GameManager.Instance.EndGame();
    }
}
