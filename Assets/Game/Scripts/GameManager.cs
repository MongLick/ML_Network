using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
	[SerializeField] TMP_Text infoText;
	[SerializeField] float countDownTime;

	private void Start()
	{
		PhotonNetwork.LocalPlayer.SetLoad(true);
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashTable changedProps)
	{
		if(changedProps.ContainsKey(CustomProperty.LOAD))
		{
			if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
			{
				if(PhotonNetwork.IsMasterClient)
				{
					PhotonNetwork.CurrentRoom.SetGameStart(true);
					PhotonNetwork.CurrentRoom.SetGameStartTime(PhotonNetwork.Time);
				}
			}
			else
			{
				infoText.text = $"{PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}";
			}
		}
	}

	public override void OnRoomPropertiesUpdate(PhotonHashTable propertiesThatChanged)
	{
		if(propertiesThatChanged.ContainsKey(CustomProperty.GAMESTARTTIME))
		{
			StartCoroutine(StartTimer());
		}
	}

	private IEnumerator StartTimer()
	{
		double loadTime = PhotonNetwork.CurrentRoom.GetGameStartTime();
		while(PhotonNetwork.Time - loadTime < countDownTime)
		{
			int reamainTime = (int)(countDownTime - (PhotonNetwork.Time - loadTime));
			infoText.text = reamainTime + 1.ToString();
			yield return null;
		}

		infoText.text = "Game Start!";
		yield return new WaitForSeconds(3f);

		infoText.text = "";
	}

	private int PlayerLoadCount()
	{
		int loadCount = 0;
		foreach(Player player in PhotonNetwork.PlayerList)
		{
			if(player.GetLoad())
			{
				loadCount++;
			}
		}
		return loadCount;
	}
}
