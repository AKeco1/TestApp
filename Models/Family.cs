namespace TestProjectWthAngular.Models
{
    public class Family
    {
        public Family()
        {
            Adress = new Address();
            Phone = new Phone();
        }

        public string Name { get; set; }
        public string Born { get; set; }
        public Address Adress { get; set; }
        public Phone? Phone { get; set; }
        public bool Parent { get; set; } = true;

        public bool IsCompleted { get { return (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Born) && (Adress.IsCompleted || Phone.IsCompleted)); } }
    }
}