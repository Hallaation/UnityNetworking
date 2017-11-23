using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(Rigidbody))]

public class PlayerController : NetworkBehaviour
{
    private int miHealth = 5;
    public int Health { get { return miHealth; } set { miHealth = value; } }
    NetworkConnection mConnection;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private Camera _camera;
    private Camera mainCamera;
    private Transform _cameraTransform;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float lookSensitivity = 0.5f;

    [SerializeField]
    private LineRenderer _lineRenderer;
    private PlayerMessageSender pSender;
    private void Start()
    {
        pSender = this.GetComponent<PlayerMessageSender>();
        mConnection = this.GetComponent<NetworkIdentity>().connectionToClient;
        ClientScene.RegisterPrefab(bulletPrefab, NetworkHash128.Parse(bulletPrefab.name));
        ClientScene.RegisterPrefab(_lineRenderer.gameObject, NetworkHash128.Parse(_lineRenderer.gameObject.name));
        //FindObjectOfType<customNetworkManager>().OnClientConnect(mConnection);
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
            _transform = this.transform;
            _rigidbody = this.GetComponent<Rigidbody>();
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
            //_rigidbody.velocity = (_cameraTransform.transform.forward * Input.GetAxis("Vertical")) + (_cameraTransform.right * Input.GetAxisRaw("Horizontal")) * speed;
            _rigidbody.velocity = (_transform.forward * Input.GetAxis("Vertical") + _transform.right * Input.GetAxis("Horizontal")) * speed;
            //_rigidbody.velocity = new Vector3(_transform.forward.x + Input.GetAxis("Vertical") * speed, _rigidbody.velocity.y, _transform.forward.z + Input.GetAxis("Horizontal") * speed);
            _transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * lookSensitivity);
            _transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * lookSensitivity, Space.World);

            if (Input.GetMouseButtonDown(0))
            {
                CmdHitScan();
                //CmdSpawnBullet();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pSender.SendScore(5, Vector3.zero, 5);
            }
        }
        //if not local player print out my network connection
        if (!isLocalPlayer)
        {
            //  Debug.Log(mConnection);
        }
    }

    [Command]
    public void CmdSpawnBullet()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, _transform.position + _transform.forward * 2, _transform.rotation);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(_transform.forward * 100, ForceMode.Acceleration);
        NetworkServer.Spawn(spawnedBullet);
    }


    [Command]
    public void CmdHitScan()
    {
        Ray ray = new Ray(_transform.position, _transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * int.MaxValue, Color.red, 0.5f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, int.MaxValue))
        {
            hit.transform.GetComponent<Rigidbody>().AddForce(_transform.forward * 5000, ForceMode.Acceleration);
            if (hit.transform.GetComponent<PlayerController>())
            {
                hit.transform.GetComponent<PlayerController>().HitPlayer(1);
            }
            //_rigidbody.velocity = Vector3.zero;
        }
        GameObject lrender = Instantiate(_lineRenderer.gameObject, _transform.position, Quaternion.identity/* _transform.rotation*/);
        lrender.GetComponent<LineRenderer>().SetPosition(0, hit.point);
        lrender.GetComponent<LineRenderer>().SetPosition(1, ray.origin);
        //NetworkServer.Spawn(lineRender);
    }

    public void SpawnBullet()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, _transform.position + _transform.forward * 2, _transform.rotation);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(_transform.forward * 100, ForceMode.Acceleration);
    }

    public void HitPlayer(int a_iDamage)
    {
        miHealth -= a_iDamage;
    }

    private void OnDisable() //Once im disabled
    {
        if (mainCamera)
        {
            //turn the main camera back on
            mainCamera.gameObject.SetActive(true);
        }
    }

    void OnConnectedToServer()
    {
        Debug.LogError("Player connected to server");
    }
}
