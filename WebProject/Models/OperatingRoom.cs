using System;
using System.Collections.Generic;

namespace WebProject.Models;

public partial class OperatingRoom
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = null!;
    public int? Capacity { get; set; }
    public string? EquipmentDetails { get; set; } 

    public virtual ICollection<OperationSchedule> OperationSchedules { get; set; } = new List<OperationSchedule>();
}
