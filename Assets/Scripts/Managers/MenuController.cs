using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private OVRRaycaster raycaster;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
    }

    public void PressButtonUnderCursor(Vector2 position)
    {
        var e = new PointerEventData(eventSystem);
        e.pointerId = -1;
        e.position = position;
        var res = new List<RaycastResult>();
        raycaster.Raycast(e, res);

        var btn = res
            .Select(r => r.gameObject.GetComponent<Button>())
            .FirstOrDefault(b => b != null);
        if (btn != null)
            ExecuteEvents.Execute(btn.gameObject, e, ExecuteEvents.submitHandler);
    }
}
