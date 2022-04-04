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
using UniCEC.Business.Services.CitySvc;
using UniCEC.Business.Services.BlogSvc;
using UniCEC.Business.Services.BlogTypeSvc;
using UniCEC.Business.Services.CompetitionSvc;
using UniCEC.Business.Services.CompetitionTypeSvc;
using UniCEC.Business.Services.MajorSvc;
using UniCEC.Business.Services.ParticipantSvc;
using UniCEC.Business.Services.RoleSvc;
using UniCEC.Business.Services.SeedsWalletSvc;
using UniCEC.Business.Services.TeamSvc;

using UniCEC.Business.Services.UniversitySvc;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.Repository.ImplRepo.BlogRepo;
using UniCEC.Data.Repository.ImplRepo.BlogTypeRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UniCEC.Data.Repository.ImplRepo.EntityTypeRepo;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.RoleRepo;
using UniCEC.Data.Repository.ImplRepo.SeedsWalletRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Business.Services.ClubActivitySvc;
using UniCEC.Business.Services.ClubPreviousSvc;
using UniCEC.Business.Services.ClubRoleSvc;
using UniCEC.Business.Services.ClubSvc;
using UniCEC.Business.Services.CompetitionEntitySvc;
using UniCEC.Business.Services.CompetitionInClubSvc;
using UniCEC.Business.Services.DepartmentInUniversitySvc;
using UniCEC.Business.Services.DepartmentSvc;
using UniCEC.Business.Services.EntityTypeSvc;
using UniCEC.Business.Services.MemberSvc;
using UniCEC.Business.Services.MemberTakesActivitySvc;
using UniCEC.Business.Services.ParticipantInTeamSvc;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubPreviousRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRoleRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentInUniversityRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo;
using UniCEC.Business.Services.SponsorSvc;
using UniCEC.Business.Services.SponsorInCompetitionSvc;
using UniCEC.Data.Repository.ImplRepo.SponsorRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;

namespace UniCEC.API
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
            services.AddDbContext<UniCECContext>();
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
            string path = Path.Combine(Directory.GetCurrentDirectory(), "UniCEC-e46a4-firebase-adminsdk-td0dr-86cc1f1def.json");
            //add firebase admin
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(path),
                ProjectId = "UniCEC-e46a4",
                ServiceAccountId = "firebase-adminsdk-td0dr@UniCEC-e46a4.iam.gserviceaccount.com"

            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "UniCEC.API",
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniCEC.API v1"));
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