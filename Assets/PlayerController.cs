using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab; // The GameObject to spawn as a projectile
    public float moveSpeed = 5f; // The speed at which the player moves
    public float projectileSpeed = 10f; // The speed at which the projectile will move
    public KeyCode dashKey = KeyCode.Space; // The key that the player can press to dash
    public float dashCooldown = 1f; // The amount of time the player has to wait before dashing again
    public float dashSpeed = 10f;

    private bool isDashing = false;
    private Vector3 dashDirection;
    private float dashTimer = 0f;
    void Update()
    {
        // Get input from the user
        float horizontalInput = Input.GetKey("a") ? -1 : Input.GetKey("d") ? 1 : 0;
        float verticalInput = Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0;

        if (isDashing)
        {
            // If the player is dashing move in the dash direction
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            // Stop dashing after a exclusive frame
            isDashing = false;
        }
        else
        {
            // If the player is not dashing, move in the regular direction
            Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            // Check if the player pressed the dash key and the dash timer is up
            if (Input.GetKeyDown(dashKey) && dashTimer <= 0)
            {
                // If the player pressed the dash key, start dashing in the direction of the mouse
                isDashing = true;
                dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Set the dash timer
                dashTimer = dashCooldown;
            }
        }
        //decrement the dash Timer
        dashTimer -= Time.deltaTime;

        // Check if the player pressed the fire button
        if (Input.GetMouseButtonDown(0))
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Get the direction of the mouse cursor
            Vector3 projectileDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            projectileDirection.z = 0;
            projectileDirection.Normalize();

            projectile.GetComponent<Rigidbody2D>().velocity = projectileDirection * projectileSpeed;
        }
    }
}