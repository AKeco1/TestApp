using System.Xml.Linq;
using TestProjectWthAngular.Models;

namespace TestProjectWthAngular.Services
{
    public interface IXmlConverter
    {
        public Task<List<Person>> CollectAllData(string[]? lines);
        public Task ParseAndSaveAsXml(List<Person> persons, XElement xml, string fileName);
        public string[] SeparatedLines(string line);
        public Task Convert(string filePath, string fileName);
    }
}
