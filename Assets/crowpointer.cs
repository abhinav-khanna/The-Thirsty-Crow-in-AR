using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using GoogleARCore.Examples.HelloAR;
using GoogleARCore.Examples.Common;

public class TargetIndicatorPoint : MonoBehaviour
{
    public Camera MainCamera;
    public RectTransform M_Icon;
    public Image M_IconImage;
    public Canvas MainCanvas;
    private Vector3 m_cameraOffsetUp;
    private Vector3 m_cameraOffsetRight;
    private Vector3 m_cameraOffsetForward;
    public Sprite m_targetIconOnScreen;
    public Sprite m_targetIconOffScreen;
    [Space]
    [Range(0, 100)]
    public float m_edgeBuffer;
    public Vector3 m_targetIconScale;
    [Space]
    public bool ShowDebugLines;


    void Start()
    {
        //mainCamera = Camera.main;
        //mainCanvas = FindObjectOfType<Canvas>();
        Debug.Assert((MainCanvas != null), "There needs to be a Canvas object in the scene for the OTI to display");
        InstainateTargetIcon();
    }

    void Update()
    {
        if (ShowDebugLines)
            DrawDebugLines();
        UpdateTargetIconPosition();
    }


    private void InstainateTargetIcon()
    {
        M_Icon = new GameObject().AddComponent<RectTransform>();
        M_Icon.transform.SetParent(MainCanvas.transform);
        M_Icon.localScale = m_targetIconScale;
        M_Icon.name = name + ": OTI icon";
        M_IconImage = M_Icon.gameObject.AddComponent<Image>();
        M_IconImage.sprite = m_targetIconOnScreen;
    }


    private void UpdateTargetIconPosition()
    {
        Vector3 newPos = transform.position;
        newPos = MainCamera.WorldToViewportPoint(newPos);
        if (newPos.z < 0)
        {
            newPos.x = 1f - newPos.x;
            newPos.y = 1f - newPos.y;
            newPos.z = 0;
            newPos = Vector3Maxamize(newPos);
        }
        newPos = MainCamera.ViewportToScreenPoint(newPos);
        newPos.x = Mathf.Clamp(newPos.x, m_edgeBuffer, Screen.width - m_edgeBuffer);
        newPos.y = Mathf.Clamp(newPos.y, m_edgeBuffer, Screen.height - m_edgeBuffer);
        M_Icon.transform.position = newPos;
    }


    public void DrawDebugLines()
    {
        Vector3 directionFromCamera = transform.position - MainCamera.transform.position;
        Vector3 cameraForwad = MainCamera.transform.forward;
        Vector3 cameraRight = MainCamera.transform.right;
        Vector3 cameraUp = MainCamera.transform.up;
        cameraForwad *= Vector3.Dot(cameraForwad, directionFromCamera);
        cameraRight *= Vector3.Dot(cameraRight, directionFromCamera);
        cameraUp *= Vector3.Dot(cameraUp, directionFromCamera);
        Debug.DrawRay(MainCamera.transform.position, directionFromCamera, Color.magenta);
        Vector3 forwardPlaneCenter = MainCamera.transform.position + cameraForwad;
        Debug.DrawLine(MainCamera.transform.position, forwardPlaneCenter, Color.blue);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraUp, Color.green);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraRight, Color.red);
    }


    public Vector3 Vector3Maxamize(Vector3 vector)
    {
        Vector3 returnVector = vector;
        float max = 0;
        max = vector.x > max ? vector.x : max;
        max = vector.y > max ? vector.y : max;
        max = vector.z > max ? vector.z : max;
        returnVector /= max;
        return returnVector;
    }
}