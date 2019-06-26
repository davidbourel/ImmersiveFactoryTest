using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ICatchable {
    
    public virtual void Catch()
    {
        Debug.Log("Catch");
        Destroy(this);
    }
}
