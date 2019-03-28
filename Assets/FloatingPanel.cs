using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPanel : MonoBehaviour
{
    public enum Direction
    {
        Horizontal,
        Vertical
    }

    [SerializeField] private Direction MoveDirection = Direction.Horizontal;
    
    [SerializeField] private float speed = 5f;

    [SerializeField] private float distance = 10f;
    // Start is called before the first frame update
    private Vector3 originalPos;
    private bool movingInPositiveDir;
    private bool playerInRange;
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveDirection == Direction.Horizontal)
        {
            if (movingInPositiveDir)
            {
                Vector3 horizontalMovement = new Vector3(speed * Time.deltaTime, 0, 0);
                transform.Translate(horizontalMovement);

                if (transform.position.x >= originalPos.x + distance / 2)
                {
                    movingInPositiveDir = false;
                }
            }
            else
            {
                Vector3 horizontalMovement = new Vector3(-speed * Time.deltaTime, 0, 0);
                transform.Translate(horizontalMovement);
                
                if (transform.position.x <= originalPos.x - distance / 2)
                {
                    movingInPositiveDir = true;
                }
            }

        }
        else
        {
            if (movingInPositiveDir)
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
                if (playerInRange)
                {
//                    PlayerProperty.player.transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
                }
                if (transform.position.y >= originalPos.y + distance / 2)
                {
                    movingInPositiveDir = false;
                }
            }
            else
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
                if (playerInRange)
                {
//                    PlayerProperty.player.transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
                }
                if (transform.position.y <= originalPos.y - distance / 2)
                {
                    movingInPositiveDir = true;
                }
            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            playerInRange = true;
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == GameManager.Instance.player)
        {
            playerInRange = false;
        }
    }
}
