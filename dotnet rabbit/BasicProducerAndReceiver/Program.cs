using BasicProducerAndReceiver;

Console.WriteLine("Starting...");
var message = string.Empty;

var queueCommunication = new QueueCommunication();
queueCommunication.Produce(message);
queueCommunication.Produce(message);
queueCommunication.Produce(message);
queueCommunication.Consume();

Console.WriteLine("Press enter to quit");
Console.ReadKey();
