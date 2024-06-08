using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _InitialVelocity;
    [SerializeField] float _Angle;
    [SerializeField] LineRenderer _Line;
    [SerializeField] float _Step;
    [SerializeField] Transform _FirePoint;
    [SerializeField] float _Height;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - _FirePoint.position;
            Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);



            Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
            targetPos.z = 0;
            float height = targetPos.y + targetPos.magnitude / 2f;
            height = Mathf.Max(0.01f, height);
            float v0;
            float angle;
            float time;
            CalculatePathWithheight(targetPos, height, out v0, out angle, out time);

            DrawPath(groundDirection.normalized, v0, angle, time, _Step);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopAllCoroutines();
                StartCoroutine(Coroutine_Movement(groundDirection.normalized, v0, angle, time));
            }
        }
    }

    private void DrawPath(Vector3 direction, float v0, float angle, float time, float step)
    {
        step = Mathf.Max(0.01f, step);
        _Line.positionCount = (int)(time / step) + 2;
        int count = 0;
        for (float i=0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _Line.SetPosition(count, _FirePoint.position+ direction*x + Vector3.up * y);
            count++;
        }
        float xfinal = v0 * time * Mathf.Cos(angle);
        float yfinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        _Line.SetPosition(count, _FirePoint.position+ direction * xfinal + Vector3.up * yfinal);
    }

    private float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b + c * c))/ (2 * a);
    }

    private void CalculatePathWithheight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tplus > tmin ? tplus : tmin;

        angle = Mathf.Atan(b * time/xt);
        v0 = b / Mathf.Sin(angle);
    }

    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle); 
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v1 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }

    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time)
    {
        float t = 0;
        while (t < time)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = _FirePoint.position+ direction * x + Vector3.up * y;
            
            t += Time.deltaTime;
            yield return null;
        }
    }
}
