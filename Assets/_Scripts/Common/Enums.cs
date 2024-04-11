namespace Common
{
    public static class Enums
    {
        public enum SpellType
        {
            Single = 0,
            AOE = 1
        }
        
        public enum SpellProjectileType
        {
            Raycast = 0,
            Self = 1,
            OnTarget = 2,
            Projectile = 3
        }
    }
}