using System.Net;
using Confluent.Kafka;

namespace ErrorReporter;

public class Reporter : IErrorReporter
{
    private ProducerConfig Config;

    public Reporter(string bootstrapServers)
    {
        Config = new ProducerConfig()
        {
            BootstrapServers = bootstrapServers,
            ClientId = Dns.GetHostName(),
        };
    }

    private IProducer<T, TS> BuildProducer<T, TS>()
    {
        return new ProducerBuilder<T, TS>(Config).Build();
    }

    public void ReportException(Exception exception)
    {
        Error.Error error = new Error.Error()
        {
            Message = exception.Message,
            Code = (uint) exception
                .GetHashCode(), // For now the code is the class hash code but in future releases it can be changed to anything (Even an string code)
        };
        BuildProducer<Null, string>().ProduceAsync("errors", new Message<Null, string>()
        {
            Value = error.ToString(), // Propagates error as protocol buffer encoded format to error recorder service
        });
    }
}