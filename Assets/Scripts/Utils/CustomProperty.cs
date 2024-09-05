using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashTaable = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{
	static PhotonHashTaable property = new PhotonHashTaable();

	public const string READY = "Ready";
	public const string LOAD = "Load";

	public static bool GetReady(this Player player)
	{
		PhotonHashTaable customProperty = player.CustomProperties;
		if (customProperty.TryGetValue(READY, out object value))
		{
			return (bool)value;
		}
		else
		{
			return false;
		}
	}

	public static void SetReady(this Player player, bool value)
	{
		property.Clear();
		property[READY] = value;
		player.SetCustomProperties(property);
	}

	public static bool GetLoad(this Player player)
	{
		PhotonHashTaable customProperty = player.CustomProperties;
		if (customProperty.TryGetValue(LOAD, out object value))
		{
			return (bool)value;
		}
		else
		{
			return false;
		}
	}

	public static void SetLoad(this Player player, bool value)
	{
		property.Clear();
		property[LOAD] = value;
		player.SetCustomProperties(property);
	}

	public const string GAMESTART = "GameStart";
	public static bool GetGameStart(this Room room)
	{
		PhotonHashTaable customProperty = room.CustomProperties;
		if (customProperty.TryGetValue(GAMESTART, out object value))
		{
			return (bool)value;
		}
		else
		{
			return false;
		}
	}

	public static void SetGameStart(this Room room, bool value)
	{
		property.Clear();
		property[GAMESTART] = value;
		room.SetCustomProperties(property);
	}

	public const string GAMESTARTTIME = "GameStartTime";

	public static double GetGameStartTime(this Room room)
	{
		PhotonHashTaable customProperty = room.CustomProperties;
		if (customProperty.TryGetValue(GAMESTARTTIME, out object value))
		{
			return (double)value;
		}
		else
		{
			return 0;
		}
	}

	public static void SetGameStartTime(this Room room, double value)
	{
		property.Clear();
		property[GAMESTARTTIME] = value;
		room.SetCustomProperties(property);
	}
}
