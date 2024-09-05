using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameManager : MonoBehaviourPunCallbacks
{
	[SerializeField] string debugRoomName;

	private void Start()
	{
		PhotonNetwork.LocalPlayer.NickName = $"TestPlayer {Random.Range(1000, 10000)}";
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 8;
		options.IsVisible = false;
		TypedLobby typedLobby = new TypedLobby("DebugLobby", LobbyType.Default);
		PhotonNetwork.JoinOrCreateRoom(debugRoomName, options, typedLobby);
	}

	public override void OnJoinedRoom()
	{
		StartCoroutine(GameStartDelay());
	}

	private IEnumerator GameStartDelay()
	{
		yield return new WaitForSeconds(1f);
		GameStart();
	}

	public void GameStart()
	{
		Vector2 pos = Random.insideUnitCircle * 30;
		PhotonNetwork.Instantiate("Player", new Vector3(pos.x, pos.y), Quaternion.identity);
	}
}
