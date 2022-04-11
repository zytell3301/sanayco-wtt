using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace GrpcService1.Domain.Entities;

[Table("off_time", Schema = "wtt")]
public class OffTime
{
    [Column("id")] public int Id;
    [Column("user_id")] public int UserId;

    /*
     * Every off time has three statuses. Approved,Rejected and Waiting. 
     */
    [Column("status")] public string Status;
    [Column("from_date")] public DateTime FromDate;
    [Column("to_date")] public DateTime ToDate;
    [Column("Description")] public string Description;
    [Column("created_at")] public DateTime? CreatedAt;
}