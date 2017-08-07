using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LerpType {
    Normal,
    EaseOut,
    EaseIn,
    Exponential,
    SmoothStep
};

public class LerpController : Singleton<LerpController> {
    public float ApplyCurveToLerpTime(float t, LerpType type) {
        switch (type) {
            case LerpType.EaseOut:
                return Mathf.Sin(t * Mathf.PI * 0.5f);
            case LerpType.EaseIn:
                return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
            case LerpType.Exponential:
                return t * t;
            case LerpType.SmoothStep:
                return t * t * (3f - 2f * t);
        }

        return t;
    }

}
