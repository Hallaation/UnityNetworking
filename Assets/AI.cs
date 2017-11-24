using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI : MonoBehaviour
{
    NavMeshAgent mAgent;
    
    Transform mTransform;
    Rigidbody mRigidBody;
    // Use this for initialization
    void Start()
    {
        mAgent = this.GetComponent<NavMeshAgent>();
        mTransform = this.transform;
        mRigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
