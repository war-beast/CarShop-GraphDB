using CarShop.Data;
using CarShop.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neo4jClient;
using System;
using Elasticsearch.Net;
using Nest;
using GraphConnection = CarShop.BLL.Models.GraphConnection;

namespace CarShop
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
			services.Configure<GraphConnection>(Configuration.GetSection("GraphConnection"));

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			var neo4JClient = new GraphClient(new Uri($"{Configuration.GetSection("GraphConnection:Host").Value}:{Configuration.GetSection("GraphConnection:Port").Value}"), Configuration.GetSection("GraphConnection:User").Value, Configuration.GetSection("GraphConnection:Password").Value);
			neo4JClient.ConnectAsync().Wait();
			services.AddSingleton<IGraphClient>(neo4JClient);

			var pool = new SingleNodeConnectionPool(new Uri(Configuration.GetSection("ElasticSearch:BaseUrl").Value));
			var settings = new ConnectionSettings(pool).DefaultIndex("carshop");
			var client = new ElasticClient(settings);
			services.AddSingleton(client);

			services.AddServices(lifetime: ServiceLifetime.Transient);

			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
