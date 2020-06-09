using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API.Core.Entities
{
    public partial class MANBU_qaContext : DbContext
    {
        public MANBU_qaContext()
        {
        }

        public MANBU_qaContext(DbContextOptions<MANBU_qaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TxClues> TxClues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        public DbSet<TxClues> Clues { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TxClues>(entity =>
            {
                entity.ToTable("tx_clues");

                entity.HasIndex(e => e.Agentid)
                    .HasName("index_agentid");

                entity.HasIndex(e => e.Casetype)
                    .HasName("index_casetype");

                entity.HasIndex(e => e.Followupstate)
                    .HasName("index_followupstate");

                entity.HasIndex(e => e.LastFollowId)
                    .HasName("index_last_follow_id");

                entity.HasIndex(e => e.Licenseno)
                    .HasName("index_licenseno");

                entity.HasIndex(e => e.Smsid)
                    .HasName("index_smsid");

                entity.HasIndex(e => e.Smsrecivedtime)
                    .HasName("index_smsrecivedtime");

                entity.HasIndex(e => e.Source)
                    .HasName("index_source");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AcceptedState)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.AcceptedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'2001-01-01 00:00:00'");

                entity.Property(e => e.Accidentremark)
                    .HasColumnName("accidentremark")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.AgentName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.AgentType)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Agentid)
                    .HasColumnName("agentid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Area)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CarOwner)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CarVin)
                    .HasColumnName("CarVIN")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Casetype)
                    .HasColumnName("casetype")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.ChosedCompanyId)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.ChosedCompanyName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ChosedModelId)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ChosedModelName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.City)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CityName)
                    .HasColumnName("city_name")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.CityName1)
                    .IsRequired()
                    .HasColumnName("CityName")
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ClueFromType)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.CurrentAgentId)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Dangerarea)
                    .HasColumnName("dangerarea")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.Deleted)
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ExpectedAddress)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ExpectedFinishedTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'2001-01-01 00:00:00'");

                entity.Property(e => e.ExpectedLat)
                    .HasColumnType("double(20,17)")
                    .HasDefaultValueSql("'0.00000000000000000'");

                entity.Property(e => e.ExpectedLng)
                    .HasColumnType("double(20,17)")
                    .HasDefaultValueSql("'0.00000000000000000'");

                entity.Property(e => e.Followupstate)
                    .HasColumnName("followupstate")
                    .HasColumnType("int(2)")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.FromAgentName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.FromRate)
                    .HasColumnType("decimal(6,4)")
                    .HasDefaultValueSql("'0.0000'");

                entity.Property(e => e.FromSettledState)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FromSettledTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'2001-01-01 00:00:00'");

                entity.Property(e => e.HasInsureInfo).HasColumnType("int(1)");

                entity.Property(e => e.IsDrivering)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.IsMany)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.LastFollowId)
                    .HasColumnName("last_follow_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Licenseno)
                    .IsRequired()
                    .HasColumnName("licenseno")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.MaintainAmount)
                    .HasColumnType("decimal(11,4)")
                    .HasDefaultValueSql("'0.0000'");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.MoldName).HasColumnType("varchar(500)");

                entity.Property(e => e.Only4s)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.OrderNum)
                    .IsRequired()
                    .HasColumnType("varchar(30)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Profits)
                    .HasColumnType("decimal(11,4)")
                    .HasDefaultValueSql("'0.0000'");

                entity.Property(e => e.Province)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ProvinceName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ReceiveCarAddress)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ReceiveLat)
                    .HasColumnType("double(20,17)")
                    .HasDefaultValueSql("'0.00000000000000000'");

                entity.Property(e => e.ReceiveLng)
                    .HasColumnType("double(20,17)")
                    .HasDefaultValueSql("'0.00000000000000000'");

                entity.Property(e => e.ReportCaseNum).HasColumnType("varchar(100)");

                entity.Property(e => e.ReportCasePeople).HasColumnType("varchar(150)");

                entity.Property(e => e.Smsid)
                    .HasColumnName("smsid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Smsrecivedtime)
                    .HasColumnName("smsrecivedtime")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasColumnType("int(4)");

                entity.Property(e => e.Sourcename)
                    .IsRequired()
                    .HasColumnName("sourcename")
                    .HasColumnType("varchar(100)");
                

                entity.Property(e => e.ToAgentId)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ToAgentName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ToRate)
                    .HasColumnType("decimal(6,4)")
                    .HasDefaultValueSql("'0.0000'");

                entity.Property(e => e.ToSettledState)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ToSettledTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'2001-01-01 00:00:00'");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });
        }
    }
}
