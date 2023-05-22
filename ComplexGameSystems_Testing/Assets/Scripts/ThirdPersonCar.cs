using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCar : MonoBehaviour {

	[SerializeField] private Transform m_followTransform;
	[SerializeField, Range(5, 20)] private float m_followDistance = 10;
	[SerializeField, Range(1, 10)] private float m_followHeight = 5;
	[SerializeField, Range(1, 10)] private float m_followSpeed = 2;
	[SerializeField, Range(1, 10)] private float m_turnSpeed = 5;
	

	private void FixedUpdate()
	{
		//Move to car
		Vector3 _targetPos = m_followTransform.position + (m_followTransform.forward * -m_followDistance) + (m_followTransform.up * m_followHeight);
		transform.position = Vector3.Lerp(transform.position, _targetPos, m_followSpeed * Time.deltaTime);

		//Look at car
		Vector3 _lookDirection = (new Vector3(m_followTransform.position.x, m_followTransform.position.y, m_followTransform.position.z) + (m_followTransform.up * 2)) - transform.position;
		Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, _rot, m_turnSpeed * Time.deltaTime);
	}

}
