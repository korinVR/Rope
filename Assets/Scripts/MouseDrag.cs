using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour
{
	Vector3 screenPoint;
	Vector3 offset;
	
	bool dragging = false;
	
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(transform.position);
		offset = transform.position - Camera.main.ScreenToWorldPoint(
			new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		
		dragging = true;
	}
	
	void OnMouseUp()
	{
		dragging = false;
	}
	
	void FixedUpdate()
	{
		if (dragging) {
			Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 dest = Camera.main.ScreenToWorldPoint(point) + offset;
			
			rigidbody.AddForce((dest - rigidbody.position) * 50f);
			rigidbody.velocity *= 0.8f;
		}
	}
}


