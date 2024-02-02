namespace CourseProject_SellingTickets.Models;

public class Discount
{

    public System.Int64 Id { get; init; }
    public string Name { get; set; }
    public int DiscountSize { get; set; }
    public string Description { get; set; }

    public Discount(System.Int64 id, string name, int discountSize, string description)
    {
        Id = id;
        Name = name;
        DiscountSize = discountSize;
        Description = description;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is Discount o)
        {
            return Id.Equals(o.Id) && 
                   Name.Equals(o.Name) &&
                   DiscountSize.Equals(o.DiscountSize) && 
                   Description.Equals(o.Description);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
}