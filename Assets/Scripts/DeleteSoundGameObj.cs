using UnityEngine;
using System.Collections;

public class DeleteSoundGameObj : MonoBehaviour {

    private float destroyTime;


	// Use this for initialization
	void Start ()
    {
        var sound = this.GetComponent<AudioSource>();
        destroyTime = sound.clip.length;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0f)
            Destroy(this.gameObject);	
	}
}
