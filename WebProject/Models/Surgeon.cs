using System;
using System.Collections.Generic;

namespace WebProject.Models;

public partial class Surgeon
{
    public int SurgeonId { get; set; }

    public int? UserId { get; set; }

    public string FullName { get; set; } = null!;

    public int? SpecializationId { get; set; }

    public string? ContactInfo { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OperationSchedule> OperationSchedules { get; set; } = new List<OperationSchedule>();

    public virtual Specialization? Specialization { get; set; }

    public virtual User? User { get; set; }
}
