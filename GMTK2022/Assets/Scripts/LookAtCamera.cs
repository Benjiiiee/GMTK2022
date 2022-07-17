using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(LookAtConstraint))]
public class LookAtCamera : MonoBehaviour
{
    private LookAtConstraint constraint;

    private void Start() {
        constraint = GetComponent<LookAtConstraint>();
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = Camera.main.transform;
        source.weight = 1f;
        constraint.AddSource(source);
        constraint.constraintActive = true;
    }
}
