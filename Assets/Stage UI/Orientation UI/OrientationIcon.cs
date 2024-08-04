using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrientationIcon : MonoBehaviour
{
    PlayerMove m_playerMove;
    RectTransform HUDCanvasRect;
    RectTransform currentRect;

    Image currentImage;
    GameObject followObject;

    bool isCreatThisIcon = true;
    bool isThisIconFollowEnemy;

    private void Start()
    {
        m_playerMove = GameObject.FindObjectOfType<PlayerMove>();
        HUDCanvasRect = transform.parent.transform.parent.transform.parent.gameObject.GetComponent<RectTransform>();

        currentRect = GetComponent<RectTransform>();
        currentImage = GetComponent<Image>();
    }

    public void SetInfo(GameObject _followObject, bool _isThisIconFollowEnemy)
    {
        followObject = _followObject;

        isCreatThisIcon = false;
        isThisIconFollowEnemy = _isThisIconFollowEnemy;

    }

    private void Update()
    {
        if (isCreatThisIcon)
            return;

        if (IsTargetOutsideCanvas(isThisIconFollowEnemy))
        {
            //Debug.Log("Enemy OUTSIDE");
            currentImage.enabled = true;
        }
        else if (!IsTargetOutsideCanvas(isThisIconFollowEnemy))
        {
            //Debug.Log("Enemy INSIDE");
            currentImage.enabled = false;
        }

        SetFollowObject();
        // currentImage.enabled = IsTargetOutsideCanvas(isThisIconFollowEnemy);

        if (IsFollowObjectDestroyed(followObject))
        {
            isCreatThisIcon = true;
            followObject = null;
            currentRect = null;
            Destroy(this.gameObject);
        }
    }

    void SetFollowObject()
    {
        Vector2 screenPoint_follewObject = RectTransformUtility.WorldToScreenPoint(Camera.main, followObject.transform.position);
        Vector2 screenPoint_player = RectTransformUtility.WorldToScreenPoint(Camera.main, m_playerMove.transform.position);

        currentRect.position = screenPoint_follewObject;//AdjustIconInsideCanvas(screenPoint_follewObject, screenPoint_player);

        currentRect.localScale = (screenPoint_player.x > screenPoint_follewObject.x) ? Vector3.one : new Vector3(-1, 1, 1);

       // currentRect.rotation = (screenPoint_player.x > screenPoint_follewObject.x) ? Quaternion.Euler(0, 0, currentRect.rotation.z - 180f) : currentRect.rotation;
       //   currentRect.rotation = (HUDCanvasRect.rect.x > screenPoint_follewObject.x) ? Quaternion.Euler(0, 0, currentRect.rotation.z - 180f) : currentRect.rotation;
    }

    bool IsTargetOutsideCanvas(bool isEnemy)
    {
        float halfWidth;
        float halfHeight;
        
        if (isEnemy)
        {
            CapsuleCollider2D followObjectCollider = followObject.GetComponent<CapsuleCollider2D>();
            halfWidth = followObjectCollider.size.x / 2;
            halfHeight = followObjectCollider.size.y / 2;
        }
        else
        {
            CircleCollider2D followObjectCollider = followObject.GetComponent<CircleCollider2D>();
            halfWidth = followObjectCollider.radius;
            halfHeight = followObjectCollider.radius;
        }

        Vector2 screenPoint_follewObject = RectTransformUtility.WorldToScreenPoint(Camera.main, followObject.transform.position);

        float minX = 0 + halfWidth;
        float maxX = HUDCanvasRect.rect.width - halfWidth;
        float minY = 0 + halfHeight;
        float maxY = HUDCanvasRect.rect.height - halfHeight;

        bool isOverflowX = (screenPoint_follewObject.x > maxX) || (screenPoint_follewObject.x < minX) ? true : false;
        bool isOverflowY = (screenPoint_follewObject.y > maxY) || (screenPoint_follewObject.y < minY) ? true : false;

        if (isOverflowX || isOverflowY)
        {
            Debug.Log("Enemy OUTSIDE!!!!!! " + "isOverflowX:"+ isOverflowX + " / isOverflowY:" + isOverflowY +
                "###" + screenPoint_follewObject.x + " / " + screenPoint_follewObject.y);
            return true;
        }
        else
        {
            Debug.Log("Enemy INSIDE!!!!!!" + "isOverflowX:" + isOverflowX + " / isOverflowY:" + isOverflowY +
                "###" + screenPoint_follewObject.x + " / " + screenPoint_follewObject.y);
            return false;
        }
           
    }

    Vector2 AdjustIconInsideCanvas(Vector2 _followVect, Vector2 _playerVect)
    {
        float halfWidth = currentRect.rect.width/2;
        float halfHeight = currentRect.rect.height/2;

        float minX = 0 + halfWidth;
        float maxX = HUDCanvasRect.rect.width - halfWidth;
        float minY = 0 + halfHeight;
        float maxY = HUDCanvasRect.rect.height - halfHeight;

        Vector2 followVect;
        if (IsTargetOutsideCanvas(isThisIconFollowEnemy))
        {
            bool isOverflowX = (_followVect.x > maxX) || (_followVect.x < minX) ? true : false;
            bool isOverflowY = (_followVect.y > maxY) || (_followVect.y < minY) ? true : false;

            float tempX = (_playerVect.x > _followVect.x) ? minX : maxX ;
            float tempY = (_playerVect.y > _followVect.y) ? minY : maxY ;

            if (isOverflowX)
                followVect = new Vector2(tempX, _followVect.y);
            else if(isOverflowY)
                followVect = new Vector2(_playerVect.x, tempY);
            else
                followVect = new Vector2(tempX, tempY);
        }
        else
            followVect = _followVect;

        return followVect;
    }

    Vector2 KeepIconInsideCanvas(Vector2 _followVect)
    {
        // UI 요소의 현재 위치를 가져옴
        Vector2 anchoredPosition = _followVect;

        float halfWidth = currentRect.rect.width / 2;
        float halfHeight = currentRect.rect.height / 2;

        float minX = -HUDCanvasRect.rect.width / 2 + halfWidth;
        float maxX = HUDCanvasRect.rect.width / 2 - halfWidth;
        float minY = -HUDCanvasRect.rect.height / 2 + halfHeight;
        float maxY = HUDCanvasRect.rect.height / 2 - halfHeight;

        // UI 요소의 위치를 캔버스 내로 제한
        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, minX, maxX);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, minY, maxY);

        return anchoredPosition;
    }

    bool IsFollowObjectDestroyed(GameObject obj)
    {
        if (obj == null || !obj.activeInHierarchy)
            return true;
        else
            return false;
    }
}
