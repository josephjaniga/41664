using UnityEngine;
using System.Collections;
using System;
using System.Timers;

public class Door : MonoBehaviour {

	public Vector3 openPosition;
	public Vector3 closePosition;

	public float Proximity = 15f;
	public float doorSpeed = 3.0f;

	public IDoorOpenMechanism mechanism;
	public bool shouldOpen;

	public DoorMechanismTypes type = DoorMechanismTypes.PlayerProximityOpen;

	public string SwitchName = "";

	void Start(){
		switch (type){
			default:
			case DoorMechanismTypes.AlwaysOpen:
				mechanism = new AlwaysOpen();
			break;
			case DoorMechanismTypes.AlwaysClosed:
				mechanism = new AlwaysClosed();
			break;
			case DoorMechanismTypes.PlayerProximityOpen:
				mechanism = new PlayerProximityOpen(gameObject.transform.parent.gameObject, Proximity);
			break;
			case DoorMechanismTypes.PlayerProximityClosed:
				doorSpeed = 12.0f;
				mechanism = new PlayerProximityClosed(gameObject.transform.parent.gameObject, Proximity);
			break;
			case DoorMechanismTypes.TimedMechanism:
				doorSpeed = 6.0f;
				mechanism = new TimedMechanism(Time.time, 4f);
			break;
			case DoorMechanismTypes.SwitchMechanism:
				mechanism = new SwitchMechanism(SwitchName);
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		shouldOpen = mechanism.targetPosition();
		if ( mechanism.targetPosition() ) {
			transform.localPosition =  Vector3.MoveTowards(transform.localPosition, openPosition, Time.deltaTime * doorSpeed);
		} else {
			transform.localPosition =  Vector3.MoveTowards(transform.localPosition, closePosition, Time.deltaTime * doorSpeed);
		}
	}

}

public enum DoorMechanismTypes{
	AlwaysOpen,
	AlwaysClosed,
	PlayerProximityOpen,
	PlayerProximityClosed,
	TimedMechanism,
	SwitchMechanism
}

public interface IDoorOpenMechanism{
	bool targetPosition();
}

public class AlwaysOpen : IDoorOpenMechanism{
	public bool targetPosition(){
		return true;
	}
}

public class AlwaysClosed : IDoorOpenMechanism{
	public bool targetPosition(){
		return false;
	}
}

public class PlayerProximityOpen : IDoorOpenMechanism{

	public float distance = 40f;
	public GameObject playerGO;
	public Transform player;
	public Transform door;

	public PlayerProximityOpen(GameObject doorGO, float dist){
		distance = dist;
		door = doorGO.transform;
		playerGO = GameObject.Find("Player");
		if ( playerGO != null ){
			player = playerGO.transform;
		}
	}

	public bool targetPosition(){
		bool temp = false;
		if ( player != null ){
			if ( Vector3.Distance(player.position, door.position) < distance ){
				temp = true;
			}
		}
		return temp;
	}
}

public class PlayerProximityClosed : IDoorOpenMechanism {

	public float distance = 40f;
	public GameObject playerGO;
	public Transform player;
	public Transform door;

	public PlayerProximityClosed(GameObject doorGO, float dist){
		distance = dist;
		door = doorGO.transform;
		playerGO = GameObject.Find("Player");
		if ( playerGO != null ){
			player = playerGO.transform;
		}
	}

	public bool targetPosition(){
		bool temp = true;
		if ( player != null ){
			if ( Vector3.Distance(player.position, door.position) < distance ){
				temp = false;
			}
		}
		return temp;
	}
}

public class TimedMechanism : IDoorOpenMechanism {

	public bool target = false;

	public int startTime;
	public int interval = 4000;

	public Timer clock;

	public TimedMechanism(float currentTime, float intervalF=4f){
		interval = Mathf.RoundToInt(intervalF * 1000);
		startTime = Mathf.RoundToInt(( currentTime + UnityEngine.Random.Range(0f, 1f) ) * 1000);
		clock = new System.Timers.Timer(interval);
		clock.Elapsed += InvertTarget;
		clock.Enabled = true;
	}

	public void InvertTarget(object source, ElapsedEventArgs e){
		target = !target;
	}

	public bool targetPosition(){
		return target;
	}
}

public class SwitchMechanism : IDoorOpenMechanism {
	public Switch doorSwitch;
	public SwitchMechanism(string switchName){
		doorSwitch = GameObject.Find(switchName).GetComponent<Switch>();
	}
	public bool targetPosition(){
		return doorSwitch.active;
	}
}