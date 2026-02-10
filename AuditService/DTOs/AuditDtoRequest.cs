using AuditService.Models;

namespace AuditService.DTOs
{
    public class AuditDtoRequest
    {
        public string EventType { get; set; }
        public string Payload { get; set; }
        public AuditDtoRequest(string eventType, string payload)
        {
            EventType = eventType;
            Payload = payload;
        }
    }
}
