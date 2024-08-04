using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundReposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")|| collision.transform.gameObject.GetComponentInParent<PlayerMove>() == null)
            return;

        Vector3 targetPosition = collision.transform.gameObject.GetComponentInParent<PlayerMove>().transform.position;
        Vector2 targetDirection = collision.transform.gameObject.GetComponentInParent<PlayerMove>().JoysticVector;

        //거리
        float distanceX = Mathf.Abs(targetPosition.x - transform.position.x);
        float distanceY = Mathf.Abs(targetPosition.y - transform.position.y);

        //방향
        float moveX = targetDirection.x > 0 ? 1 : -1;
        float moveY = targetDirection.y > 0 ? 1 : -1;

        if (distanceX > distanceY)
        {
            transform.Translate(Vector3.right * moveX * 40);
        }
        else if (distanceX < distanceY)
        {
            transform.Translate(Vector3.up * moveY * 40);
        }
    }
}
