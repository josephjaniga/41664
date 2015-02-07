using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Vector3 openPosition;
	public Vector3 closePosition;

	public float Proximity = 15f;
	public float doorSpeed = 3.0f;

	public IDoorOpenMechanism mechanism;
	public bool shouldOpen;

	public DoorMechanismTypes type = DoorMechanismTypes.PlayerProximityOpen;

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
				doorSpeed = 6.0f;
				mechanism = new PlayerProximityClosed(gameObject.transform.parent.gameObject, Proximity);
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
	PlayerProximityClosed
}

public interface IDoorOpenMechanism
{
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
	public Transform player;
	public Transform door;

	public PlayerProximityOpen(GameObject doorGO, float dist){
		distance = dist;
		door = doorGO.transform;
		if ( GameObject.Find("Player") != null ){
			player = GameObject.Find("Player").transform;
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

public class PlayerProximityClosed : IDoorOpenMechanism{

	public float distance = 40f;
	public Transform player;
	public Transform door;

	public PlayerProximityClosed(GameObject doorGO, float dist){
		distance = dist;
		door = doorGO.transform;
		if ( GameObject.Find("Player") != null ){
			player = GameObject.Find("Player").transform;
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