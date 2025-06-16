using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private float _speed;
    private float _range;
    private float _duration;

    private Vector3 _startPos;
    private Vector3 _direction;

    public void Initialize(float speed, float range, Vector3 direction)
    {
        _speed = speed;
        _range = range;

        _startPos = transform.position;
        _direction = direction;
    }

    private void Update()
    {
        float moveStep = _speed * Time.deltaTime;
        transform.position += _direction * moveStep;
    }
}
