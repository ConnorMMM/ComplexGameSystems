using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCar : MonoBehaviour {

	[SerializeField] private Transform m_followTarget;
	[SerializeField, Range(5, 20)] private float m_followDistance = 12;
	[SerializeField, Range(1, 10)] private float m_followHeight = 6;
	[SerializeField, Range(1, 10)] private float m_followSpeed = 5;
	[SerializeField, Range(1, 10)] private float m_turnSpeed = 5;
	

	private void FixedUpdate()
	{
		//Move to car
		Vector3 _targetPos = m_followTarget.position + (m_followTarget.forward * -m_followDistance) + (m_followTarget.up * m_followHeight);
		transform.position = Vector3.Lerp(transform.position, _targetPos, m_followSpeed * Time.deltaTime);

		//Look at car
		Vector3 _lookDirection = (new Vector3(m_followTarget.position.x, m_followTarget.position.y, m_followTarget.position.z) + (m_followTarget.up * 2)) - transform.position;
		Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, _rot, m_turnSpeed * Time.deltaTime);
	}

	public void SetFollowTarget(Transform _target)
	{
		m_followTarget = _target;
	}

}
