namespace Modules.Roots.Scripts
{
    public interface IRootSegment
    {
        bool IsDied { get; }
        void Die(IRootSegment from);
    }
}