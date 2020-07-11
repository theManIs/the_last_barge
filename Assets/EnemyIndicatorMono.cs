using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicatorMono : MonoBehaviour
{
    public Transform FollowingEnemy;
    public Transform PlayerPosition;
    public RectTransform IndicatorPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        Vector3 v_diff = (FollowingEnemy.position - PlayerPosition.position);
        float atan2 = Mathf.Atan2(v_diff.y, v_diff.x) * Mathf.Rad2Deg;

        //В моём случае спрайт стрелки повёрнут вверх, так что я отнял 90 градусов.
        transform.rotation = Quaternion.Euler(0f, 0f, atan2 - 90);

        //Вычесляем нужное значение...
        transform.position = Vector3.MoveTowards(transform.position, FollowingEnemy.transform.position, 1000);

        //и подганяем его под размеры экрана.
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    protected void OnDrawGizmos()
    {
//        Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width, (int)(Screen.height / 2), 0)), out RaycastHit raycastHitRight, 100);
//        bool doHitLeft = Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(0, (int)(Screen.height / 2), 0)), out RaycastHit raycastHitLeft, 100);
//        bool doHitMouse = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHitMouse, 100);
//        Debug.Log(Input.mousePosition);
//
//        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, (int)(Screen.height / 2), 0)), raycastHitRight.point);
//        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(0, (int)(Screen.height / 2), 0)), raycastHitLeft.point);
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), raycastHitMouse.point);
//
//        Debug.Log(raycastHitMouse.transform.gameObject.name);
    }
}
