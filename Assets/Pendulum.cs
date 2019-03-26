using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    private Quaternion start, end;

    [SerializeField, Range(0.0f, 360f)] private float angle = 90f;
    [SerializeField, Range(0.0f, 5f)] private float speed = 2f;
    [SerializeField, Range(0.0f, 10.0f)] private float startTime = 0;
    [SerializeField] private int damageToPlayer;

    [SerializeField] private EnemyDetector enemyDetector;

    public bool fullCharge;
    // Start is called before the first frame update
    void Start()
    {
        SetUpStartAndEndAngle();
    }

    private void SetUpStartAndEndAngle()
    {
        print(start+" "+end);
        start = PendulumRotation(angle);
        end = PendulumRotation(-angle);
    }

    private void FixedUpdate()
    {
        if (!fullCharge)
        {
            startTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(start, end, (Mathf.Sin(startTime * speed + Mathf.PI/2)+1.0f)/2.0f);
            
        }
        else
        {
            transform.Rotate(new Vector3(0,0,speed*Time.fixedDeltaTime));
        }


        if (enemyDetector.playerInRange())
        {
            PlayerProperty.playerClass.TakeDamage(damageToPlayer);
            PlayerProperty.playerClass.GetKnockOff(transform.TransformDirection(transform.position));

        }
    }

    Quaternion PendulumRotation(float _angle)
    {
        var pendulumRotation = transform.rotation;
        var angleZ = pendulumRotation.eulerAngles.z + _angle;

        if (angleZ > 180)
        {
            angleZ -= 360;
        }
        else if (angleZ < -180)
        {
            angleZ += 360;
        }
        
        pendulumRotation.eulerAngles = new Vector3(pendulumRotation.eulerAngles.x,pendulumRotation.eulerAngles.y, angleZ);
        return pendulumRotation;
    }

    public void ChangeAngleTo(float newAngle)
    {
        angle = newAngle;
        SetUpStartAndEndAngle();
    }
}
