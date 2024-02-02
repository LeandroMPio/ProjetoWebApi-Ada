namespace Domain.Requests;

public class BaseProductRequest
{
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public double? Price { get; set; }
}

public class UpdateProductRequest : BaseProductRequest
{
    public int id { get; set; }
}
