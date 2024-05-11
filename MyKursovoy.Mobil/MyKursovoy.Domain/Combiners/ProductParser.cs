namespace MyKursovoy.Domain.Combiners
{
    public class ProductParser
    {
        public int? Id_Order { get; set; }

        public IEnumerable<ProductCombiner> Products { get; set; } = [];
    }
}

