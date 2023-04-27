using CodeGenEngine;

namespace CodeGenRestAPI.Services.Language
{
    public interface ILanguageDictionaryService
    {
        LanguageDeclaration GetLanguage(string key);
        string[] GetAllKeys();
    }
}
