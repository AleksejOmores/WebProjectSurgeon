using System;
using System.Collections.Generic;

namespace WebProject.Models;

public partial class Specialization
{
    public int SpecializationId { get; set; }

    public string SpecializationName { get; set; } = null!;

    public virtual ICollection<Surgeon> Surgeons { get; set; } = new List<Surgeon>();
}
