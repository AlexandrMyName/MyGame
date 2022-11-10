


public class Character 
{
    private float health = 100f;
    private float maxHealth;
    private string name;
    


    protected float Health { get { return health; } set { health = value;  health = health > maxHealth ? maxHealth : health; } }

    protected string Name { get { return name; }}

    protected Character (string name, float health, float maxHealth) { this.name = name; this.health = health; this.maxHealth = maxHealth; this.health = maxHealth; }
}
