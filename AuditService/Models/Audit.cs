namespace AuditService.Models
{
    public class Audit
    {
        public Guid AuditId { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public DateTime Created_At { get; set; }

    }
}
