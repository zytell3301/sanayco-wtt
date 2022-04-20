#region

#endregion

namespace GrpcService1.Domain.Entities;

public class OffTime
{
    public DateTime? CreatedAt { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public int Id { get; set; }

    /*
     * Every off time has three statuses. Approved,Rejected and Waiting. 
     */
    public string Status { get; set; }
    public DateTime ToDate { get; set; }
    public int UserId { get; set; }
}