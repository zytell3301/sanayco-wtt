#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace GrpcService1.Domain.Entities;

[Table("off_time", Schema = "wtt")]
public class OffTime
{
    [Column("created_at")] public DateTime? CreatedAt;
    [Column("Description")] public string Description;
    [Column("from_date")] public DateTime FromDate;
    [Column("id")] public int Id;

    /*
     * Every off time has three statuses. Approved,Rejected and Waiting. 
     */
    [Column("status")] public string Status;
    [Column("to_date")] public DateTime ToDate;
    [Column("user_id")] public int UserId;
}