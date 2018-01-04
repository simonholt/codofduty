using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class BringIntoView : MonoBehaviour, IInputClickHandler
{
    public GameObject TargetGameObject;

    Interpolator interpolator;

    void Start()
    {
        interpolator = TargetGameObject.GetComponent<Interpolator>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (interpolator != null)
        {
            // If the object isn't in view, make it so
            Camera mainCamera = CameraCache.Main;
            var cameraTransform = mainCamera.transform;

            var toPosition = cameraTransform.position + cameraTransform.forward * 1.0f;


            interpolator.SetTargetPosition(toPosition);
        }
    }
}
