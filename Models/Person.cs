namespace TestProjectWthAngular.Models
{
    public class Person
    {
        public Person()
        {
            Family = new List<Family>();
            Adress = new Address();
            Phone = new Phone();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Adress { get; set; }
        public Phone? Phone { get; set; }
        public virtual ICollection<Family>? Family { get; set; }
        public bool Parent { get; set; } = true;

        public bool IsCompleted
        {
            get
            {
                return !String.IsNullOrEmpty(FirstName)
                    && !String.IsNullOrEmpty(LastName)
                    && Phone.IsCompleted;
            }
        }
    }
}
