using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviourPun
{
    [SerializeField] Rigidbody rigid;

	private void Awake()
	{
		if(photonView.InstantiationData != null)
		{
			Vector3 force = (Vector3)photonView.InstantiationData[0];
			Vector3 torque = (Vector3)photonView.InstantiationData[1];

			rigid.AddForce(force, ForceMode.Impulse);
			rigid.AddTorque(torque, ForceMode.Impulse);
		}
	}

	private void Update()
	{
		if (!photonView.IsMine)
		{
			return;
		}

		if(transform.position.sqrMagnitude > 40000)
		{
			PhotonNetwork.Destroy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		/*if(collision.collider.gameObject)
		{
			if(photonView.IsMine == false)
			{
				return;
			}
		}*/
	}
}
