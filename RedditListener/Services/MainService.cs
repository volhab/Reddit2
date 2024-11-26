using RedditListener.Interfaces;

namespace RedditListener.Services
{
    public class MainService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private IRedditService redditService;
        private IDataAccessService accessService;

        public MainService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {   
            string token;
            string after = string.Empty;

            using (var scope = _services.CreateScope())
            {
                redditService = scope.ServiceProvider.GetRequiredService<IRedditService>();
                accessService = scope.ServiceProvider.GetRequiredService<IDataAccessService>();

                token = redditService.GetToken().GetAwaiter().GetResult();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var postResponse = await redditService.ReadPosts(token, after);
                    if (postResponse != null)
                    {
                        Console.WriteLine($"rateLimitUsed: {postResponse.ratelimitUsed}, ratelimitRemaining: {postResponse.ratelimitRemaining}, rateLimitReset: {postResponse.ratelimitReset}");

                        if (postResponse.ratelimitRemaining == 0)
                        {
                            Thread.Sleep(60 * 1000);
                            token = redditService.GetToken().GetAwaiter().GetResult();
                        }

                        if (postResponse.data != null && postResponse.data.children.Count > 0)
                        {
                            var posts = postResponse.data.children.Select(p => p.data).ToList();

                            if (posts.Count > 0)
                            {
                                after = posts.Last().name;
                                await accessService.SavePosts(posts);
                                await accessService.SaveUsers(posts);
                                await accessService.UpdateSubreddit(posts.First());
                            }
                        }                      
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
                }
            }
        }
    }
}