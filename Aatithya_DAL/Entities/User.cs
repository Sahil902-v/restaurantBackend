using System;
using System.Collections.Generic;

namespace Aatithya_DAL.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int PermissionVersion { get; set; }
}
