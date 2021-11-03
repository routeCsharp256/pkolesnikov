using MediatR;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class NotificationAboutEventDomainEvent : INotification
    {
        public string EventName { get; }

        public NotificationAboutEventDomainEvent(string eventName)
        {
            EventName = eventName;
        }
    }
}