namespace TestProjectWthAngular.Models
{
    public class Phone
    {
        public string Mobile { get; set; }
        public string FixPhone { get; set; }
        public bool Parent { get; set; } = true;
        public bool IsCompleted { get { return !String.IsNullOrEmpty(Mobile) && !String.IsNullOrEmpty(FixPhone); } }
    }
}