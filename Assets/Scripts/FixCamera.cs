using UnityEngine;
using System.Collections;

public class FixCamera : MonoBehaviour
{

    public float dampTime;
    private Vector3 newPosition;
    private int directionFacingBefore = 2;
    private float posY;

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.posY < 0)
            posY = 0f;
        else
            posY = PlayerMovement.posY;
        int directionFacing = PlayerMovement.directionFacing;
        if (directionFacing == 0)
        {
            if(transform.position.x <= -27.5f && PlayerMovement.posX <= -27.5f)
            {
                transform.position = new Vector3(transform.position.x, posY, -10);
            }
            else if (PlayerMovement.posX + 3 > 25.0f)
            {
                newPosition = new Vector3(25.0f, posY, -10);
                transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
            }
            else
            {
                directionFacingBefore = directionFacing;
                newPosition = new Vector3(PlayerMovement.posX + 3, posY, -10);
                transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
            }
        }
        else if (directionFacing == 1)
        {
            if (transform.position.x >= 25.0f && PlayerMovement.posX >= 25.0f)
            {
                transform.position = new Vector3(transform.position.x, posY, -10);
            }
            else if (PlayerMovement.posX - 3 < -27.5f)
            {
                newPosition = new Vector3(-27.5f, posY, -10);
                transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
            }
            else
            {
                directionFacingBefore = directionFacing;
                newPosition = new Vector3(PlayerMovement.posX - 3, posY, -10);
                transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
            }
        }
        else if (directionFacing == 2)
        {
            if (directionFacingBefore == 0)
            {
                if (transform.position.x <= -27.5f && PlayerMovement.posX <= -27.5f)
                {
                    transform.position = new Vector3(transform.position.x, posY, -10);
                }
                else if (PlayerMovement.posX + 3 > 25.0f)
                {
                    newPosition = new Vector3(25.0f, posY, -10);
                    transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
                }
                else
                {
                    newPosition = new Vector3(PlayerMovement.posX + 2, posY, -10);
                    transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
                }
            }
            else if (directionFacingBefore == 1)
            {
                if (transform.position.x >= 25.0f && PlayerMovement.posX >= 25.0f)
                {
                    transform.position = new Vector3(transform.position.x, posY, -10);
                }
                else if (PlayerMovement.posX - 3 < -27.5f)
                {
                    newPosition = new Vector3(-27.5f, posY, -10);
                    transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
                }
                else
                {
                    newPosition = new Vector3(PlayerMovement.posX - 2, posY, -10);
                    transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
                }
            }
            else if (directionFacingBefore == 2)
            {
                newPosition = new Vector3(PlayerMovement.posX, posY, -10);
                transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
            }
        }
    }
}