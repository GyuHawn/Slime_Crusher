using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private StageManager stageManager;
    private PlayerController playerController;

    public float rotationSpd;
    public float moveSpeed;

    private void Awake()
    {
        if (!stageManager)
            stageManager = FindObjectOfType<StageManager>();
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (stageManager.mainStage == 4)
        {
            transform.Rotate(Vector3.forward * rotationSpd * Time.deltaTime);
        }
        else if (stageManager.mainStage == 6)
        {
            MoveToLastClickPosition();
        }
    }

    private void MoveToLastClickPosition()
    {
        Vector3 targetPosition = playerController.GetLastClickPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
