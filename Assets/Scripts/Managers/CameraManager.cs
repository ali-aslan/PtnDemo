using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : BaseSingleton<CameraManager>
{
    public bool dragging;
    private bool DraggingOnUI;
    private Vector3 camOrjPos;
    private Vector3 difference;
    public Vector3 mouseCursorPos;
    public bool PointerOnUi;

    void Update()
    {
        mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PointerOnUi = EventSystem.current.IsPointerOverGameObject();


        if (Input.GetMouseButtonUp(0))
        {
            DraggingOnUI = false;
            dragging = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            camOrjPos = mouseCursorPos;
        }

        if(PointerOnUi && Input.GetMouseButtonDown(0))
        {
            DraggingOnUI = true;
        }

        if (dragging && !PointerOnUi && !DraggingOnUI) //check cursor where if is on ui disable camera movement
        {
            difference = mouseCursorPos - Camera.main.transform.position;

            Camera.main.transform.position = camOrjPos - difference;

            Vector3 cameraPos = Camera.main.transform.position;
            cameraPos.x = Mathf.Clamp(cameraPos.x, 0, GridManager.Instance.Width);
            cameraPos.y = Mathf.Clamp(cameraPos.y, 0, GridManager.Instance.Height);
            Camera.main.transform.position = cameraPos;
        }


        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) // zoom in-out
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + 1f, 5f, 10f);

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) // zoom in-out
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 1f, 5f, 10f);


    }
}
