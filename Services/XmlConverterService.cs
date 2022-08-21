using System.Xml.Linq;
using TestProjectWthAngular.Models;

namespace TestProjectWthAngular.Services
{
    public class XmlConverterService : IXmlConverter
    {
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly IConfiguration _configuration;

        public XmlConverterService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Person>> CollectAllData(string[]? lines)
        {
            try
            {
                var persons = new List<Person>();

                var person = new Person();

                int startingIndex = int.Parse(GetStringValueForKey("StartingIndex"));

                for (int i = 0; i < lines?.Length; i++)
                {
                    string[] array = SeparatedLines(lines[i]);
                    string nextLine = "";

                    if (i < lines.Length - 1)
                        nextLine = SeparatedLines(lines[i + 1])[0];

                    if (array[startingIndex] == GetStringValueForKey("PersonTerminationSign"))
                    {
                        person = new Person();
                        person.FirstName = array[startingIndex + 1];
                        person.LastName = array[startingIndex + 2];
                    }

                    if (array[startingIndex] == GetStringValueForKey("TelephoneTerminationSign") && person.Family?.Count == 0)
                    {
                        Phone phone = new Phone();
                        phone.Mobile = array[startingIndex + 1];
                        phone.FixPhone = array[startingIndex + 2];
                        person.Phone = phone;
                    }

                    if (array[startingIndex] == GetStringValueForKey("TelephoneTerminationSign") && person.Family?.Count > 0)
                    {
                        Phone phone = new Phone();
                        phone.Mobile = array[startingIndex + 1];
                        phone.FixPhone = array[startingIndex + 2];
                        person.Family.LastOrDefault().Phone = phone;

                    }

                    if (array[startingIndex] == GetStringValueForKey("AddressTerminationSign") && person.Family?.Count == 0)
                    {
                        person.Adress.Street = array[startingIndex + 1];
                        person.Adress.City = array[startingIndex + 2];

                        if (array.Length > 2)
                            person.Adress.PostCode = array[startingIndex + 3];
                    }
                    else if (array[startingIndex] == GetStringValueForKey("AddressTerminationSign") && person.Family?.Count > 0)
                    {
                        Address address = new Address();
                        address.Street = array[startingIndex + 1];
                        address.City = array[startingIndex + 2];

                        if (array.Length > 2)
                            address.PostCode = array[startingIndex + 3];

                        person.Family.LastOrDefault().Adress = address;
                    }

                    if (array[startingIndex] == GetStringValueForKey("FamilyTerminationSign"))
                    {
                        Family fam = new Family();
                        fam.Name = array[startingIndex + 1];
                        fam.Born = array[startingIndex + 2];
                        person.Family?.Add(fam);
                    }

                    if (person.IsCompleted && nextLine.Equals(GetStringValueForKey("PersonTerminationSign")) || i == lines.Length - 1)
                        persons.Add(person);
                }
                return persons;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
             

        public async Task Convert(string filePath, string fileName)
        {
            string[] lines = File.ReadAllLines(filePath);
                        
            List<Person> persons = await CollectAllData(lines);

            XElement xml = new XElement(GetStringValueForKey("PeopleTag"));

            await ParseAndSaveAsXml(persons, xml, fileName);
        }

        public async Task ParseAndSaveAsXml(List<Person> persons, XElement xml, string fileName)
        {
            try
            {
                for (int i = 0; i < persons.Count; i++)
                {
                    xml.Add(new XElement(GetStringValueForKey("PersonTag"),
                        new XElement(GetStringValueForKey("FirstNameTag"), persons[i].FirstName),
                        new XElement(GetStringValueForKey("LastNameTag"), persons[i].LastName),
                        new XElement(GetStringValueForKey("AddressTag"), new XElement(GetStringValueForKey("StreetTag"), persons[i].Adress.Street),
                                                new XElement(GetStringValueForKey("CityTag"), persons[i].Adress.City),
                                                new XElement(GetStringValueForKey("ZipCodeTag"), persons[i].Adress.PostCode != null ? persons[i].Adress.PostCode : "")),
                        new XElement(GetStringValueForKey("PhoneTag"), new XElement(GetStringValueForKey("MobilePhoneTag"), persons[i].Phone?.Mobile != null ? persons[i].Phone?.Mobile : ""),
                                                new XElement(GetStringValueForKey("LandLinePhoneTag"), persons[i]?.Phone?.FixPhone != null ? persons[i].Phone?.FixPhone : "")),
                        from f in persons[i].Family
                        select new XElement(GetStringValueForKey("FamilyTag"), new XElement(GetStringValueForKey("NameTag"), f.Name),
                                                new XElement(GetStringValueForKey("BornTag"), f.Born),
                                                new XElement(GetStringValueForKey("AddressTag"),
                                                            new XElement(GetStringValueForKey("StreetTag"), f.Adress.Street != null ? f.Adress.Street : ""),
                                                            new XElement(GetStringValueForKey("CityTag"), f.Adress.City != null ? f.Adress.City : ""),
                                                            new XElement(GetStringValueForKey("ZipCodeTag"), f.Adress.PostCode != null ? f.Adress.PostCode : "")),
                                                new XElement(GetStringValueForKey("PhoneTag"),
                                                             new XElement(GetStringValueForKey("MobilePhoneTag"), f.Phone?.Mobile != null ? f.Phone.Mobile : ""),
                                                             new XElement(GetStringValueForKey("LandLinePhoneTag"), f.Phone?.FixPhone != null ? f.Phone.FixPhone : "")))));

                }
                xml.Save($"{filePath}\\Converted_{fileName}{GetStringValueForKey("FileSaveExtension")}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
                 }

        public string[] SeparatedLines(string line)
        {
           return line.Split(GetStringValueForKey("SeparatorSign")).ToArray();
        }

        private string GetStringValueForKey(string key)
        {
            return _configuration.GetSection(key).Value;
        }
    }
}
