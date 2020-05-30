using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.Translate;
using Newtonsoft.Json;

namespace Blazor.I18nText.Generate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Generate translation files using AWS Translate from Blazor I18nText JSON");

            Console.Write("Enter File Path to English JSON: ");
            var file = Console.ReadLine();
            Console.Write("Enter list of languages to translate (comma separated): ");
            var languages = Console.ReadLine().Split(',');

            var dir = Path.GetDirectoryName(file);
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(file));
            using (var trans = new AmazonTranslateClient())
            {
                foreach (var lang in languages)
                {
                    var newData = new Dictionary<string, string>();
                    foreach (var item in data)
                    {
                        var translation = await trans.TranslateTextAsync(new Amazon.Translate.Model.TranslateTextRequest()
                        {
                            SourceLanguageCode = "en",
                            TargetLanguageCode = lang,
                            Text = item.Value
                        });
                        if (translation.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            newData.Add(item.Key, translation.TranslatedText);
                        }
                        else
                            newData.Add(item.Key, "NO TRANSLATION");
                        //TODO: Error
                    }
                    var fn = Path.GetFileName(file).Split('.');
                    var newFn = $"{fn[0]}.{lang}.{fn[2]}";
                    var newFnPath = Path.Combine(dir, newFn);
                    Console.WriteLine(newFnPath);
                    if (!File.Exists(newFnPath))
                        File.Create(newFnPath).Dispose();
                    using (var fs = new FileStream(newFnPath, FileMode.Truncate))
                    {
                        var newJson = JsonConvert.SerializeObject(newData, new JsonSerializerSettings()
                        {
                            Formatting = Formatting.Indented,
                        });
                        using (var sr = new StreamWriter(fs, System.Text.Encoding.UTF8))
                            await sr.WriteAsync(newJson);
                    }
                }
            }
        }
    }
}
