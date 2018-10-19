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

        public virtual DbSet<BET01> BET01 { get; set; }
        public virtual DbSet<BET01_LANG> BET01_LANG { get; set; }
        public virtual DbSet<BET02> BET02 { get; set; }
        public virtual DbSet<BET02_LANG> BET02_LANG { get; set; }
        public virtual DbSet<HRMT01> HRMT01 { get; set; }
        public virtual DbSet<HRMT24> HRMT24 { get; set; }
        public virtual DbSet<HRMT25> HRMT25 { get; set; }

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
            modelBuilder.Entity<BET01>(entity =>
            {
                entity.HasKey(e => e.CLASS_TYPE);

                entity.Property(e => e.CLASS_TYPE)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.CHG_DATE).HasColumnType("datetime");

                entity.Property(e => e.CHG_PERSON).HasMaxLength(50);

                entity.Property(e => e.CRE_DATE).HasColumnType("datetime");

                entity.Property(e => e.CRE_PERSON).HasMaxLength(50);

                entity.Property(e => e.MEMO).HasMaxLength(50);
            });

            modelBuilder.Entity<BET01_LANG>(entity =>
            {
                entity.HasKey(e => new { e.CLASS_TYPE, e.LANGUAGE_ID });

                entity.Property(e => e.CLASS_TYPE).HasMaxLength(10);

                entity.Property(e => e.LANGUAGE_ID).HasMaxLength(50);

                entity.Property(e => e.CHG_DATE).HasColumnType("datetime");

                entity.Property(e => e.CHG_PERSON).HasMaxLength(50);

                entity.Property(e => e.CLASS_NAME).HasMaxLength(50);

                entity.Property(e => e.CRE_DATE).HasColumnType("datetime");

                entity.Property(e => e.CRE_PERSON).HasMaxLength(50);

                entity.Property(e => e.MEMO).HasMaxLength(50);
            });

            modelBuilder.Entity<BET02>(entity =>
            {
                entity.HasKey(e => e.BULLET_ID);

                entity.Property(e => e.BULLET_ID)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CHG_DATE).HasColumnType("datetime");

                entity.Property(e => e.CHG_PERSON).HasMaxLength(50);

                entity.Property(e => e.CLASS_TYPE).HasMaxLength(10);

                entity.Property(e => e.CRE_DATE).HasColumnType("datetime");

                entity.Property(e => e.CRE_PERSON).HasMaxLength(50);

                entity.Property(e => e.E_DATE).HasColumnType("datetime");

                entity.Property(e => e.ISPUBLISH).HasMaxLength(1);

                entity.Property(e => e.MEMO).HasMaxLength(50);

                entity.Property(e => e.S_DATE).HasColumnType("datetime");

                entity.Property(e => e.TOP_YN).HasMaxLength(50);
            });

            modelBuilder.Entity<BET02_LANG>(entity =>
            {
                entity.HasKey(e => new { e.BULLET_ID, e.LANGUAGE_ID });

                entity.Property(e => e.BULLET_ID).HasMaxLength(50);

                entity.Property(e => e.LANGUAGE_ID).HasMaxLength(50);

                entity.Property(e => e.CHG_DATE).HasColumnType("datetime");

                entity.Property(e => e.CHG_PERSON).HasMaxLength(50);

                entity.Property(e => e.CRE_DATE).HasColumnType("datetime");

                entity.Property(e => e.CRE_PERSON).HasMaxLength(50);

                entity.Property(e => e.SUBJECT).HasMaxLength(200);

                entity.HasOne(d => d.BULLET_)
                    .WithMany(p => p.BET02_LANG)
                    .HasForeignKey(d => d.BULLET_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BET02_LANG_BET02");
            });

            modelBuilder.Entity<HRMT01>(entity =>
            {
                entity.HasKey(e => e.USER_NO);

                entity.Property(e => e.USER_NO)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.ADDRESS).HasMaxLength(500);

                entity.Property(e => e.BIRTHDAY).HasColumnType("date");

                entity.Property(e => e.CHG_DATE).HasColumnType("datetime");

                entity.Property(e => e.CHG_PERSON).HasMaxLength(50);

                entity.Property(e => e.EXP_DATE).HasColumnType("date");

                entity.Property(e => e.E_MAIL).HasMaxLength(100);

                entity.Property(e => e.IDNO).HasMaxLength(20);

                entity.Property(e => e.JOB_TITLE).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(20);

                entity.Property(e => e.PASSWORD).HasMaxLength(50);

                entity.Property(e => e.PHONE).HasMaxLength(50);

                entity.Property(e => e.PHOTO).HasMaxLength(255);

                entity.Property(e => e.SERVICE_UNITS).HasMaxLength(100);

                entity.Property(e => e.SEX)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SNAME).HasMaxLength(20);

                entity.Property(e => e.TEL_NO).HasMaxLength(50);
            });

            modelBuilder.Entity<HRMT24>(entity =>
            {
                entity.HasKey(e => e.ROLE_ID);

                entity.Property(e => e.ROLE_ID)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.CHG_DATE).HasColumnType("datetime");

                entity.Property(e => e.CHG_PERSON).HasMaxLength(50);

                entity.Property(e => e.ROLE_LEVEL).HasColumnType("money");

                entity.Property(e => e.ROLE_NAME).HasMaxLength(255);
            });

            modelBuilder.Entity<HRMT25>(entity =>
            {
                entity.HasKey(e => new { e.USER_NO, e.ROLE_ID });

                entity.Property(e => e.USER_NO).HasMaxLength(50);

                entity.Property(e => e.ROLE_ID).HasMaxLength(10);

                entity.Property(e => e.CHG_DATE).HasColumnType("datetime");

                entity.Property(e => e.CHG_PERSON).HasMaxLength(50);

                entity.Property(e => e.DEFAULT_YN)
                    .IsRequired()
                    .HasMaxLength(1);
            });
        }
    }
}
