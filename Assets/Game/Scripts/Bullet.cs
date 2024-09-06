using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float moveSpeed;

	public Vector3 Velocity { get{ return rigid.velocity; } }

	private void Awake()
	{
		rigid.velocity = transform.forward * moveSpeed;
		Destroy(gameObject, 5f);
	}
}
