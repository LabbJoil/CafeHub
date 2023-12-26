
using CHDesktop.Models.Domain;

namespace CHDesktop.Models.ReportProcessing;

public class ComplaintSummary
{
    public Complaint? MainComplaint { get; set; }
    public User? UserComplain {  get; set; }
    public List<Attachment>? ListAttachmets { get; set; }
    public bool IsSelected {  get; set; }
}
