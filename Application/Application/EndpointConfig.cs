namespace Application
{
    using FluentValidation;
    using Library.Authorization;
    using Library.Validation;
    using log4net;
    using NServiceBus;
    using Raven.Client;
    using Raven.Client.Document;
    using System;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization, ISpecifyMessageHandlerOrdering
    {
        public void Init()
        {
            Configure.Transactions.Advanced(t => t.DefaultTimeout(new TimeSpan(0, 5, 0)));
            Configure.Serialization.Json();
            Configure
                .With(AllAssemblies.Except("ServiceStack"))
                .StructureMapBuilder()
                .Log4Net()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
                .DefiningCommandsAs(t => t.Namespace != null && (t.Namespace.EndsWith("Commands") || t.Namespace.EndsWith("Queries")))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"))
                .UnicastBus()
                .RavenPersistence()
                .RavenSubscriptionStorage()
                .UseInMemoryTimeoutPersister()
                .InMemoryFaultManagement()
                .InMemorySagaPersister();
            LogManager.GetRepository().Threshold = log4net.Core.Level.Warn;

            var store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "Demo-ReadModels" };
            store.Initialize();

            Configure.Instance.Configurer.RegisterSingleton<IDocumentStore>(store);
            Configure.Instance.Configurer.RegisterSingleton<IValidatorFactory>(new StructureMapValidatorFactory());
        }
        public void SpecifyOrder(Order order)
        {
            order.Specify(First<ValidationMessageHandler>.Then<AuthorizationMessageHandler>());
        }
    }
}