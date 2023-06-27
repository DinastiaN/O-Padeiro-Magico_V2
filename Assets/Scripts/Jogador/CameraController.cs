using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera aimCamera;
    public GameObject crosshair;

    private void Start()
    {
        aimCamera.enabled = false;
        crosshair.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            aimCamera.enabled = true;
            crosshair.SetActive(true);
        }
        else
        {
            aimCamera.enabled = false;
            crosshair.SetActive(false);
        }
    }
}
