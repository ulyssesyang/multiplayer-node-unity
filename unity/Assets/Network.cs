using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{
	static SocketIOComponent socket;

	public GameObject playerPrefab;

	Dictionary<string, GameObject> players;

	void Start()
	{
		socket = GetComponent<SocketIOComponent> ();
		socket.On ("open", OnConnected);
		socket.On ("spawn", OnSpawned);
		socket.On ("move", OnMove);

		players = new Dictionary<string, GameObject> ();
	}

	void OnConnected(SocketIOEvent e)
	{
		Debug.Log ("server connected...");
	}

	void OnSpawned(SocketIOEvent e)
	{
		Debug.Log ("player spawned..."+e.data);
		var player = Instantiate (playerPrefab);

		players.Add (e.data["id"].ToString(), player);
	}

	void OnMove (SocketIOEvent e)
	{
		var id = e.data ["id"].ToString();
		var player = players [id];

		var x = GetFloatFromJson (e.data, "x");
		var z = GetFloatFromJson (e.data, "y");
		var pos = new Vector3 (x, 0, z);


		var navigatePos = player.GetComponent<NavigatePosition> ();
		navigatePos.NavigateTo (pos);
	}

	void OnRegistered (SocketIOEvent e)
	{
		
	}

	float GetFloatFromJson(JSONObject data, string key)
	{
		return float.Parse(data[key].ToString().Replace("\"",""));
	}
}
