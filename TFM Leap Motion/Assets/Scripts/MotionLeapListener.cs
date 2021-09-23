using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class MotionLeapListener : MonoBehaviour
{
    private Leap.Controller controller = null;
    public static float thumbDistance = 40;
    public Frame frame = null;
    public int fingers = 0;
    public int hands = 0;
    public float handPitch = 0.0F;
    public float handRoll = 0.0F;
    public float handYaw = 0.0F;
    public Leap.Hand firstHand;
    public Leap.Hand secondHand;
    public Vector firstHandPosition = Vector.Zero;
    public Vector secondHandPosition = Vector.Zero;
    public Vector firstHandFingerPosition = Vector.Zero;
    public Vector secondHandFingerPosition = Vector.Zero;
    public Vector firstHandDirection = Vector.Zero;
    public Vector secondHandDirection = Vector.Zero;
    public Vector firstFingerDirection = Vector.Zero;
    public Vector secondFingerDirection = Vector.Zero;
    public Vector firstHandVelocity = Vector.Zero;
    public Vector secondHandVelocity = Vector.Zero;
    public Vector thumbTipFirstHandPosition = Vector.Zero;
    public Vector indexTipFirstHandPosition = Vector.Zero;
    public Vector ringTipFirstHandPosition = Vector.Zero;
    public Vector middleTipFirstHandPosition = Vector.Zero;
    public Vector pinkyTipFirstHandPosition = Vector.Zero;
    public Vector thumbTipSecondHandPosition = Vector.Zero;
    public Vector indexTipSecondHandPosition = Vector.Zero;
    public Vector ringTipSecondHandPosition = Vector.Zero;
    public Vector middleTipSecondHandPosition = Vector.Zero;
    public Vector pinkyTipSecondHandPosition = Vector.Zero;
    public Vector thumbTipFirstHandVelocity = Vector.Zero;
    public Vector indexTipFirstHandVelocity = Vector.Zero;
    public Vector ringTipFirstHandVelocity = Vector.Zero;
    public Vector middleTipFirstHandVelocity = Vector.Zero;
    public Vector pinkyTipFirstHandVelocity = Vector.Zero;
    public Vector thumbTipSecondHandVelocity = Vector.Zero;
    public Vector indexTipSecondHandVelocity = Vector.Zero;
    public Vector ringTipSecondHandVelocity = Vector.Zero;
    public Vector middleTipSecondHandVelocity = Vector.Zero;
    public Vector pinkyTipSecondHandVelocity = Vector.Zero;
    public float firstHandGrabStrength;
    public float secondHandGrabStrength;
    public Leap.Finger thumb = null;    
    public long timestamp = 0;
    public static bool connected = false;


    public bool refresh()
    {
        try
        {
            if (controller == null) controller = new Leap.Controller();

            connected = controller.IsConnected && controller.Devices.Count > 0;

            if (connected)
            {
                frame = controller.Frame();
                fingers = 10;
                hands = frame.Hands.Count;
                timestamp = frame.Timestamp;
                if (frame.Hands.Count > 0)
                {
                    firstHandPosition = frame.Hands[0].PalmPosition;
                    if (frame.Hands.Count == 1)
                    {
                        firstHandGrabStrength = frame.Hands[0].GrabStrength;
                        firstHand = frame.Hands[0];
                        thumbTipFirstHandPosition = frame.Hands[0].Fingers[0].TipPosition;
                        indexTipFirstHandPosition = frame.Hands[0].Fingers[1].TipPosition;
                        ringTipFirstHandPosition = frame.Hands[0].Fingers[2].TipPosition;
                        middleTipFirstHandPosition = frame.Hands[0].Fingers[3].TipPosition;
                        pinkyTipFirstHandPosition = frame.Hands[0].Fingers[4].TipPosition;
                        firstHandPosition = frame.Hands[0].PalmPosition;
                        firstHandDirection = frame.Hands[0].Direction;
                        firstHandFingerPosition = frame.Hands[0].Fingers[0].TipPosition;
                        firstFingerDirection = frame.Hands[0].Fingers[0].Direction;
                        firstHandVelocity = frame.Hands[0].PalmVelocity;
                    }
                    else if (frame.Hands.Count == 2)
                    {
                        firstHandGrabStrength = frame.Hands[0].GrabStrength;
                        secondHandGrabStrength = frame.Hands[1].GrabStrength;
                        firstHand = frame.Hands[0];
                        secondHand = frame.Hands[1];
                        thumbTipSecondHandPosition = frame.Hands[1].Fingers[0].TipPosition;
                        indexTipSecondHandPosition = frame.Hands[1].Fingers[1].TipPosition;
                        ringTipSecondHandPosition = frame.Hands[1].Fingers[2].TipPosition;
                        middleTipSecondHandPosition = frame.Hands[1].Fingers[3].TipPosition;
                        pinkyTipSecondHandPosition = frame.Hands[1].Fingers[4].TipPosition;
                        secondHandPosition = frame.Hands[1].PalmPosition;
                        secondHandDirection = frame.Hands[1].Direction;
                        secondHandFingerPosition = frame.Hands[1].Fingers[0].TipPosition;
                        secondFingerDirection = frame.Hands[1].Fingers[0].Direction;
                        firstHandVelocity = frame.Hands[0].PalmVelocity;
                        secondHandVelocity = frame.Hands[1].PalmVelocity;
                    }
                    Vector normal = frame.Hands[0].PalmNormal;
                    Vector direction = frame.Hands[0].Direction;
                    handPitch = (float)direction.Pitch * 180.0f / (float)System.Math.PI;
                    handRoll = (float)normal.Roll * 180.0f / (float)System.Math.PI;
                    handYaw = (float)direction.Yaw * 180.0f / (float)System.Math.PI;
                    thumb = null;
                    foreach (Leap.Finger finger in frame.Hands[0].Fingers)
                    {
                        if (thumb != null && finger.TipPosition.x < thumb.TipPosition.x && finger.TipPosition.x < firstHandPosition.x)
                            thumb = finger;

                        else if (thumb == null && finger.TipPosition.x < firstHandPosition.x - thumbDistance)
                            thumb = finger;
                    }
                }
            }
            else
            {
                fingers = 0;
                hands = 0;
                handPitch = 0.0F;
                handRoll = 0.0F;
                handYaw = 0.0F;
                firstHandPosition = Vector.Zero;
                secondHandPosition = Vector.Zero;
                firstHandFingerPosition = Vector.Zero;
                secondHandFingerPosition = Vector.Zero;
                firstHandDirection = Vector.Zero;
                secondHandDirection = Vector.Zero;
                firstFingerDirection = Vector.Zero;
                secondFingerDirection = Vector.Zero;
                firstHandVelocity = Vector.Zero;
                secondHandVelocity = Vector.Zero;
                thumbTipFirstHandPosition = Vector.Zero;
                indexTipFirstHandPosition = Vector.Zero;
                ringTipFirstHandPosition = Vector.Zero;
                middleTipFirstHandPosition = Vector.Zero;
                pinkyTipFirstHandPosition = Vector.Zero;
                thumbTipSecondHandPosition = Vector.Zero;
                indexTipSecondHandPosition = Vector.Zero;
                ringTipSecondHandPosition = Vector.Zero;
                middleTipSecondHandPosition = Vector.Zero;
                pinkyTipSecondHandPosition = Vector.Zero;
                thumbTipFirstHandVelocity = Vector.Zero;
                indexTipFirstHandVelocity = Vector.Zero;
                ringTipFirstHandVelocity = Vector.Zero;
                middleTipFirstHandVelocity = Vector.Zero;
                pinkyTipFirstHandVelocity = Vector.Zero;
                thumbTipSecondHandVelocity = Vector.Zero;
                indexTipSecondHandVelocity = Vector.Zero;
                ringTipSecondHandVelocity = Vector.Zero;
                middleTipSecondHandVelocity = Vector.Zero;
                pinkyTipSecondHandVelocity = Vector.Zero;
                thumb = null;
            }

            return true;
        }
        catch (System.Exception e) { UnityEngine.Debug.LogException(e); return false; }
    }

    public UnityEngine.Vector3 rotation(Leap.Hand hand)
    {
        UnityEngine.Vector3 rotationAngles = new UnityEngine.Vector3(0, 0, 0);
        Vector normal = hand.PalmNormal;
        Vector direction = hand.Direction;
        rotationAngles.x = (float)direction.Pitch * 180.0f / (float)System.Math.PI;
        rotationAngles.z = (float)normal.Roll * 180.0f / (float)System.Math.PI;
        rotationAngles.y = (float)direction.Yaw * 180.0f / (float)System.Math.PI;
        return rotationAngles;
    }

    public bool checkFist(Leap.Hand hand, float limit)
    {
        return hand.GrabStrength == limit;
    }

}
