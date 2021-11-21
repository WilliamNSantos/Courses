namespace MyVoby.Domain.Core.Events;

public class Event 
{
    public DateTime Timestamp { get; protected set; }

    public Event()
    {
        Timestamp = DateTime.Now;
    }
}