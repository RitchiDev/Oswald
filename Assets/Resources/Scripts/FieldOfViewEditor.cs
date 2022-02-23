using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponController))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        WeaponController data = (WeaponController)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(data.transform.position, Vector3.up, Vector3.forward, 360f, data.AttackRadius);

        Vector3 viewAngleOne = DirectionFromAngle(data.transform.eulerAngles.y, -data.AttackAngle / 2);
        Vector3 viewAngleTwo = DirectionFromAngle(data.transform.eulerAngles.y, data.AttackAngle / 2);

        Handles.color = Color.red;
        Handles.DrawLine(data.transform.position, data.transform.position + viewAngleOne * data.AttackRadius);
        Handles.DrawLine(data.transform.position, data.transform.position + viewAngleTwo * data.AttackRadius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegress)
    {
        angleInDegress += eulerY;

        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }
}
