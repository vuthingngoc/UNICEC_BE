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
using UNICS.Business.Services.CitySvc;
using UNICS.Business.Services.BlogSvc;
using UNICS.Business.Services.BlogTypeSvc;
using UNICS.Business.Services.CompetitionSvc;
using UNICS.Business.Services.CompetitionTypeSvc;
using UNICS.Business.Services.MajorSvc;
using UNICS.Business.Services.ParticipantSvc;
using UNICS.Business.Services.RoleSvc;
using UNICS.Business.Services.SeedsWalletSvc;
using UNICS.Business.Services.TeamSvc;

using UNICS.Business.Services.UniversitySvc;
using UNICS.Business.Services.UserSvc;
using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;
using UNICS.Data.Repository.ImplRepo.CityRepo;
using UNICS.Data.Repository.ImplRepo.BlogRepo;
using UNICS.Data.Repository.ImplRepo.BlogTypeRepo;
using UNICS.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UNICS.Data.Repository.ImplRepo.CompetitionRepo;
using UNICS.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UNICS.Data.Repository.ImplRepo.EntityTypeRepo;
using UNICS.Data.Repository.ImplRepo.MajorRepo;
using UNICS.Data.Repository.ImplRepo.ParticipantRepo;
using UNICS.Data.Repository.ImplRepo.RoleRepo;
using UNICS.Data.Repository.ImplRepo.SeedsWalletRepo;
using UNICS.Data.Repository.ImplRepo.TeamRepo;
using UNICS.Data.Repository.ImplRepo.UniversityRepo;
using UNICS.Data.Repository.ImplRepo.UserRepo;
using UNICS.Business.Services.ClubActivitySvc;
using UNICS.Business.Services.ClubPreviousSvc;
using UNICS.Business.Services.ClubRoleSvc;
using UNICS.Business.Services.ClubSvc;
using UNICS.Business.Services.CompetitionEntitySvc;
using UNICS.Business.Services.CompetitionInClubSvc;
using UNICS.Business.Services.DepartmentInUniversitySvc;
using UNICS.Business.Services.DepartmentSvc;
using UNICS.Business.Services.EntityTypeSvc;
using UNICS.Business.Services.MemberSvc;
using UNICS.Business.Services.MemberTakesActivitySvc;
using UNICS.Business.Services.ParticipantInTeamSvc;
using UNICS.Data.Repository.ImplRepo.ClubActivityRepo;
using UNICS.Data.Repository.ImplRepo.ClubPreviousRepo;
using UNICS.Data.Repository.ImplRepo.ClubRepo;
using UNICS.Data.Repository.ImplRepo.ClubRoleRepo;
using UNICS.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UNICS.Data.Repository.ImplRepo.DepartmentInUniversityRepo;
using UNICS.Data.Repository.ImplRepo.DepartmentRepo;
using UNICS.Data.Repository.ImplRepo.MemberRepo;
using UNICS.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UNICS.Data.Repository.ImplRepo.ParticipantInTeamRepo;
using UNICS.Business.Services.SponsorSvc;
using UNICS.Business.Services.SponsorInCompetitionSvc;
using UNICS.Data.Repository.ImplRepo.SponsorRepo;
using UNICS.Data.Repository.ImplRepo.SponsorInCompetitionRepo;

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
                config.DefaultApiVersion = new ApiVersion(1, 0);
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
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IBlogTypeService, BlogTypeService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IClubActivityService, ClubActivityService>();
            services.AddScoped<IClubPreviousService, ClubPreviousService>();
            services.AddScoped<IClubRoleService, ClubRoleService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<ICompetitionEntityService, CompetitionEntityService>();
            services.AddScoped<ICompetitionInClubService, CompetitionInClubService>();
            services.AddScoped<ICompetitionService, CompetitionService>();
            services.AddScoped<ICompetitionTypeService, CompetitionTypeService>();
            services.AddScoped<IDepartmentInUniversityService, DepartmentInUniversityService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEntityTypeService, EntityTypeService>();
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMemberTakesActivityService, MemberTakesActivityService>();
            services.AddScoped<IParticipantInTeamService, ParticipantInTeamService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISeedsWalletService, SeedsWalletService>();
            services.AddScoped<ISponsorService, SponsorService>();
            services.AddScoped<ISponsorInCompetitionService, SponsorInCompetitionService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUniversityService, UniversityService>();      
            services.AddScoped<IUserService, UserService>();

            // Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IBlogRepo, BlogRepo>();
            services.AddTransient<IBlogTypeRepo, BlogTypeRepo>();
            services.AddTransient<ICityRepo, CityRepo>();
            services.AddTransient<IClubActivityRepo, ClubActivityRepo>();
            services.AddTransient<IClubPreviousRepo, ClubPreviousRepo>();
            services.AddTransient<IClubRepo, ClubRepo>();
            services.AddTransient<IClubRoleRepo, ClubRoleRepo>();
            services.AddTransient<ICompetitionEntityRepo, CompetitionEntityRepo>();
            services.AddTransient<ICompetitionInClubRepo, CompetitionInClubRepo>();
            services.AddTransient<ICompetitionRepo, CompetitionRepo>();
            services.AddTransient<ICompetitionTypeRepo, CompetitionTypeRepo>();
            services.AddTransient<IDepartmentInUniversityRepo, DepartmentInUniversityRepo>();
            services.AddTransient<IDepartmentRepo, DepartmentRepo>();
            services.AddTransient<IEntityTypeRepo, EntityTypeRepo>();                        
            services.AddTransient<IMajorRepo, MajorRepo>();
            services.AddTransient<IMemberRepo, MemberRepo>();
            services.AddTransient<IMemberRepo, MemberRepo>();
            services.AddTransient<IMemberTakesActivityRepo, MemberTakesActivityRepo>();
            services.AddTransient<IParticipantInTeamRepo, ParticipantInTeamRepo>();
            services.AddTransient<IParticipantRepo, ParticipantRepo>();
            services.AddTransient<IRoleRepo, RoleRepo>();
            services.AddTransient<ISeedsWalletRepo, SeedsWalletRepo>();
            services.AddTransient<ISponsorRepo, SponsorRepo>();
            services.AddTransient<ISponsorInCompetitionRepo, SponsorInCompetitionRepo>();
            services.AddTransient<ITeamRepo, TeamRepo>();
            services.AddTransient<IUniversityRepo, UniversityRepo>();
            services.AddTransient<IUserRepo, UserRepo>();

            //----------------------------------------------FIREBASE-------------------------
            string path = Path.Combine(Directory.GetCurrentDirectory(), "unics-e46a4-firebase-adminsdk-td0dr-86cc1f1def.json");
            //add firebase admin
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(path),
                ProjectId = "unics-e46a4",
                ServiceAccountId = "firebase-adminsdk-td0dr@unics-e46a4.iam.gserviceaccount.com"

            });


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
