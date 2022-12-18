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


	private OVRRaycaster _uiObject;
	private Vector3 _hitPos;

    private void Start()
    {
		linePointer.enabled = true;
		linePointer.positionCount = 2;
	}

    void FixedUpdate()
	{
		Ray ray = new Ray(rayTransform.position, rayTransform.forward);
		RaycastHit hit;

		linePointer.SetPosition(0, ray.origin);
		if (Physics.Raycast(ray, out hit, rayDrawDistance))
		{
			linePointer.SetPosition(1, hit.point);
            _uiObject = hit.rigidbody?.GetComponent<OVRRaycaster>();
			if (_uiObject == null) return;
            _hitPos = _uiObject.transform.InverseTransformPoint(hit.point);

        }
		else
		{
			linePointer.SetPosition(1, ray.origin + ray.direction * rayDrawDistance);
		}
	}

	public void ClickUI()
	{
        var e = new PointerEventData(EventSystem.current);
		e.position = _hitPos;

        var res = new List<RaycastResult>();
        _uiObject?.Raycast(e, res);

        var btn = res
            .Select(r => r.gameObject.GetComponent<Button>())
            .FirstOrDefault(b => b != null);
        if (btn != null)
            ExecuteEvents.Execute(btn.gameObject, e, ExecuteEvents.submitHandler);
    }
}
