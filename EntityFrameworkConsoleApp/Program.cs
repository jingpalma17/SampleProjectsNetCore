using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkConsoleApp.Models;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace EntityFrameworkConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            Run().Wait();
            Console.ReadLine();
            Console.WriteLine("finished");
        }

        static async Task Run()
        {
            var client = new HttpClient();
            var discoveryDoc = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

            Console.WriteLine("Test GET /Identity");
            var user1TokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryDoc.TokenEndpoint,

                ClientId = "client2",
                ClientSecret = "secret",
                Scope = string.Join(' ',
                    "article.read",
                    "article.write",
                    "identity.read"
                ),
                UserName = "user1",
                Password = "Pass123$"
            });
            var user1ApiClient = new HttpClient();
            user1ApiClient.SetBearerToken(user1TokenResponse.AccessToken);
            var identityResponse = await user1ApiClient.GetAsync("https://localhost:52402/Identity");
            var identityContent = await identityResponse.Content.ReadAsStringAsync();
            Console.WriteLine(identityResponse.StatusCode);
            Console.WriteLine(identityContent);
            Console.ReadLine();

            Console.WriteLine("Test POST /Article");
            var admin1TokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryDoc.TokenEndpoint,

                ClientId = "client2",
                ClientSecret = "secret",
                Scope = string.Join(' ',
                    "article.read",
                    "article.write",
                    "identity.read"
                ),
                UserName = "admin1",
                Password = "Pass123$"
            });
            var admin1ApiClient = new HttpClient();
            var newArticle = new Article
            {
                Title = "Title Sample",
                Description = "Descriptions sample ...",
                Category = "Internal",
            };
            var content = new StringContent(JsonConvert.SerializeObject(newArticle), Encoding.UTF8, "application/json");
            admin1ApiClient.SetBearerToken(admin1TokenResponse.AccessToken);
            var articleResponse = await admin1ApiClient.PostAsync("https://localhost:52402/Articles", content);
            Console.WriteLine(articleResponse.StatusCode);
            var articleContentJson = await articleResponse.Content.ReadAsStringAsync();
            newArticle = JsonConvert.DeserializeObject<Article>(articleContentJson);
            Console.WriteLine($"Article Id:{newArticle.Id}");
            Console.WriteLine($"Article Title:{newArticle.Title}");
            Console.WriteLine($"Article Description:{newArticle.Description}");
            Console.WriteLine($"Article Category:{newArticle.Category}");
            Console.ReadLine();


            using (var context = new EntityFrameworkConsoleAppContext())
            {
                // Test create article
                newArticle = new Article
                {
                    Title = "Title Sample",
                    Description = "Descriptions sample ...",
                    Category = "Internal",
                };
                context.Add(newArticle);
                await context.SaveChangesAsync();
                Console.WriteLine("Test create article");
                Console.WriteLine($"Article Id:{newArticle.Id}");
                Console.WriteLine($"Article Title:{newArticle.Title}");
                Console.WriteLine($"Article Description:{newArticle.Description}");
                Console.WriteLine($"Article Category:{newArticle.Category}");
                Console.ReadLine();

                // Test get article by id
                newArticle = await context.Articles.FindAsync(newArticle.Id);
                Console.WriteLine("Test get article by id");
                Console.WriteLine($"Article Id:{newArticle.Id}");
                Console.WriteLine($"Article Title:{newArticle.Title}");
                Console.WriteLine($"Article Description:{newArticle.Description}");
                Console.WriteLine($"Article Category:{newArticle.Category}");
                Console.ReadLine();

                // Test update article
                newArticle.Title = "Title Sample Updated";
                newArticle.Description = "Descriptions sample updated ...";
                await context.SaveChangesAsync();
                Console.WriteLine("Test update article");
                Console.WriteLine($"Article Id:{newArticle.Id}");
                Console.WriteLine($"Article Title:{newArticle.Title}");
                Console.WriteLine($"Article Description:{newArticle.Description}");
                Console.WriteLine($"Article Category:{newArticle.Category}");
                Console.ReadLine();

                // Test get all articles
                var articles = context.Articles;
                Console.WriteLine("Test get all articles");
                foreach (var data in articles)
                {
                    Console.WriteLine($"{data.Id}. {data.Title}");
                }
                Console.ReadLine();
            }
        }
    }
}
