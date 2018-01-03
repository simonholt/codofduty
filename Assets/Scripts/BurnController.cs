using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BurnTarget")
        {
            Debug.Log("Burnable target bro");
            // Fire smoke effect
            var ps = GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            // Make burny noise
            var audio = GetComponentInChildren<AudioSource>();
            audio.Play();

            var rootObject = other.transform.root;

            var rb = rootObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            rootObject.GetComponent<Animator>().enabled = false;

            // Assign other to class (speech?)

            // Remove other (explode / gravity)
            Destroy(rootObject, 5.0f);
        }
    }
}
