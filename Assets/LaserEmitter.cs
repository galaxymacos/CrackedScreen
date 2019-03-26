using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private int damage = 10;

    [SerializeField] private float startAngle = 0f;

    [SerializeField] private float rotateSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        startAngle = (startAngle + rotateSpeed * Time.deltaTime)%360;
        var position = transform.position;
        lr.SetPosition(0,position);
        Physics.Raycast(position, new Vector3(Mathf.Sin(startAngle),0,Mathf.Cos(startAngle)),out var hitInfo,100f);
        if (hitInfo.collider)
        {
            lr.SetPosition(1,hitInfo.point);
            if (hitInfo.collider.gameObject == PlayerProperty.player)
            {
                PlayerProperty.playerClass.TakeDamage(damage);
                PlayerProperty.playerClass.GetKnockOff(hitInfo.point);
                
            }
        }
        else
        {
            lr.SetPosition(1,new Vector3(Mathf.Sin(startAngle),0,Mathf.Cos(startAngle))*5000);
        }
    }
}
