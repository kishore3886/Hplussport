namespace Hplussport.API.Classes
{
    public class ProductQueryParameters : QueryParameters
    {

        public string? sku { get; set; }
        public decimal? minprce { get; set; }
        public decimal? maxprice { get; set; }

        public string Name { get; set; }
    }
}
