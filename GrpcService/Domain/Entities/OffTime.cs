namespace GrpcService1.Domain.Entities;

public class OffTime
{
    public int Id;
    public int UserId;

    /*
     * Every off time has three statuses. Approved,Rejected and Waiting. 
     */
    public string Status;
    public DateTime Date;
    public DateTime from;
    public DateTime to;
    public string Description;
}