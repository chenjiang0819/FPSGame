using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyState;

public class Context
{
    // public GameObject gameObject;
    public Transform transform;
    // public Animator animator;
    // public Rigidbody physics;
    public NavMeshAgent agent;
    // public SphereCollider sphereCollider;
    // public BoxCollider boxCollider;
    // public CapsuleCollider capsuleCollider;
    // public CharacterController characterController;

    // Add other game specific systems here
    public AIAgent aIAgent;

    public static Context CreateFromGameObject(GameObject gameObject)
    {
        // Fetch all commonly used components
        Context context = new Context();
        // context.gameObject = gameObject;
        context.transform = gameObject.transform;
        // context.animator = gameObject.GetComponent<Animator>();
        // context.physics = gameObject.GetComponent<Rigidbody>();
        context.agent = gameObject.GetComponent<NavMeshAgent>();
        // context.sphereCollider = gameObject.GetComponent<SphereCollider>();
        // context.boxCollider = gameObject.GetComponent<BoxCollider>();
        // context.capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        // context.characterController = gameObject.GetComponent<CharacterController>();

        // Add whatever else you need here...
        context.aIAgent = gameObject.GetComponent<AIAgent>();

        return context;
    }
}
