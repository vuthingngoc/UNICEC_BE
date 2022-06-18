using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace UniCEC.Data.Models.DB
{
    public partial class UniCECContext : DbContext
    {
        private IConfiguration _configuration;
        public UniCECContext()
        {
        }

        public UniCECContext(DbContextOptions<UniCECContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<ActivitiesEntity> ActivitiesEntities { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<ClubRole> ClubRoles { get; set; }
        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<CompetitionActivity> CompetitionActivities { get; set; }
        public virtual DbSet<CompetitionEntity> CompetitionEntities { get; set; }
        public virtual DbSet<CompetitionInClub> CompetitionInClubs { get; set; }
        public virtual DbSet<CompetitionInDepartment> CompetitionInDepartments { get; set; }
        public virtual DbSet<CompetitionManager> CompetitionManagers { get; set; }
        public virtual DbSet<CompetitionRole> CompetitionRoles { get; set; }
        public virtual DbSet<CompetitionRound> CompetitionRounds { get; set; }
        public virtual DbSet<CompetitionType> CompetitionTypes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Influencer> Influencers { get; set; }
        public virtual DbSet<InfluencerInCompetition> InfluencerInCompetitions { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberTakesActivity> MemberTakesActivities { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<ParticipantInTeam> ParticipantInTeams { get; set; }
        public virtual DbSet<RegisterForm> RegisterForms { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SeedsWallet> SeedsWallets { get; set; }
        public virtual DbSet<Sponsor> Sponsors { get; set; }
        public virtual DbSet<SponsorInCompetition> SponsorInCompetitions { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamInRound> TeamInRounds { get; set; }
        public virtual DbSet<TeamRole> TeamRoles { get; set; }
        public virtual DbSet<Term> Terms { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("UniCEC"));
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ActivitiesEntity>(entity =>
            {
                entity.ToTable("ActivitiesEntity");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionActivityId).HasColumnName("CompetitionActivityID");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.CompetitionActivity)
                    .WithMany(p => p.ActivitiesEntities)
                    .HasForeignKey(d => d.CompetitionActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Activitie__Compe__5FB337D6");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Club>(entity =>
            {
                entity.ToTable("Club");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClubContact)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClubFanpage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Founding).HasColumnType("datetime");

                entity.Property(e => e.Image)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UniversityId).HasColumnName("UniversityID");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Clubs)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Club__University__60A75C0F");
            });

            modelBuilder.Entity<ClubRole>(entity =>
            {
                entity.ToTable("ClubRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Competition>(entity =>
            {
                entity.ToTable("Competition");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AddressName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CompetitionTypeId).HasColumnName("CompetitionTypeID");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.EndTimeRegister).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.SeedsCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.StartTimeRegister).HasColumnType("datetime");

                entity.HasOne(d => d.CompetitionType)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.CompetitionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Compe__619B8048");
            });

            modelBuilder.Entity<CompetitionActivity>(entity =>
            {
                entity.ToTable("CompetitionActivity");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Ending).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SeedsCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.CompetitionActivities)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Compe__628FA481");
            });

            modelBuilder.Entity<CompetitionEntity>(entity =>
            {
                entity.ToTable("CompetitionEntity");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.CompetitionEntities)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Compe__6383C8BA");
            });

            modelBuilder.Entity<CompetitionInClub>(entity =>
            {
                entity.ToTable("CompetitionInClub");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClubId).HasColumnName("ClubID");

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.CompetitionInClubs)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__ClubI__6477ECF3");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.CompetitionInClubs)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Compe__656C112C");
            });

            modelBuilder.Entity<CompetitionInDepartment>(entity =>
            {
                entity.ToTable("CompetitionInDepartment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.CompetitionInDepartments)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Compe__66603565");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.CompetitionInDepartments)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Depar__6754599E");
            });

            modelBuilder.Entity<CompetitionManager>(entity =>
            {
                entity.ToTable("CompetitionManager");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionInClubId).HasColumnName("CompetitionInClubID");

                entity.Property(e => e.CompetitionRoleId).HasColumnName("CompetitionRoleID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.CompetitionInClub)
                    .WithMany(p => p.CompetitionManagers)
                    .HasForeignKey(d => d.CompetitionInClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Compe__68487DD7");

                entity.HasOne(d => d.CompetitionRole)
                    .WithMany(p => p.CompetitionManagers)
                    .HasForeignKey(d => d.CompetitionRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Compe__693CA210");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CompetitionManagers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Membe__6A30C649");
            });

            modelBuilder.Entity<CompetitionRole>(entity =>
            {
                entity.ToTable("CompetitionRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CompetitionRound>(entity =>
            {
                entity.ToTable("CompetitionRound");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.CompetitionRounds)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Competiti__Compe__6B24EA82");
            });

            modelBuilder.Entity<CompetitionType>(entity =>
            {
                entity.ToTable("CompetitionType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Influencer>(entity =>
            {
                entity.ToTable("Influencer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<InfluencerInCompetition>(entity =>
            {
                entity.ToTable("InfluencerInCompetition");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.InfluencerId).HasColumnName("InfluencerID");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.InfluencerInCompetitions)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Influence__Compe__6C190EBB");

                entity.HasOne(d => d.Influencer)
                    .WithMany(p => p.InfluencerInCompetitions)
                    .HasForeignKey(d => d.InfluencerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Influence__Influ__6D0D32F4");
            });

            modelBuilder.Entity<Major>(entity =>
            {
                entity.ToTable("Major");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.MajorCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UniversityId).HasColumnName("UniversityID");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Majors)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Major__Departmen__6E01572D");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Majors)
                    .HasForeignKey(d => d.UniversityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Major__Universit__6EF57B66");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClubId).HasColumnName("ClubID");

                entity.Property(e => e.ClubRoleId).HasColumnName("ClubRoleID");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.TermId).HasColumnName("TermID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Member__ClubID__6FE99F9F");

                entity.HasOne(d => d.ClubRole)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.ClubRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Member__ClubRole__70DDC3D8");

                entity.HasOne(d => d.Term)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.TermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TermID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Member__UserID__71D1E811");
            });

            modelBuilder.Entity<MemberTakesActivity>(entity =>
            {
                entity.ToTable("MemberTakesActivity");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookerId).HasColumnName("BookerID");

                entity.Property(e => e.CompetitionActivityId).HasColumnName("CompetitionActivityID");

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.CompetitionActivity)
                    .WithMany(p => p.MemberTakesActivities)
                    .HasForeignKey(d => d.CompetitionActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MemberTak__Compe__73BA3083");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberTakesActivities)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MemberTak__Membe__74AE54BC");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("Participant");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.RegisterTime).HasColumnType("datetime");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__Compe__75A278F5");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__Participa__Membe__76969D2E");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__Stude__778AC167");
            });

            modelBuilder.Entity<ParticipantInTeam>(entity =>
            {
                entity.ToTable("ParticipantInTeam");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.TeamRoleId).HasColumnName("TeamRoleID");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.ParticipantInTeams)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__Parti__787EE5A0");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.ParticipantInTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__TeamI__797309D9");

                entity.HasOne(d => d.TeamRole)
                    .WithMany(p => p.ParticipantInTeams)
                    .HasForeignKey(d => d.TeamRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__TeamR__7A672E12");
            });

            modelBuilder.Entity<RegisterForm>(entity =>
            {
                entity.ToTable("RegisterForm");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("URL");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.RegisterForms)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RegisterF__ClubI__7B5B524B");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SeedsWallet>(entity =>
            {
                entity.ToTable("SeedsWallet");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .HasColumnName("ID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.SeedsWallets)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SeedsWall__Stude__7C4F7684");
            });

            modelBuilder.Entity<Sponsor>(entity =>
            {
                entity.ToTable("Sponsor");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SponsorInCompetition>(entity =>
            {
                entity.ToTable("SponsorInCompetition");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment).HasMaxLength(4000);

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Feedback).HasMaxLength(4000);

                entity.Property(e => e.ReviewDate).HasColumnType("datetime");

                entity.Property(e => e.SponsorId).HasColumnName("SponsorID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.SponsorInCompetitions)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SponsorIn__Compe__7D439ABD");

                entity.HasOne(d => d.Sponsor)
                    .WithMany(p => p.SponsorInCompetitions)
                    .HasForeignKey(d => d.SponsorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SponsorIn__Spons__7E37BEF6");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitionId).HasColumnName("CompetitionID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.InvitedCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team__Competitio__7F2BE32F");
            });

            modelBuilder.Entity<TeamInRound>(entity =>
            {
                entity.ToTable("TeamInRound");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Result)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.RoundId).HasColumnName("RoundID");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.HasOne(d => d.Round)
                    .WithMany(p => p.TeamInRounds)
                    .HasForeignKey(d => d.RoundId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamInRou__Round__00200768");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamInRounds)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamInRou__TeamI__01142BA1");
            });

            modelBuilder.Entity<TeamRole>(entity =>
            {
                entity.ToTable("TeamRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Term>(entity =>
            {
                entity.ToTable("Term");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.ToTable("University");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Closing)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Founding).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Openning)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UniCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Universities)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Universit__CityI__02084FDA");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Dob)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DOB");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.MajorId).HasColumnName("MajorID");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.SponsorId).HasColumnName("SponsorID");

                entity.Property(e => e.StudentCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UniversityId).HasColumnName("UniversityID");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.MajorId)
                    .HasConstraintName("FK__User__MajorID__02FC7413");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__RoleID__03F0984C");

                entity.HasOne(d => d.Sponsor)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.SponsorId)
                    .HasConstraintName("FK__User__SponsorID__04E4BC85");

                entity.HasOne(d => d.University)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UniversityId)
                    .HasConstraintName("FK__User__University__05D8E0BE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
