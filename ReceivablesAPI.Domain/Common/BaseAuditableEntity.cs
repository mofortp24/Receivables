using System.ComponentModel.DataAnnotations;

namespace ReceivablesAPI.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }

    [MaxLength(75)]
    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    [MaxLength(75)]
    public string? LastModifiedBy { get; set; }
}
