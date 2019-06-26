using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    
    [SerializeField] protected bool isActive = false;
    [SerializeField] protected float TrapSpeed = 30f;
    [SerializeField] protected float maxDelay = 2f;

    protected Vector3 positionTrap;

    protected virtual void Start()
    {
        positionTrap = transform.position;
    }

    protected IEnumerator DelayStart()
    {
        float delay = Random.Range(0, maxDelay);
        yield return new WaitForSeconds(delay);
        isActive = true;
    }
}
