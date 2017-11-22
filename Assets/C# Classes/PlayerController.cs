using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(Rigidbody))]

public class PlayerController : NetworkBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;
    private Camera _camera;
    private Camera mainCamera;
    private Transform _cameraTransform;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lookSensitivity = 0.5f;


    private void Start()
    {
        ClientScene.RegisterPrefab(bulletPrefab, NetworkHash128.Parse(bulletPrefab.name));
        if (isLocalPlayer) //If I'm the local player
        {
            Cursor.lockState = CursorLockMode.Locked;
            _transform = this.transform; //get my transform and keep a reference to it.
            _rigidbody = this.GetComponent<Rigidbody>();
            _camera = this.GetComponentInChildren<Camera>();
            _cameraTransform = _camera.transform;

            mainCamera = Camera.main; //get the main camera and disable it
            mainCamera.gameObject.SetActive(false);
        }
        else //If im not the local player
        {
            _camera = this.GetComponentInChildren<Camera>();
            _camera.enabled = false; //turn the camera off. 
            _camera.GetComponent<AudioListener>().enabled = false; //disable the camera
        }

    }

    private void Update()
    {
        //if I am the local player
        if (isLocalPlayer)
        {
            //Apply movement vector to rigidbody velocity;
            _rigidbody.velocity = ((_cameraTransform.transform.forward * Input.GetAxis("Vertical")) + (_cameraTransform.right * Input.GetAxisRaw("Horizontal"))) * speed;

            _transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * lookSensitivity);
            _transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * lookSensitivity, Space.World);


            if (Input.GetMouseButtonDown(0))
            {
                CmdSpawnBullet();
            }
        }
    }

    [Command]
    public void CmdSpawnBullet()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, _transform.position + _transform.forward * 2, _transform.rotation);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(_transform.forward * 10, ForceMode.Acceleration);
        NetworkServer.Spawn(spawnedBullet);
    }
    private void OnDisable() //Once im disabled
    {
        if (mainCamera)
        {
            //turn the main camera back on
            mainCamera.gameObject.SetActive(true);
        }
    }
}
