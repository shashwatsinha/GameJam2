using UnityEngine;
using System.Collections;

public class FixCamera : MonoBehaviour
{

    public float dampTime = 0.1f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 playerPosition;
    private Vector3 newPosition;

    // Update is called once per frame
    void Update()
    {
        int directionFacing = PlayerMovement.directionFacing;
        if (directionFacing == 0)
        {
            newPosition = new Vector3(PlayerMovement.posX + 3, transform.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
        }
        else if(directionFacing == 1)
        {
            //playerPosition = new Vector3(PlayerMovement.posX - 1, transform.position.y, -10);
            newPosition = new Vector3(PlayerMovement.posX - 3, transform.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, newPosition, dampTime);
        }
        else if(directionFacing == 2)
        {
            playerPosition = new Vector3(PlayerMovement.posX, transform.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, playerPosition, dampTime*0.6f);
        }
        //playerPosition = new Vector3(PlayerMovement.posX, transform.position.y,-10);
        //transform.position = Vector3.Lerp(transform.position, playerPosition, dampTime);

    }

    
}