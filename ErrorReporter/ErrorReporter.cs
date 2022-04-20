#region

using System.Net;
using Confluent.Kafka;

#endregion

namespace ErrorReporter;

public class Reporter : IErrorReporter
{
    private readonly ProducerConfig Config;

    public Reporter(string bootstrapServers)
    {
        Config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            ClientId = Dns.GetHostName()
        };
    }

    public void ReportException(Exception exception)
    {
        var error = new Error.Error
        {
            Message = exception.Message,
            Code = (uint) exception
                .GetHashCode() // For now the code is the class hash code but in future releases it can be changed to anything (Even an string code)
        };
        BuildProducer<Null, string>().ProduceAsync("errors", new Message<Null, string>
        {
            Value = error.ToString() // Propagates error as protocol buffer encoded format to error recorder service
        });
    }

    private IProducer<T, TS> BuildProducer<T, TS>()
    {
        return new ProducerBuilder<T, TS>(Config).Build();
    }
}