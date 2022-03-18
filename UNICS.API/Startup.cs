using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using UNICS.Data.Repository.GenericRepo;


namespace UNICS.API
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
            //
            services.AddControllers();

            // versioning
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1,0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                // show many versions else that app support
                config.ReportApiVersions = true;
                //
                config.ApiVersionReader = new HeaderApiVersionReader();
            });

            // DI - Dependency Injection
            // Add DbContext
            //services.AddDbContext<UNICSContext>();
            // Service
            //services.AddScoped<IAlbumService, AlbumService>();
            //services.AddScoped<IAlbumTypeService, AlbumTypeService>();
            //services.AddScoped<IAreaService, AreaService>();
            //services.AddScoped<IBlogService, BlogService>();
            //services.AddScoped<IBlogTypeService, BlogTypeService>();
            //services.AddScoped<ICampusService, CampusService>();
            //services.AddScoped<ICommentService, CommentService>();
            //services.AddScoped<ICompetitionService, CompetitionService>();
            //services.AddScoped<ICompetitionTypeService, CompetitionTypeService>();
            //services.AddScoped<IGroupUniversityService, GroupUniversityService>();
            //services.AddScoped<IImageService, ImageService>();
            //services.AddScoped<IMajorService, MajorService>();
            //services.AddScoped<IMajorInCompetitionService, MajorInCompetitionService>();
            //services.AddScoped<IMajorTypeService, MajorTypeService>();
            //services.AddScoped<IManagerInCompetitionService, ManagerInCompetitionService>();
            //services.AddScoped<IParticipantService, ParticipantService>();
            //services.AddScoped<IRatingService, RatingService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<ISeedsWalletService, SeedsWalletService>();
            //services.AddScoped<ITeamService, TeamService>();
            //services.AddScoped<IUniversityService, UniversityService>();
            //services.AddScoped<ITeamService, TeamService>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IVideoService, VideoService>();

            // Repository
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddTransient<IAlbumRepo, AlbumRepo>();
            //services.AddTransient<IAlbumTypeRepo, AlbumTypeRepo>();
            //services.AddTransient<IAreaRepo, AreaRepo>();
            //services.AddTransient<IBlogRepo, BlogRepo>();
            //services.AddTransient<IBlogTypeRepo, BlogTypeRepo>();
            //services.AddTransient<ICampusRepo, CampusRepo>();
            //services.AddTransient<ICommentRepo, CommentRepo>();
            //services.AddTransient<ICompetitionRepo, CompetitionRepo>();
            //services.AddTransient<ICompetitionTypeRepo, CompetitionTypeRepo>();
            //services.AddTransient<IGroupUniversityRepo, GroupUniversityRepo>();
            //services.AddTransient<IImageRepo, ImageRepo>();
            //services.AddTransient<IMajorInCompetitionRepo, MajorInCompetitionRepo>();
            //services.AddTransient<IMajorRepo, MajorRepo>();
            //services.AddTransient<IMajorTypeRepo, MajorTypeRepo>();
            //services.AddTransient<IManagerInCompetitionRepo, ManagerInCompetitionRepo>();
            //services.AddTransient<IParticipantRepo, ParticipantRepo>();
            //services.AddTransient<IRatingRepo, RatingRepo>();
            //services.AddTransient<IRoleRepo, RoleRepo>();
            //services.AddTransient<ISeedsWalletRepo, SeedsWalletRepo>();
            //services.AddTransient<ITeamRepo, TeamRepo>();            
            //services.AddTransient<IUniversityRepo, UniversityRepo>();
            //services.AddTransient<IUserRepo, UserRepo>();
            //services.AddTransient<IVideoRepo, VideoRepo>();

            //----------------------------------------------FIREBASE-------------------------
            string path = Path.Combine(Directory.GetCurrentDirectory(), "unics-e46a4-firebase-adminsdk-td0dr-86cc1f1def.json");
            //add firebase admin
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(path),
                ProjectId = "unics-e46a4",
                ServiceAccountId = "firebase-adminsdk-td0dr@unics-e46a4.iam.gserviceaccount.com"

            }) ;


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "UNICS.API",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UNICS.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
