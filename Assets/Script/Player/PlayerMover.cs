using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    Rigidbody rb;
    PlayerData data;

    public void Setup(PlayerData data, Rigidbody rb)
    {
        this.data = data;
        this.rb = rb;
    }

    public void Move(float h, float v, bool shift, bool grounded, bool jump)
    {
        Vector3 vel = data.CalculateVelocity(h, v, shift, grounded, Time.fixedDeltaTime);

        float y = rb.linearVelocity.y;

        if (jump)
        {
            rb.AddForce(Vector3.up * data.JumpPower, ForceMode.Impulse);
        }

        rb.linearVelocity = new Vector3(vel.x, y, vel.z);

        Vector3 input = new Vector3(h, 0, v);
        if (input != Vector3.zero && !data.IsAttacking)
        {
            rb.MoveRotation(data.CalculateRotation(rb.rotation, input, Time.fixedDeltaTime));
        }
    }
}