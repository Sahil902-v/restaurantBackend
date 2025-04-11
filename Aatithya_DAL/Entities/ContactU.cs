using System;
using System.Collections.Generic;

namespace Aatithya_DAL.Entities;

public partial class ContactU
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public string Message { get; set; } = null!;
}
