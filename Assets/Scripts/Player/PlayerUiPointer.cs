using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUiPointer : MonoBehaviour
{
	[Tooltip("Object which points with Z axis. E.g. CentreEyeAnchor from OVRCameraRig")]
	public Transform rayTransform;
	[Header("Visual Elements")]
	[Tooltip("Line Renderer used to draw selection ray.")]
	public LineRenderer linePointer = null;
	[Tooltip("Visually, how far out should the ray be drawn.")]
	public float rayDrawDistance = 2.5f;

    private void Start()
    {
		linePointer.enabled = true;
		linePointer.positionCount = 2;
	}

    void Update()
	{
		Ray ray = new Ray(rayTransform.position, rayTransform.forward);
		RaycastHit hit;

		linePointer.SetPosition(0, ray.origin);
		if (Physics.Raycast(ray, out hit, rayDrawDistance))
		{
			linePointer.SetPosition(1, hit.point);
			var rc = hit.rigidbody.GetComponent<OVRRaycaster>();
			var e = new PointerEventData(EventSystem.current);
			e.position = rc.transform.InverseTransformPoint(hit.point);

			var res = new List<RaycastResult>();
			rc.Raycast(e, res);

			if (InputManager.uiClickPressed) 
			{
				var btn = res
					.Select(r => r.gameObject.GetComponent<Button>())
					.FirstOrDefault(b => b != null);
				if (btn != null)
					ExecuteEvents.Execute(btn.gameObject, e, ExecuteEvents.submitHandler);
			}
		}
		else
		{
			linePointer.SetPosition(1, ray.origin + ray.direction * rayDrawDistance);
		}
	}
}
