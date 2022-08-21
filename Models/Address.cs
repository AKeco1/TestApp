namespace TestProjectWthAngular.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public bool Parent { get; set; } = true;
        public bool IsCompleted { get { return !String.IsNullOrEmpty(Street) && !String.IsNullOrEmpty(City); } }
    }
}