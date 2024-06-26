using UnityEngine;

public class GrapplingRope: MonoBehaviour
{
    [Header("References")]
    public Grappling grappling;

    [Header("Settings")]
    public int quality = 200; 
    public float damper = 14; 
    public float strength = 800; 
    public float velocity = 15; 
    public float waveCount = 3; 
    public float waveHeight = 1;
    public AnimationCurve affectCurve;

    private Spring spring; 
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    //Called after Update
    private void LateUpdate()
    {
        DrawRope();
    }

    void DrawRope()
    {
        // Dont Draw when not grappling rope
        if (!grappling.IsGrappling())
        {
            currentGrapplePosition = grappling._gunTip.position;

            // reset the simulation
            spring.Reset();

            // reset the positionCount of the lineRenderer
            if (lr.positionCount > 0)
                lr.positionCount = 0;

            return;
        }

        if (lr.positionCount == 0)
        {
            
            spring.SetVelocity(velocity);

            // set the positionCount of the lineRenderer depending on the quality of the rope
            lr.positionCount = quality + 1;
        }

        // set the spring simulation
        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 grapplePoint = grappling.GetGrapplePoint();
        Vector3 gunTipPosition = grappling._gunTip.position;

        // find the upwards direction relative to the rope
        Vector3 up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        // lerp the currentGrapplePositin towards the grapplePoint
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        // loop through all segments of the rope and animate them
        for (int i = 0; i < quality + 1; i++)
        {
            float delta = i / (float)quality;
            // calculate the offset of the current rope segment
            Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);

            // lerp the lineRenderer position towards the currentGrapplePosition + the offset you just calculated
            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}

