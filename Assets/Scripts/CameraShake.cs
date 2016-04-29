using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 2f;
    private float duration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;
    private Vector3 shakeRange = new Vector3(1, 1, 1);
    private bool shakeEnabled = false;
    private float camYPosition;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
        duration = shakeDuration;
    }

    void Update()
    {
        originalPos = new Vector3(PlayerMovement.posX, camTransform.localPosition.y, camTransform.localPosition.z);
        if (Input.GetKeyDown(KeyCode.K))
        {
            shakeEnabled = true;
            camYPosition = camTransform.localPosition.y;
        }
        if (shakeEnabled)
        {
            shake();
        }
        /*GameObject gameObject = GameObject.Find("Main Camera");
        if (shakeDuration > 0)
        {
            gameObject.GetComponent<FixCamera>().enabled = false;
            camTransform.localPosition = originalPos + Vector3.Scale((Random.insideUnitSphere * shakeAmount), shakeRange);

            shakeDuration -= Time.deltaTime * decreaseFactor;
            shakeRange = new Vector3(shakeRange.x*-1,shakeRange.y,shakeRange.z);
        }
        else
        {
            gameObject.GetComponent<FixCamera>().enabled = true;
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }*/
    }
    void shake()
    {
        GameObject gameObject = GameObject.Find("Main Camera");
        if (shakeDuration > 0)
        {
            gameObject.GetComponent<FixCamera>().enabled = false;
            camTransform.localPosition = originalPos + Vector3.Scale((Random.insideUnitSphere * shakeAmount), shakeRange);

            shakeDuration -= Time.deltaTime * decreaseFactor;
            shakeRange = new Vector3(shakeRange.x*-1, shakeRange.y, shakeRange.z);
        }
        else
        {
            gameObject.GetComponent<FixCamera>().enabled = true;
            shakeDuration = duration;
            camTransform.localPosition = new Vector3(originalPos.x,camYPosition,originalPos.z);
            shakeEnabled = false;
        }
    }
}