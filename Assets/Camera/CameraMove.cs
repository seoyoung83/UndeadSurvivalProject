using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
    Default_cameraSize_5,
    Middle_cameraSize_6,
    Last_cameraSize_7,
}

public class CameraMove : MonoBehaviour
{
    public  CameraType currentCamera = CameraType.Default_cameraSize_5;
    public CameraDataSet cameraDataSet;

    [SerializeField] Transform m_playScreenTool;

    Vector3 camaraOffset = new Vector3(0, 0, -10); // 거리 조정은 Camera.size
    float cameraMoveSpeed = 1.25f;
    Transform target;
    Camera m_camera;

    float bossModeSizeup = 2f;

    private void Awake()
    {
        target = GameObject.FindObjectOfType<PlayerMove>().transform;
        m_camera = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        if (target == null)
            return;

        if (StageManager.playState == PlayState.BossCombat)
            bossModeSizeup = 1.5f;
        else
            bossModeSizeup = 0;

             Vector3 pos = new Vector3(target.position.x, target.position.y, 0) + camaraOffset;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * cameraMoveSpeed);

        m_camera.orthographicSize = Mathf.Lerp(m_camera.orthographicSize, cameraDataSet.cameraSet[(int)currentCamera].cameraSize + bossModeSizeup, 0.005f);

        m_playScreenTool.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * cameraMoveSpeed);
        m_playScreenTool.localScale = Vector3.Lerp(m_playScreenTool.localScale,
            cameraDataSet.cameraSet[(int)currentCamera].playScreenToolScaleSize, 0.005f);
    }

    public void SetCameraType(CameraType _type)
    {
        currentCamera = _type;
    }
}
