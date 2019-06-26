using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMovement : Trap {

    [SerializeField] private float height = 0.5f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DelayStart());
    }

    // Update is called once per frame
    void Update () {

        if(isActive){
            float newY = Mathf.Sin(Time.time * TrapSpeed) * height;
            transform.position = new Vector3(positionTrap.x, positionTrap.y + newY, positionTrap.z);
        }
	}
}
