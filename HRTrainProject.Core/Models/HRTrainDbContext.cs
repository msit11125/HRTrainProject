using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRTrainProject.Core.Models
{
    public partial class HRTrainDbContext : DbContext
    {
        public HRTrainDbContext()
        {
        }

        public HRTrainDbContext(DbContextOptions<HRTrainDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bet01> Bet01 { get; set; }
        public virtual DbSet<Bet01Lang> Bet01Lang { get; set; }
        public virtual DbSet<Bet02> Bet02 { get; set; }
        public virtual DbSet<Bet02Lang> Bet02Lang { get; set; }
        public virtual DbSet<Hrmt01> Hrmt01 { get; set; }
        public virtual DbSet<Hrmt24> Hrmt24 { get; set; }
        public virtual DbSet<Hrmt25> Hrmt25 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=HRTrainDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bet01>(entity =>
            {
                entity.HasKey(e => e.ClassType);

                entity.ToTable("BET01");

                entity.Property(e => e.ClassType)
                    .HasColumnName("CLASS_TYPE")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.ChgDate)
                    .HasColumnName("CHG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChgPerson)
                    .HasColumnName("CHG_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.CreDate)
                    .HasColumnName("CRE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.CrePerson)
                    .HasColumnName("CRE_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderBy).HasColumnName("ORDER_BY");

                entity.Property(e => e.ShowCount).HasColumnName("SHOW_COUNT");
            });

            modelBuilder.Entity<Bet01Lang>(entity =>
            {
                entity.HasKey(e => new { e.ClassType, e.LanguageId });

                entity.ToTable("BET01_LANG");

                entity.Property(e => e.ClassType)
                    .HasColumnName("CLASS_TYPE")
                    .HasMaxLength(10);

                entity.Property(e => e.LanguageId)
                    .HasColumnName("LANGUAGE_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.ChgDate)
                    .HasColumnName("CHG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChgPerson)
                    .HasColumnName("CHG_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.ClassName)
                    .HasColumnName("CLASS_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.CreDate)
                    .HasColumnName("CRE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.CrePerson)
                    .HasColumnName("CRE_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Bet02>(entity =>
            {
                entity.HasKey(e => e.BulletId);

                entity.ToTable("BET02");

                entity.Property(e => e.BulletId)
                    .HasColumnName("BULLET_ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.ChgDate)
                    .HasColumnName("CHG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChgPerson)
                    .HasColumnName("CHG_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.ClassType)
                    .HasColumnName("CLASS_TYPE")
                    .HasMaxLength(10);

                entity.Property(e => e.CreDate)
                    .HasColumnName("CRE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.CrePerson)
                    .HasColumnName("CRE_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.EDate)
                    .HasColumnName("E_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ispublish)
                    .HasColumnName("ISPUBLISH")
                    .HasMaxLength(1);

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasMaxLength(50);

                entity.Property(e => e.SDate)
                    .HasColumnName("S_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.TopYn)
                    .HasColumnName("TOP_YN")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Bet02Lang>(entity =>
            {
                entity.HasKey(e => new { e.BulletId, e.LanguageId });

                entity.ToTable("BET02_LANG");

                entity.Property(e => e.BulletId)
                    .HasColumnName("BULLET_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.LanguageId)
                    .HasColumnName("LANGUAGE_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.ChgDate)
                    .HasColumnName("CHG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChgPerson)
                    .HasColumnName("CHG_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.ContentTxt).HasColumnName("CONTENT_TXT");

                entity.Property(e => e.CreDate)
                    .HasColumnName("CRE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.CrePerson)
                    .HasColumnName("CRE_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.Subject)
                    .HasColumnName("SUBJECT")
                    .HasMaxLength(200);

                entity.HasOne(d => d.Bullet)
                    .WithMany(p => p.Bet02Lang)
                    .HasForeignKey(d => d.BulletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BET02_LANG_BET02");
            });

            modelBuilder.Entity<Hrmt01>(entity =>
            {
                entity.HasKey(e => e.UserNo);

                entity.ToTable("HRMT01");

                entity.Property(e => e.UserNo)
                    .HasColumnName("USER_NO")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("ADDRESS")
                    .HasMaxLength(500);

                entity.Property(e => e.Birthday)
                    .HasColumnName("BIRTHDAY")
                    .HasColumnType("date");

                entity.Property(e => e.ChgDate)
                    .HasColumnName("CHG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChgPerson)
                    .HasColumnName("CHG_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.EMail)
                    .HasColumnName("E_MAIL")
                    .HasMaxLength(100);

                entity.Property(e => e.ExpDate)
                    .HasColumnName("EXP_DATE")
                    .HasColumnType("date");

                entity.Property(e => e.Idno)
                    .HasColumnName("IDNO")
                    .HasMaxLength(20);

                entity.Property(e => e.InitStatus).HasColumnName("INIT_STATUS");

                entity.Property(e => e.JobTitle)
                    .HasColumnName("JOB_TITLE")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(20);

                entity.Property(e => e.Password)
                    .HasColumnName("PASSWORD")
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasMaxLength(50);

                entity.Property(e => e.Photo)
                    .HasColumnName("PHOTO")
                    .HasMaxLength(255);

                entity.Property(e => e.ServiceUnits)
                    .HasColumnName("SERVICE_UNITS")
                    .HasMaxLength(100);

                entity.Property(e => e.Sex)
                    .HasColumnName("SEX")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Sname)
                    .HasColumnName("SNAME")
                    .HasMaxLength(20);

                entity.Property(e => e.Tall).HasColumnName("TALL");

                entity.Property(e => e.TelNo)
                    .HasColumnName("TEL_NO")
                    .HasMaxLength(50);

                entity.Property(e => e.UserStatus).HasColumnName("USER_STATUS");

                entity.Property(e => e.Weight).HasColumnName("WEIGHT");
            });

            modelBuilder.Entity<Hrmt24>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("HRMT24");

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.ChgDate)
                    .HasColumnName("CHG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChgPerson)
                    .HasColumnName("CHG_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.RoleLevel)
                    .HasColumnName("ROLE_LEVEL")
                    .HasColumnType("money");

                entity.Property(e => e.RoleName)
                    .HasColumnName("ROLE_NAME")
                    .HasMaxLength(255);

                entity.Property(e => e.RoleType).HasColumnName("ROLE_TYPE");
            });

            modelBuilder.Entity<Hrmt25>(entity =>
            {
                entity.HasKey(e => new { e.UserNo, e.RoleId });

                entity.ToTable("HRMT25");

                entity.Property(e => e.UserNo)
                    .HasColumnName("USER_NO")
                    .HasMaxLength(50);

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasMaxLength(10);

                entity.Property(e => e.ChgDate)
                    .HasColumnName("CHG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChgPerson)
                    .HasColumnName("CHG_PERSON")
                    .HasMaxLength(50);

                entity.Property(e => e.DefaultYn)
                    .IsRequired()
                    .HasColumnName("DEFAULT_YN")
                    .HasMaxLength(1);
            });
        }
    }
}
