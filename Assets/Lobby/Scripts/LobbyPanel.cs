using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;

    private Dictionary<string, RoomEntry> roomDictionary;

	private void Awake()
	{
        for(int i = 0; i < roomContent.childCount; i++)
        {
            Destroy(roomContent.GetChild(i).gameObject);
        }
		roomDictionary = new Dictionary<string, RoomEntry>();
	}

	private void OnDisable()
	{
		roomDictionary.Clear();
	}

	public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if(roomInfo.RemovedFromList || roomInfo.IsOpen == false || roomInfo.IsVisible == false)
            {
                RoomEntry roomEnrty = roomDictionary[roomInfo.Name];
                roomDictionary.Remove(roomInfo.Name);
                Destroy(roomEnrty.gameObject);

                continue;
            }

			if (roomDictionary.ContainsKey(roomInfo.Name))
			{
                RoomEntry roomEntry = roomDictionary[roomInfo.Name];
                roomEntry.SetRoomInfo(roomInfo);
			}

            else
            {
                RoomEntry roomEnrty = Instantiate(roomEntryPrefab, roomContent);
                roomEnrty.SetRoomInfo(roomInfo);
                roomDictionary.Add(roomInfo.Name, roomEnrty);
            }
		}
    }
}
