using System;

namespace backend.Dtos;

public class ImageRequest
{
    public Guid ProductId { get; set; }
    public List<string> ImageUrls { get; set; }

}
