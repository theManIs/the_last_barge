using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardHealth : MonoBehaviour
{
    public NavyBrig RelatedObject;
    public Healthbar HealthBar;

    private Camera _camera;
    private Canvas _canvas;
    private bool Enabled = true;

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();

        if (_canvas)
        {
            _camera = _canvas.worldCamera;
        }

        if (!_canvas || !_camera)
        {
            Enabled = false;
        }
    }

    public void TakeDamage(int amount)
    {
        if (HealthBar)
        {
            HealthBar.TakeDamage(amount);
        }
    }

    public void SetHealth(int amount)
    {
        if (HealthBar)
        {
            HealthBar.SetHealth(amount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Enabled)
        {
            transform.LookAt(_camera.transform);
        }
    }
}
