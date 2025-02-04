﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainRooms : MonoBehaviour
{
    // Start is called before the first frame update
    public static MainRooms instance;
    public enum MainRoomType
    {
        WARM,
        HEALTHY,
        SAD,
        DESPERATE,
        HAPPY,
        LONELY

    };
    public float rotationSpeed = 1.8f;
    public float resetLimit = 1.9f;

    private AudioSource audioSource;
    private Quaternion targetRotation;
    private bool rotating = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        if(rotating == true)
        {
            if(audioSource.isPlaying == false)
            {
                audioSource.Play();
                Debug.Log("Started playing");
            }
        }

        if (rotating && angle < resetLimit)
        {
            Camera.main.GetComponent<CameraShake>().Shake();
            transform.rotation = targetRotation;
            rotating = false;
            audioSource.Stop();
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }


    public static void Rotate()
    {
        if (instance.rotating == true)
            return;

        Vector3 direction = Vector3.right;

        float verticalValue = Input.GetAxis("Vertical");
        float horizontalValue = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalValue) < Mathf.Abs(verticalValue))
        {

            if (verticalValue < 0)
            {
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.right;
            }
        }
        else
        {
            if (horizontalValue < 0)
            {
                direction = Vector3.forward;
            }
            else
            {
                direction = Vector3.back;
            }
        }

        instance.targetRotation = Quaternion.AngleAxis(90, direction) * instance.transform.rotation;
        instance.rotating = true;
    }

    public static bool IsRotating()
    {
        return (SceneManager.GetActiveScene().name.Equals("MainRoom") || SceneManager.GetActiveScene().name.Equals("StartGame")) && instance.rotating;
    }

}
