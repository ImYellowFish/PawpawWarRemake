using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

public class BreastHelpThruster : BreastComponent {
    public NormalizedCurve helpForceMag_vs_HelpAxis;
    public NormalizedCurve helpForceMag_vs_BreastSpeed;
    public NormalizedCurve helpForceMag_vs_BreastLength;

    public float baseHelpForce;

    // TODO: maintain these things in breast
    [ReadOnly]
    public float currentHelpAxis;
    [ReadOnly]
    public Vector3 currentHelpForce;
    [ReadOnly]
    public float currentBreastSpeed;
    [ReadOnly]
    public float currentBreastLength;

    private Rigidbody breastBody { get { return host.rb_breast; } }
    
    void FixedUpdate()
    {
        currentHelpForce = GetHelpForce();
        breastBody.AddForce(currentHelpForce);
    }

    private Vector3 GetHelpForce()
    {
        currentBreastSpeed = breastBody.velocity.magnitude;
        currentBreastLength = Vector3.Distance(host.tr_breastBody.position, host.tr_breastOrigin.position);

        var mag_helpAxis = helpForceMag_vs_HelpAxis.Evaluate(Mathf.Abs(currentHelpAxis));
        var mag_breastSpeed = helpForceMag_vs_BreastSpeed.Evaluate(currentBreastSpeed);
        var mag_breastLength = helpForceMag_vs_BreastLength.Evaluate(currentBreastLength);

        var dir = breastBody.velocity.normalized;
        return dir * (baseHelpForce * mag_helpAxis * mag_breastSpeed);
    }

    [Space]
    [InspectorButton("OnStopBreastButton")]
    public bool stopBreastButton;

    public void OnStopBreastButton()
    {
        breastBody.velocity = Vector3.zero;
    }
}
