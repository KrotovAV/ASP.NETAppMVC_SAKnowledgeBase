namespace SAKnowledgeBase.Services
{
    public interface ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file);
        void WriteCSV<T>(List<T> records);
    }
}
