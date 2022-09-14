using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;
using UniCEC.Business.Services.ActivitiesEntitySvc;
using UniCEC.Business.Services.CitySvc;
using UniCEC.Business.Services.ClubRoleSvc;
using UniCEC.Business.Services.ClubSvc;
using UniCEC.Business.Services.CompetitionActivitySvc;
using UniCEC.Business.Services.CompetitionEntitySvc;
using UniCEC.Business.Services.CompetitionHistorySvc;
using UniCEC.Business.Services.CompetitionRoleSvc;
using UniCEC.Business.Services.CompetitionRoundSvc;
using UniCEC.Business.Services.CompetitionSvc;
using UniCEC.Business.Services.CompetitionTypeSvc;
using UniCEC.Business.Services.DepartmentSvc;
using UniCEC.Business.Services.EntityTypeSvc;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Services.FirebaseSvc;
using UniCEC.Business.Services.MajorSvc;
using UniCEC.Business.Services.MatchSvc;
using UniCEC.Business.Services.CompetitionRoundTypeSvc;
using UniCEC.Business.Services.MemberSvc;
using UniCEC.Business.Services.MemberTakesActivitySvc;
using UniCEC.Business.Services.NotificationSvc;
using UniCEC.Business.Services.ParticipantInTeamSvc;
using UniCEC.Business.Services.ParticipantSvc;
using UniCEC.Business.Services.RoleSvc;
using UniCEC.Business.Services.SeedsWalletSvc;
using UniCEC.Business.Services.TeamInMatchSvc;
using UniCEC.Business.Services.TeamInRoundSvc;
using UniCEC.Business.Services.TeamSvc;
using UniCEC.Business.Services.UniversitySvc;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.Repository.ImplRepo.ActivitiesEntityRepo;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRoleRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInMajorRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoleRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.Repository.ImplRepo.EntityTypeRepo;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.Repository.ImplRepo.MatchRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundTypeRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.Repository.ImplRepo.NotificationRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.RoleRepo;
using UniCEC.Data.Repository.ImplRepo.SeedsWalletRepo;
using UniCEC.Data.Repository.ImplRepo.TeamInMatchRepo;
using UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRoleRepo;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;

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
            services.AddScoped<IActivitiesEntityService, ActivitiesEntityService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICompetitionActivityService, CompetitionActivityService>();
            services.AddScoped<IClubRoleService, ClubRoleService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<ICompetitionActivityService, CompetitionActivityService>();         
            services.AddScoped<ICompetitionRoleService, CompetitionRoleService>();
            services.AddScoped<ICompetitionEntityService, CompetitionEntityService>();
            services.AddScoped<ICompetitionService, CompetitionService>();
            services.AddScoped<ICompetitionTypeService, CompetitionTypeService>();
            services.AddScoped<ICompetitionRoundService, CompetitionRoundService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEntityTypeService, EntityTypeService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMemberTakesActivityService, MemberTakesActivityService>();
            services.AddScoped<IParticipantInTeamService, ParticipantInTeamService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISeedsWalletService, SeedsWalletService>();
            services.AddScoped<ICompetitionHistoryService, CompetitionHistoryService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITeamInRoundService, TeamInRoundService>();
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<ICompetitionRoundTypeService, CompetitionRoundTypeService>();
            services.AddScoped<ITeamInMatchService, TeamInMatchService>();

            // Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IActivitiesEntityRepo, ActivitiesEntityRepo>();
            services.AddTransient<ICityRepo, CityRepo>();
            services.AddTransient<ICompetitionActivityRepo, CompetitionActivityRepo>();
            services.AddTransient<IClubRepo, ClubRepo>();
            services.AddTransient<IClubRoleRepo, ClubRoleRepo>();
            services.AddTransient<ICompetitionHistoryRepo, CompetitionHistoryRepo>();
            services.AddTransient<ICompetitionActivityRepo, CompetitionActivityRepo>();
            services.AddTransient<ICompetitionEntityRepo, CompetitionEntityRepo>();
            services.AddTransient<ICompetitionInClubRepo, CompetitionInClubRepo>();
            services.AddTransient<ICompetitionInMajorRepo, CompetitionInMajorRepo>();
            services.AddTransient<ICompetitionRepo, CompetitionRepo>();
            services.AddTransient<ICompetitionTypeRepo, CompetitionTypeRepo>();
            services.AddTransient<ICompetitionRoundRepo, CompetitionRoundRepo>();
            services.AddTransient<IEntityTypeRepo, EntityTypeRepo>();
            services.AddTransient<ICompetitionRoleRepo, CompetitionRoleRepo>();
            services.AddTransient<IMajorRepo, MajorRepo>();          
            services.AddTransient<IDepartmentRepo, DepartmentRepo>();
            services.AddTransient<IMemberRepo, MemberRepo>();
            services.AddTransient<IMemberTakesActivityRepo, MemberTakesActivityRepo>();
            services.AddTransient<IParticipantInTeamRepo, ParticipantInTeamRepo>();
            services.AddTransient<IParticipantRepo, ParticipantRepo>();
            services.AddTransient<IRoleRepo, RoleRepo>();
            services.AddTransient<ISeedsWalletRepo, SeedsWalletRepo>();
            services.AddTransient<ITeamRepo, TeamRepo>();
            services.AddTransient<ITeamRoleRepo, TeamRoleRepo>();
            services.AddTransient<ITeamInRoundRepo, TeamInRoundRepo>();
            services.AddTransient<IUniversityRepo, UniversityRepo>();
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IMemberInCompetitionRepo, MemberInCompetitionRepo>();
            services.AddTransient<INotificationRepo, NotificationRepo>();
            services.AddTransient<IMatchRepo, MatchRepo>();
            services.AddTransient<ICompetitionRoundTypeRepo, CompetitionRoundTypeRepo>();
            services.AddTransient<ITeamInMatchRepo, TeamInMatchRepo>();

            //----------------------------------------------FIREBASE-------------------------
            string path = Path.Combine(Directory.GetCurrentDirectory(), Configuration["Jwt:Firebase:FileConfig"]);
            //add firebase admin sdk
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(path),
                ProjectId = Configuration["Jwt:Firebase:ValidAudience"],
                ServiceAccountId = Configuration["Jwt:Firebase:ServiceAccount"]
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                //opt.Authority = Configuration["Jwt:Firebase:ValidIssuer"];
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Firebase:ValidIssuer"],
                    ValidAudience = Configuration["Jwt:Firebase:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Firebase:PrivateKey"]))
                };
            });
            //enable CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowOrigin",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders(new string[] { "Authorization", "authorization", "X-Device-Token" });
                    });
            });

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "UniCEC.API",
                    Version = "v1",
                    Description = "APIs for UniCEC"
                });


                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. " +
                                    "\n\nEnter 'Bearer' [space] and then your token in the text input below. " +
                                      "\n\nExample: 'Bearer 12345abcde'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        securityScheme,
                        new string[]{ }
                    }
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Do not change existed code in this method, please ! You can ADD MORE code for your business !!!
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniCEC.API v1");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowOrigin");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
