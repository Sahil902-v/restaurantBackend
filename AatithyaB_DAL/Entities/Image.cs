using System;
using System.Collections.Generic;

namespace AatithyaB_DAL.Entities;

public partial class Image
{
    public int Id { get; set; }

    public string ImgUrl { get; set; } = null!;

    public int ImgCategory { get; set; }

    public string ImgTitle { get; set; } = null!;
}
