using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MobaCamera : MonoBehaviour
{
    Camera _Camera;
    Transform _tranform;
    public float m_fCameraSpeed = 2000;
    public Text _text;

    Vector3 mouseDragOrigin;
    public bool m_bMouseDrag = true;
    public bool m_bEdgeScroll = true;
    // Use this for initialization
    void Start()
    {
        _Camera = this.GetComponent<Camera>();
        _tranform = this.transform;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bEdgeScroll)
        {
            Vector3 mouseScreenSpace = _Camera.ScreenToViewportPoint(Input.mousePosition);
            //get the distance between the 2 vectors
            float distance = (new Vector3(0.5f, 0.5f, 0) - mouseScreenSpace).magnitude;
            _text.text = mouseScreenSpace.ToString();
            mouseScreenSpace = EdgeScrollDeadZones(new Vector3(0.5f, 0.5f, 0) - mouseScreenSpace, 0.5f);

            if (distance >= 0.5f)
            {
                _tranform.position -= new Vector3(mouseScreenSpace.x, 0, mouseScreenSpace.y) * m_fCameraSpeed * Time.deltaTime;
            }
        }

        if (Input.GetMouseButton(2) && m_bMouseDrag)
        {
            Vector3 mousePosition = _Camera.ScreenToViewportPoint(Input.mousePosition);
            Debug.Log(mouseDragOrigin - mousePosition);
            if ((mouseDragOrigin - mousePosition).magnitude > 0)
            {
                Vector3 resultingVector = mouseDragOrigin - mousePosition;
                Debug.Log(resultingVector);
                _tranform.position += new Vector3(resultingVector.x, 0, resultingVector.y) * m_fCameraSpeed;
            }
        }
    }

    private void LateUpdate()
    {
        //compare the last frame, if any chahnges, reset it
        if (mouseDragOrigin != _Camera.ScreenToViewportPoint(Input.mousePosition))
        {
            mouseDragOrigin = _Camera.ScreenToViewportPoint(Input.mousePosition);
        }
    }
    Vector3 EdgeScrollDeadZones(Vector3 mouseScreenSpace, float deadzone)
    {
        Vector3 temp = mouseScreenSpace;
        if (Mathf.Abs(mouseScreenSpace.x) <= deadzone)
        {
            temp.x = 0;
        }

        if (Mathf.Abs(mouseScreenSpace.y) <= deadzone)
        {
            temp.y = 0;
        }
        return temp;
    }
}
