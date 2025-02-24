using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebProject.Models;

public partial class OperationSchedule
{
    public int OperationId { get; set; }

    [Required(ErrorMessage = "Не указан хирург")]
    public int? SurgeonId { get; set; }

    [Required(ErrorMessage = "Не указано время начала")]
    public DateTime StartDateTime { get; set; }

    [Required(ErrorMessage = "Не указано время окончания")]
    public DateTime EndDateTime { get; set; }

    [Required(ErrorMessage = "Не указана операционная")]
    public int? OperatingRoomId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual OperatingRoom? OperatingRoom { get; set; }
    public virtual Surgeon? Surgeon { get; set; }
}

