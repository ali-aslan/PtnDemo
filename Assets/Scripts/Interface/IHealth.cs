public interface IHealth
{
    //public Building specs { get; set; }

    public int Health { get; set; }

    public void TakeDamage(int damage);

    public void ToDisappear();

}
