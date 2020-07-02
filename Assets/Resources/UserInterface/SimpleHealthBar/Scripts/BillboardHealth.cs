using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillboardHealth : MonoBehaviour
{
    public Healthbar HealthBar;
    public RectTransform[] ArmorShields;

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

        if (!_camera)
        {
            _camera = Camera.main;
            _canvas.worldCamera = _camera;
        }

        if (!_canvas || !_camera)
        {
            Enabled = false;
        }
    }

    public void DisableHealthBar()
    {
        if (HealthBar)
        {
            HealthBar.gameObject.SetActive(false);
        }
    }

    public void DisableArmorAmount(int armorCount)
    {
        for (int i = 0; i < armorCount; i++)
        {
//            Debug.Log(i);
            DisableArmor(i);
        }
    }

    public void DisableArmor(int armorIndex)
    {
        Image img = null;

        if (ArmorShields.Length > armorIndex)
        {
            img = ArmorShields[armorIndex].GetComponent<Image>();
        }

        if (img != null)
        {
            img.enabled = false;
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
