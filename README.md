# Blazor.I18nText.Generate

Using Amazon Translate, take JSON file in english and create ones for other languages.

Designed to be used with https://github.com/jsakamoto/Toolbelt.Blazor.I18nText

### Example
```bash
dotnet run
Generate translation files using AWS Translate from Blazor I18nText JSON
Enter File Path to English JSON: /Users/nwestfall/Projects/Project/i18ntext/ProjectStrings.en.json
Enter list of languages to translate (comma separated): fr-CA,es
/Users/nwestfall/Projects/Project/i18ntext/ProjectStrings.fr-CA.json
/Users/nwestfall/Projects/Project/i18ntext/ProjectStrings.es.json