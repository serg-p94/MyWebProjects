using MvcCalculator.Helper;

namespace MvcCalculator.Models
{
    public class ActorAdding
    {
        public Actor Actor { get; set; }
        public ActorAddingResult Result { get; set; }

        public ActorAdding()
        {
            Actor = new Actor();
            Result = ActorAddingResult.No;
        }
    }
}