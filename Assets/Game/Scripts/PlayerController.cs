using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
	[SerializeField] List<Color> colorList;
	[SerializeField] PlayerInput input;
    [SerializeField] float moveSpeed;
	[SerializeField] int fireCount;
    private Vector3 moveDir;

	private void Awake()
	{
		if(photonView.IsMine == false)
		{
			Destroy(input);
		}

		SetPlayerColor();
	}

	private void Update()
	{
		transform.Translate(moveDir * moveSpeed * Time.deltaTime);
	}

	private void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
		moveDir.z = value.Get<Vector2>().y;
	}

	private void OnFire(InputValue value)
	{
		fireCount++;
	}

	private void SetPlayerColor()
	{
		int playerNumber = photonView.Owner.GetPlayerNumber();
		if(colorList == null || colorList.Count <= playerNumber)
		{
			return;
		}

		Renderer render = GetComponent<Renderer>();
		render.material.color = colorList[playerNumber];

		if(photonView.IsMine)
		{
			render.material.color = Color.green;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.IsWriting)
		{
			stream.SendNext(fireCount);
		}
		else
		{
			fireCount = (int)stream.ReceiveNext();
		}
	}
}
