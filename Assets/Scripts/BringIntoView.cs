using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class BringIntoView : MonoBehaviour, IInputClickHandler
{
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // If the object isn't in view, make it so
        Camera mainCamera = CameraCache.Main;
        var cameraTransform = mainCamera.transform;

        var toPosition = cameraTransform.position + cameraTransform.forward * 1.0f;

        Interpolator interpolator = GetComponent<Interpolator>();
        interpolator.SetTargetPosition(toPosition);
    }
}
