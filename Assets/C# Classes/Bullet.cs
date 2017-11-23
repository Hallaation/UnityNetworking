using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    private Transform _transform;
    private Rigidbody _rigidbody;
    private ParticleSystem _ps;
    // Use this for initialization

    void Start()
    {
        _transform = this.transform;
        _rigidbody = this.GetComponent<Rigidbody>();
        _ps = this.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(_transform.position, _transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, _rigidbody.velocity.magnitude * 2))
        {
            _rigidbody.velocity = Vector3.zero;
            _ps.Play();
        }
    }
}

public class RegisterHostMessage : MessageBase
{
    public string gameName;
    public string comment;
    public bool passwordProtected;
}

public class MasterClient
{
    public NetworkClient client;
    public const short RegisterHostMsgId = 888;

    public void RegisterHost(string name)
    {
        RegisterHostMessage msg = new RegisterHostMessage();
        msg.gameName = name;
        msg.comment = "test";
        msg.passwordProtected = false;

        client.Send(RegisterHostMsgId, msg);
    }

}