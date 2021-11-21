using MyVoby.Domain.Core.Bus;
using MyVoby.Domain.Core.Commands;
using MyVoby.Domain.Core.Events;
using MediatR;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using System.Text;

namespace MyVoby.Infra.Bus;

public class RabbitMQBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes;

    public RabbitMQBus(IMediator mediator)
    {
        _mediator = mediator;
        _handlers = new Dictionary<string, List<Type>>();
        _eventTypes = new List<Type>();
    }

    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public void Publish<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using ( var connection = factory.CreateConnection())
        using ( var channel = connection.CreateModel())
        {
            var eventName = @event.GetType().Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var message = JsonSerializer.SerializeToUtf8Bytes(@event);

            channel.BasicPublish("", eventName, null, message);
        }
    }

    public void Subscribe<T, TH>()
        where T : Event
        where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T))) 
            _eventTypes.Add(typeof(T)); 

        if (!_handlers.ContainsKey(eventName))
            _handlers.Add(eventName, new List<Type>());

        if (_handlers[eventName].Any(h => h.GetType() == handlerType))
            throw new ArgumentException(
                $"Handler type {handlerType.Name} already ir registred for '{eventName}'", nameof(handlerType));

        _handlers[eventName].Add(handlerType);

        StartBasicConsume<T>();
    }

    private void StartBasicConsume<T>() where T : Event
    {
        var factory = new ConnectionFactory{
            HostName = "localhost",
            DispatchConsumersAsync = true
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        var eventName = typeof(T).Name;

        var QueueDeclare = channel.QueueDeclare(eventName, false, false, false, null);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += Consumer_Received;

        channel.BasicConsume(eventName, true, consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch(Exception ex)
        {

        }
    }

    private async Task ProcessEvent(string eventName, string message){
        if(_handlers.ContainsKey(eventName))
        {
            var subscriptions = _handlers[eventName];
            foreach(var subscription in subscriptions)
            {
                var handler = Activator.CreateInstance(subscription);
                if (handler is null) continue;
                var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                var @event = JsonSerializer.Deserialize(message, eventType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
            }
        }
    }
}