public interface IStats
{
    public int Damage { get; }
    public float SawsVelocity { get; }
    public int Coins { get; }

    public void IncreaseDamage();
    public void AddSaws();
    public void IncreaseSawsVelocity();
    public void AddCoins();
}
