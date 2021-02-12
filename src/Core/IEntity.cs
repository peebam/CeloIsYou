
namespace CeloIsYou.Core
{
    public interface IEntity : IDrawable, IUpdatable
    {
        bool IsDone { get; }
    }
}
