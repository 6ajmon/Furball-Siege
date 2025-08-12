using Godot;
public struct Attack
{
    public float Damage;
    public Vector3 Direction;
    public Node3D Source;

    public Attack(float damage, Vector3 direction, Node3D source)
    {
        Damage = damage;
        Direction = direction;
        Source = source;
    }
}
