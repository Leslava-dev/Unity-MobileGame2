using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    [SerializeField] private Light[] lights;

    private void Start()
    {
        ActivateCamera(0); // Старт з першої камери
    }

    private void Update()
    {
        // ---> Натискаюння клавіши 1, 2, 3:
        if (Input.GetKeyDown(KeyCode.Alpha1)) ActivateCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ActivateCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ActivateCamera(2);
    }

    private void ActivateCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == index)
            {
                cameras[i].Priority = 10;
                if (i < lights.Length)
                    lights[i].enabled = true;
            }
            else
            {
                cameras[i].Priority = 0;
                if (i < lights.Length)
                    lights[i].enabled = false;
            }
        }
    }
}
