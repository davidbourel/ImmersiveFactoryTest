using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private int lifeNumber = 2;
    [SerializeField] private float invincibilityDuration = 0.5f;

    private Camera myCamera;

    private int currentLifeNumber = 3;

    private bool isInvincible = false;
    private float invicibilityTimer = 0;

    // Use this for initialization
    void Start()
    {
        myCamera = Camera.main;
        currentLifeNumber = lifeNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isInvincible)
        {
            invicibilityTimer += Time.deltaTime;
            if(invicibilityTimer > invincibilityDuration)
            {
                CanvasManager.Instance.PlayerHit(false); 
                isInvincible = false;
                invicibilityTimer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = myCamera.ScreenPointToRay(transform.forward); //Debug.DrawRay(transform.position, myCamera.transform.forward * 10, Color.green,10);

            //Check if raycast touch something catchable
            if (Physics.Raycast(transform.position, myCamera.transform.forward, out hit, Mathf.Infinity))
            {
                ICatchable objectCaught = hit.transform.gameObject.GetComponent<ICatchable>();

                if (objectCaught != null)
                {
                    objectCaught.Catch();
                    if(currentLifeNumber < lifeNumber)
                    {
                        RecoverLife();
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!isInvincible)
        {
            Trap trap = col.transform.gameObject.GetComponent<Trap>();

            if (trap != null)
            {
                isInvincible = true;
                currentLifeNumber--;
                GameManager.Instance.PlayerLoseLife(currentLifeNumber);

                if (currentLifeNumber < 1)
                {
                    GameManager.Instance.PlayerDie();
                }
                else
                {
                    CanvasManager.Instance.PlayerHit(true); 
                }
            }
        }
    }

    public void ResetPlayerAfterDie(Transform spawnPosition)
    {
        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
        currentLifeNumber = lifeNumber;
        GameManager.Instance.PlayerLoseLife(currentLifeNumber);
    }

    public void RecoverLife()
    {
        currentLifeNumber ++;
        GameManager.Instance.PlayerLoseLife(currentLifeNumber);
    }

    public int CurrentLifeNumber
    {
        get
        {
            return currentLifeNumber;
        }
    }

}
