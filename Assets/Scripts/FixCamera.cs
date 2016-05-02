using UnityEngine;
using System.Collections;

public class FixCamera : MonoBehaviour
{

    public float dampTime = 0.1f;
    private Vector3 velocity = Vector3.zero;
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
            directionFacingBefore = directionFacing;
            newPosition = new Vector3(PlayerMovement.posX + 3, posY, -10);
            transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
        }
        else if (directionFacing == 1)
        {
            directionFacingBefore = directionFacing;
            newPosition = new Vector3(PlayerMovement.posX - 3, posY, -10);
            transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
        }
        else if (directionFacing == 2)
        {
            if (directionFacingBefore == 0)
            {
                newPosition = new Vector3(PlayerMovement.posX + 2, posY, -10);
                transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
            }
            else if (directionFacingBefore == 1)
            {
                newPosition = new Vector3(PlayerMovement.posX - 2, posY, -10);
                transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
            }
            else if (directionFacingBefore == 2)
            {
                newPosition = new Vector3(PlayerMovement.posX, posY, -10);
                transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
            }
        }
    }
}