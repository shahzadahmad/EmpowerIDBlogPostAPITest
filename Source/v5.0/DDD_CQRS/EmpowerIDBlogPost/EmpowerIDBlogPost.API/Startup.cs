using AspNetCoreRateLimit;
using EmpowerIDBlogPost.Application.Caching;
using EmpowerIDBlogPost.Application.CommandHandlers;
using EmpowerIDBlogPost.Application.Commands;
using EmpowerIDBlogPost.Application.Dispatcher;
using EmpowerIDBlogPost.Application.DTOs;
using EmpowerIDBlogPost.Application.Queries;
using EmpowerIDBlogPost.Application.QueryHandlers;
using EmpowerIDBlogPost.Application.Services;
using EmpowerIDBlogPost.Domain.Entities;
using EmpowerIDBlogPost.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Collections.Generic;

namespace EmpowerIDBlogPost.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext and specify the connection string
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Configure Redis ConnectionMultiplexer and register it as a singleton
            var redisOptions = ConfigurationOptions.Parse(Configuration["Redis:ConnectionString"]);
            redisOptions.ClientName = Configuration["Redis:InstanceName"];
            var redisConnection = ConnectionMultiplexer.Connect(redisOptions);

            services.AddSingleton<IConnectionMultiplexer>(redisConnection);

            // Register RedisCacheService and other services that use it            
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IRepository<BlogPost>, Repository<BlogPost>>();

            // Configure rate limiting policies
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 100,          // Requests allowed per time span
                        Period = "1m"         // Time span (e.g., 1 minute)
                    }
                };
            });
            
            services.AddScoped<IDispatcher, Dispatcher>();                        
            services.AddScoped<IQueryHandler<GetBlogPostsQuery, IEnumerable<BlogPostDto>>, GetBlogPostsQueryHandler>();
            services.AddScoped<IQueryHandler<GetBlogPostQuery, BlogPostDto>, GetBlogPostQueryHandler>();
            services.AddScoped<ICommandHandler<CreateBlogPostCommand, BlogPostDto>, CreateBlogPostCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateBlogPostCommand, BlogPostDto>, UpdateBlogPostCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteBlogPostCommand, bool>, DeleteBlogPostCommandHandler>();


            // Add rate limiting middleware
            services.AddMemoryCache();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmpowerIDBlogPost.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmpowerIDBlogPost.API v1"));
            }

            // Use rate limiting middleware
            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
