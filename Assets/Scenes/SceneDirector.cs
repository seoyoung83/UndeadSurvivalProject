using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneDirector : MonoBehaviour //���ӻ� ���߿� ����
{

    CameraMove cameraMove;

    [SerializeField] GameObject canvas;

    public bool isSartGame = false;


    public void LoadScene()
    {
        SceneManager.LoadSceneAsync("MainInterfaceScene", LoadSceneMode.Additive);

       // cameraMove.SetCamaera();
        canvas.gameObject.SetActive(false);
    }
}
