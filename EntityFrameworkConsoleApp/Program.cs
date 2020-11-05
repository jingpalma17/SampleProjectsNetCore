using System;
using System.Net.Http;
using System.Threading.Tasks;
using EntityFrameworkConsoleApp.Models;
using IdentityModel.Client;

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
            Console.WriteLine("Test identity endpoint");
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client2",
                ClientSecret = "secret",
                Scope = string.Join(' ',
                    "article.read",
                    "article.write",
                    "identity.read"
                ),
                UserName = "bob",
                Password = "Pass123$"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:52402/Identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(content);
            }

            Console.ReadLine();


            using (var context = new EntityFrameworkConsoleAppContext())
            {
                // Test create article
                var newArticle = new Article
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
