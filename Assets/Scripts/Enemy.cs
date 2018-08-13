using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public Boolean AddRotationOnMove = true;

    public bool duck;

    [SerializeField]
    public int Health;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private int GoldReward;

    // Use this for initialization
    void Start()
    {
        MovementController script = gameObject.AddComponent<MovementController>();
        script.WalkSpeed = Speed;
        script.RotationSpeed = RotationSpeed;
        script.AddRotation = AddRotationOnMove;

        GameObject target = GameObject.FindGameObjectWithTag("Target");
        Vector3 correction = (duck) ? Vector3.up * 10f : Vector3.zero;
        script.NavigateTo(target.transform.position + correction);
    }

    void Update()
    {
        // Handle death
        if (Health <= 0)
        {
            WorldManager.Instance.Inventory.Gold += GoldReward;

            // Set GoldReward to zero in case this runs more than once
            GoldReward = 0;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        Health = Math.Max(Health - damage, 0);
    }
}
