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

using UNICS.Business.Services.AlbumSvc;
using UNICS.Business.Services.AlbumTypeSvc;
using UNICS.Business.Services.AreaSvc;
using UNICS.Business.Services.BlogSvc;
using UNICS.Business.Services.BlogTypeSvc;
using UNICS.Business.Services.CampusSvc;
using UNICS.Business.Services.CommentSvc;
using UNICS.Business.Services.CompetitionSvc;
using UNICS.Business.Services.CompetitionTypeSvc;
using UNICS.Business.Services.GroupUniversitySvc;
using UNICS.Business.Services.ImageSvc;
using UNICS.Business.Services.MajorInCompetitionSvc;
using UNICS.Business.Services.MajorSvc;
using UNICS.Business.Services.MajorTypeSvc;
using UNICS.Business.Services.ManagerInCompetitionSvc;
using UNICS.Business.Services.ParticipantSvc;
using UNICS.Business.Services.RatingSvc;
using UNICS.Business.Services.RoleSvc;
using UNICS.Business.Services.SeedsWalletSvc;
using UNICS.Business.Services.TeamSvc;

using UNICS.Business.Services.UniversitySvc;
using UNICS.Business.Services.UserSvc;
using UNICS.Business.Services.VideoSvc;
using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;
using UNICS.Data.Repository.ImplRepo.AlbumRepo;
using UNICS.Data.Repository.ImplRepo.AlbumTypeRepo;
using UNICS.Data.Repository.ImplRepo.AreaRepo;
using UNICS.Data.Repository.ImplRepo.BlogRepo;
using UNICS.Data.Repository.ImplRepo.BlogTypeRepo;
using UNICS.Data.Repository.ImplRepo.CampusRepo;
using UNICS.Data.Repository.ImplRepo.CommentRepo;
using UNICS.Data.Repository.ImplRepo.CompetitionRepo;
using UNICS.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UNICS.Data.Repository.ImplRepo.GroupUniversityRepo;
using UNICS.Data.Repository.ImplRepo.ImageRepo;
using UNICS.Data.Repository.ImplRepo.MajorInCompetitionRepo;
using UNICS.Data.Repository.ImplRepo.MajorRepo;
using UNICS.Data.Repository.ImplRepo.MajorTypeRepo;
using UNICS.Data.Repository.ImplRepo.ManagerInCompetitionRepo;
using UNICS.Data.Repository.ImplRepo.ParticipantRepo;
using UNICS.Data.Repository.ImplRepo.RatingRepo;
using UNICS.Data.Repository.ImplRepo.RoleRepo;
using UNICS.Data.Repository.ImplRepo.SeedsWalletRepo;
using UNICS.Data.Repository.ImplRepo.TeamRepo;
using UNICS.Data.Repository.ImplRepo.UniversityRepo;
using UNICS.Data.Repository.ImplRepo.UserRepo;
using UNICS.Data.Repository.ImplRepo.VideoRepo;

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
            services.AddDbContext<UNICSContext>();
            // Service
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IAlbumTypeService, AlbumTypeService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IBlogTypeService, BlogTypeService>();
            services.AddScoped<ICampusService, CampusService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICompetitionService, CompetitionService>();
            services.AddScoped<ICompetitionTypeService, CompetitionTypeService>();
            services.AddScoped<IGroupUniversityService, GroupUniversityService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<IMajorInCompetitionService, MajorInCompetitionService>();
            services.AddScoped<IMajorTypeService, MajorTypeService>();
            services.AddScoped<IManagerInCompetitionService, ManagerInCompetitionService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISeedsWalletService, SeedsWalletService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVideoService, VideoService>();

            // Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IEntityTypeRepo, EntityTypeRepo>();
            services.AddTransient<ICompetitionEntityRepo, CompetitionEntityRepo>();
            services.AddTransient<ICityRepo, CityRepo>();
            services.AddTransient<IBlogRepo, BlogRepo>();
            services.AddTransient<IBlogTypeRepo, BlogTypeRepo>();
            services.AddTransient<ICampusRepo, CampusRepo>();
            services.AddTransient<ICommentRepo, CommentRepo>();
            services.AddTransient<ICompetitionRepo, CompetitionRepo>();
            services.AddTransient<ICompetitionTypeRepo, CompetitionTypeRepo>();
            services.AddTransient<IGroupUniversityRepo, GroupUniversityRepo>();
            services.AddTransient<IImageRepo, ImageRepo>();
            services.AddTransient<IMajorInCompetitionRepo, MajorInCompetitionRepo>();
            services.AddTransient<IMajorRepo, MajorRepo>();
            services.AddTransient<IMajorTypeRepo, MajorTypeRepo>();
            services.AddTransient<IManagerInCompetitionRepo, ManagerInCompetitionRepo>();
            services.AddTransient<IParticipantRepo, ParticipantRepo>();
            services.AddTransient<IRatingRepo, RatingRepo>();
            services.AddTransient<IRoleRepo, RoleRepo>();
            services.AddTransient<ISeedsWalletRepo, SeedsWalletRepo>();
            services.AddTransient<ITeamRepo, TeamRepo>();            
            services.AddTransient<IUniversityRepo, UniversityRepo>();
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IVideoRepo, VideoRepo>();

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
