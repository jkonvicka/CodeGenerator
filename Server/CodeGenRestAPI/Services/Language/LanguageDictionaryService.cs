using CodeGenEngine;
using CodeGenRestAPI.WrapperModel;
using Newtonsoft.Json;

namespace CodeGenRestAPI.Services.Language
{
    public class LanguageDictionaryService : ILanguageDictionaryService
    {
        private readonly Dictionary<string, LanguageDeclaration> _languageDictionary = new();

        public LanguageDictionaryService()
        {
            string rootPath = Directory.GetCurrentDirectory();
            var languagesDirectory = Path.Combine(rootPath, "Configuration/Languages/");
            string[] fileEntries = Directory.GetFiles(languagesDirectory, "*.json");

            foreach (string filePath in fileEntries)
            {
                string json = File.ReadAllText(filePath);
                LanguageDeclaration? obj = JsonConvert.DeserializeObject<LanguageDeclaration>(json);
                if (obj != null)
                {
                    _languageDictionary.Add(Path.GetFileNameWithoutExtension(filePath), obj);
                }
            }
        }

        public string[] GetAllKeys()
        {
            return _languageDictionary.Keys.ToArray();
        }

        public LanguageDeclaration GetLanguage(string key)
        {
            if (!_languageDictionary.ContainsKey(key))
            {
                throw new NotSupportedException($"Language '{key}' is not supported.");
            }
            return _languageDictionary[key];
        }
    }

}
