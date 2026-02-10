using AuditService.Models;

namespace AuditService.DTOs
{
    public class AuditDtoResponse
    {
        public Guid AuditId { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public DateTime Created_At { get; set; }
        public AuditDtoResponse(Audit audit)
        {
            AuditId = audit.AuditId;
            EventType = audit.EventType;
            Payload = audit.Payload;
            Created_At = audit.Created_At;
        }
    }


}
