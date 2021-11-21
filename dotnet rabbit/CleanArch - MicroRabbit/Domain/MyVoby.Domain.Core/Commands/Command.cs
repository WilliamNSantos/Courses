using MyVoby.Domain.Core.Events;

namespace MyVoby.Domain.Core.Commands;

public abstract class Command : Message
{
    public DateTime Timestamp { get; protected set; }

    protected Command(){
        Timestamp = DateTime.Now;
    }
}

