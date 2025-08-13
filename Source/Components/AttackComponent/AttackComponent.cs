using Godot;
public struct Attack
{
    public float Damage;
    public Vector3 GlobalPosition;
    public float SpeedForce;

    public Attack(float damage, Vector3 globalPosition, float speedForce)
    {
        Damage = damage;
        GlobalPosition = globalPosition;
        SpeedForce = speedForce;
    }
}
