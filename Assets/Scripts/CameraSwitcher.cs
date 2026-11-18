using System;
using TMPro;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{

    // spots for the cameras, just putting them in via the inspector
    public GameObject camera1spot;
    public GameObject camera2spot;
    public GameObject camera3spot;
    public GameObject camera4spot;

    public Camera camera;
    public TMP_Text cameraText;

    private string outgoingCameraText;

    private int[] orderOfCameras = {4, 2, 1, 3};
    private int index = 0;

    void Start()
    {
        ResetCamera();
    }


    private void ResetCamera()
    {
        index=0;
        SetCameraToSpot(4);
        cameraText.text = outgoingCameraText;
    }
    private void SetCameraToSpot(int spot)
    {
        switch (spot)
        {
            case 1:
                camera.transform.position = camera1spot.transform.position;
                camera.transform.rotation = camera1spot.transform.rotation;
                break;
            case 2:
                camera.transform.position = camera2spot.transform.position;
                camera.transform.rotation = camera2spot.transform.rotation;
                break;
            case 3:
                camera.transform.position = camera3spot.transform.position;
                camera.transform.rotation = camera3spot.transform.rotation;
                break;
            case 4:
                camera.transform.position = camera4spot.transform.position;
                camera.transform.rotation = camera4spot.transform.rotation;
                break;
        }

        outgoingCameraText = "CAM\n" + spot.ToString();

    }

    private int GetNextCameraSpot()
    {
        index++;
        if (index > 3) {index = 0;}
        return orderOfCameras[index];
    }

    public void SetNextCameraSpot()
    {
        SetCameraToSpot(GetNextCameraSpot());
        cameraText.text = outgoingCameraText;
    }
}
