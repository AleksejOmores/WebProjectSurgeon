using System;
using System.Collections.Generic;

namespace WebProject.Models;

public partial class OperationSchedule
{
    public int OperationId { get; set; }

    public int? SurgeonId { get; set; }

    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    public int? OperatingRoomId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual OperatingRoom? OperatingRoom { get; set; }

    public virtual Surgeon? Surgeon { get; set; }
}
