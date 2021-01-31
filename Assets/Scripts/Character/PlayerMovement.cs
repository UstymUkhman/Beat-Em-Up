﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private new CharacterAnimation animation;
    private float defaulRotation = 0.0f;
    private CharacterHealth health;
    private Rigidbody body;

    public float horizontalSpeed = 2.0f;
    private bool isKnockedDown = false;
    public float verticalSpeed = 1.5f;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        health = GetComponent<CharacterHealth>();

        defaulRotation = gameObject.transform.eulerAngles.y;
        animation = GetComponentInChildren<CharacterAnimation>();
    }

    private void Update()
    {
        UpdatePlayerPosition();
        UpdatePlayerRotation();
        UpdatePlayerAnimation();
    }

    private void UpdatePlayerPosition()
    {
        if (knockDown && Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(EnableEnemyManager(0.5f));
            animation.StandUp();
        }
    }

    private IEnumerator EnableEnemyManager(float wait)
    {
        yield return new WaitForSeconds(wait);
        health.ToggleEnemyManager(true);
    }

    private void UpdatePlayerRotation()
    {
        float horizontalAxis = Input.GetAxisRaw(Axis.HORIZONTAL);
        float verticalAxis = Input.GetAxisRaw(Axis.VERTICAL);

        bool horizontalRotation = horizontalAxis != 0.0f;
        bool verticalRotation = verticalAxis != 0.0f;

        float rotation = 0.0f;

        if (verticalRotation)
        {
            rotation += verticalAxis == 1.0f ? -180.0f : -360.0f;
        }

        if (horizontalRotation)
        {
            float currentRotation = verticalAxis == -1.0f ? 270.0f : -90.0f;

            rotation += horizontalAxis == -1.0f ? -270.0f :
                verticalRotation ? currentRotation : -90.0f;
        }

        if (verticalRotation || horizontalRotation)
        {
            int directions = (verticalRotation && horizontalRotation) ? 2 : 1;
            transform.rotation = Quaternion.Euler(0.0f, rotation / directions, 0.0f);
        }
    }

    private void UpdatePlayerAnimation()
    {
        animation.Walk(
            Input.GetAxisRaw(Axis.HORIZONTAL) != 0 ||
            Input.GetAxisRaw(Axis.VERTICAL) != 0
        );
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector3(
            Input.GetAxisRaw(Axis.HORIZONTAL) * -horizontalSpeed,
            body.velocity.y,
            Input.GetAxisRaw(Axis.VERTICAL) * -verticalSpeed
        );
    }

    public bool knockDown
    {
        get {
            return isKnockedDown;
        }

        set {
            isKnockedDown = value;
        }
    }
}
