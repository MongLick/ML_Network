using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
	[SerializeField] Rigidbody rigid;
	[SerializeField] List<Color> colorList;
	[SerializeField] PlayerInput input;
	[SerializeField] Bullet bulletPrefab;
	[SerializeField] float fireCoolTime;
	[SerializeField] float moveSpeed;
	[SerializeField] float movePower;
	[SerializeField] float maxSpeed;
	[SerializeField] float rotateSpeed;
	[SerializeField] float currentSpeed;
	[SerializeField] int fireCount;
	private Vector3 moveDir;

	private float lastFireTime = float.MinValue;

	private void Awake()
	{
		if (photonView.IsMine == false)
		{
			Destroy(input);
		}

		SetPlayerColor();
	}

	private void Update()
	{
		Rotate();
	}

	private void FixedUpdate()
	{
		Accelate();
	}

	private void Accelate()
	{
		rigid.AddForce(moveDir.z * transform.forward * movePower, ForceMode.Force);
		if (rigid.velocity.sqrMagnitude > maxSpeed * maxSpeed)
		{
			rigid.velocity = rigid.velocity.normalized * maxSpeed;
		}
		currentSpeed = rigid.velocity.magnitude;
	}

	private void Rotate()
	{
		transform.Rotate(Vector3.up, moveDir.x * rotateSpeed * Time.deltaTime);
	}

	private void OnMove(InputValue value)
	{
		moveDir.x = value.Get<Vector2>().x;
		moveDir.z = value.Get<Vector2>().y;
	}

	private void OnFire(InputValue value)
	{
		photonView.RPC("RequestCreateBullet", RpcTarget.MasterClient);
	}

	[PunRPC]
	private void RequestCreateBullet()
	{
		if (Time.time < lastFireTime + fireCoolTime)
		{
			return;
		}
		lastFireTime = Time.time;
		photonView.RPC("ResultCreateBullet", RpcTarget.AllViaServer, transform.position, transform.rotation);
	}

	[PunRPC]
	private void ResultCreateBullet(Vector3 psition, Quaternion rotation, PhotonMessageInfo info)
	{
		float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

		fireCount++;
		Bullet bullet = Instantiate(bulletPrefab, psition, rotation);
		bullet.transform.position += bullet.Velocity * lag * 0.001f;
	}

	private void SetPlayerColor()
	{
		int playerNumber = photonView.Owner.GetPlayerNumber();
		if (colorList == null || colorList.Count <= playerNumber)
		{
			return;
		}

		Renderer render = GetComponent<Renderer>();
		render.material.color = colorList[playerNumber];

		if (photonView.IsMine)
		{
			render.material.color = Color.green;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(currentSpeed);
		}
		else
		{
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
			currentSpeed = (float)stream.ReceiveNext();
		}
	}
}
