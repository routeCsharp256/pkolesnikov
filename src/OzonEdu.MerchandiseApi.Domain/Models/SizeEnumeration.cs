namespace OzonEdu.MerchandiseApi.Domain.Models
{
    public class SizeEnumeration : Enumeration
    {
        public bool HasSize { get; }

        public SizeEnumeration(int id, string name, bool hasSize = false) : base(id, name)
        {
            HasSize = hasSize;
        }
    }
}