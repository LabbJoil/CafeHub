
using CafeHub.Models.Domain;

namespace CafeHub.Models.ReportProcessing;

public class ComplaintSummary
{
    public Complaint? MainComplaint { get; set; }
    public User? UserComplain {  get; set; }
    public List<Attachment>? ListAttachmets { get; set; }
}
