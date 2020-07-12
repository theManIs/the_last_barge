using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicatorMono : MonoBehaviour
{
    public Transform FollowingEnemy;
    public Transform ApproximationPoint;
    public RectTransform IndicatorPanel;
    public Camera WorldCamera;
    public GameObject GameObjectToInstantiate;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(FollowingEnemy.position);

        pos.x = Mathf.Clamp(pos.x, 0.07f, 0.93f);
        pos.y = Mathf.Clamp(pos.y, 0.15f, 0.85f);
        Vector3 posV = Camera.main.ViewportToWorldPoint(pos);
        Vector3 posCanvas = Camera.main.WorldToScreenPoint(posV);
        posCanvas.x -=  Convert.ToInt32(Screen.width / 2);
        posCanvas.y -=  Convert.ToInt32(Screen.height / 2);
        posCanvas.z = 0;
        IndicatorPanel.localPosition = posCanvas;

        Vector3 rotateDot = Camera.main.WorldToViewportPoint(FollowingEnemy.position); ;
        rotateDot.x = Mathf.Clamp01(rotateDot.x);
        rotateDot.y = Mathf.Clamp01(rotateDot.y);
        Vector3 rotateVDot = Camera.main.ViewportToWorldPoint(rotateDot);
        Vector3 rotateCanvas = Camera.main.WorldToScreenPoint(rotateVDot);
        rotateCanvas.x -= Convert.ToInt32(Screen.width / 2);
        rotateCanvas.y -= Convert.ToInt32(Screen.height / 2);
        rotateCanvas.z = 0;

        float angleX = rotateDot.x < 0.5f ? 180 : 0;
        Vector3 enemyDirection = rotateCanvas - posCanvas;
        float angleY = Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg;
        IndicatorPanel.rotation = Quaternion.Euler(new Vector3(angleX, 0, angleY));
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
