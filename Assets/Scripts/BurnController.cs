using HoloToolkit.Unity.InputModule;
using System.Collections;
using UnityEngine;

public class BurnController : MonoBehaviour, ISpeechHandler
{
    private GameObject currentTarget;

    private GameObject rootObject;

    void Start()
    {
        rootObject = transform.root.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "BurnTarget")
        {
            // Fire smoke effect
            var ps = rootObject.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
                ps.Play();

            // Make burny noise
            var audio = rootObject.GetComponentInChildren<AudioSource>();
            if (audio != null)
                audio.Play();

            // Disable the IKnife, enable speech:
            // Note - capsule collider has to be on the same object as this script
            GetComponent<CapsuleCollider>().enabled = false;

            currentTarget = other.transform.root.gameObject;
        }
    }

    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        Debug.Log("Recognized the word " + eventData.RecognizedText);

        if (currentTarget != null)
        {
            StartCoroutine(SendChangesToServer(eventData.RecognizedText));
        }
    }

    IEnumerator SendChangesToServer(string assignClass)
    {
        var myCurrentTarget = currentTarget;
        currentTarget = null;


        // Send thing to server

        var rb = myCurrentTarget.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        var animator = myCurrentTarget.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Assign other to class (speech?)
        Debug.Log("Assigning an object to a class named " + assignClass);

        // Re-enable the knife thingie
        // Note - capsule collider has to be on the same object as this script
        GetComponent<CapsuleCollider>().enabled = true;

        // Remove other (explode / gravity)
        Destroy(myCurrentTarget, 5.0f);

        yield break;
    }
}
