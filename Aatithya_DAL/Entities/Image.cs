using System;
using System.Collections.Generic;

namespace Aatithya_DAL.Entities;

public partial class Image
{
    public int Id { get; set; }

    public string ImgUrl { get; set; } = null!;

    public int ImgCategory { get; set; }

    public string ImgTitle { get; set; } = null!;

    public bool IsMainDisp { get; set; }

    public string? ImgName { get; set; }

    public bool IsMenu { get; set; }

    public bool IsGallery { get; set; }
}
