using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraDataSet", menuName = "ScriptableObjects/Camera Data Setting", order = 2)]
public class CameraDataSet : ScriptableObject
{
    [System.Serializable]
    public class CameraSet
    {
        public CameraType CameraType;
        public float cameraSize;
        public Vector3 playScreenToolScaleSize;
    }

    public CameraSet[] cameraSet;
}
