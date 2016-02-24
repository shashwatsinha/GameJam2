using UnityEngine;
using System.Collections;

public class FixCamera : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
  
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(PlayerMovement.posX, transform.position.y,-10);

    }

    
}