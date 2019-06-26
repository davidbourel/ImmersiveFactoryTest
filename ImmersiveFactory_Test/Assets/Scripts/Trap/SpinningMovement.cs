using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningMovement : Trap {

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DelayStart());
    }

	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            transform.Rotate(Vector3.right, TrapSpeed * Time.deltaTime);
        }
	}
}
