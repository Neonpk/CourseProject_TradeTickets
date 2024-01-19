namespace CourseProject_SellingTickets.Models;

public class Discount
{

    public System.Int64 Id { get; }
    public string Name { get; }
    public int DiscountSize { get; }
    public string Description { get; }

    public Discount(System.Int64 id, string name, int discountSize, string description)
    {
        Id = id;
        Name = name;
        DiscountSize = discountSize;
        Description = description;
    }
    
}