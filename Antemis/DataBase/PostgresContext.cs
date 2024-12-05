using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Antemis.Database;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Work> Works { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=postgres;Username=postgres;Password=genger404");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerinn).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.Customerinn).HasColumnName("customerinn");
            entity.Property(e => e.Arrivaldate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("arrivaldate");
            entity.Property(e => e.Hotelid).HasColumnName("hotelid");
            entity.Property(e => e.Leavingdate).HasColumnName("leavingdate");
            entity.Property(e => e.Prepayment)
                .HasDefaultValue(0)
                .HasColumnName("prepayment");
            entity.Property(e => e.Roomnumber).HasColumnName("roomnumber");

            entity.HasOne(d => d.CustomerinnNavigation).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.Customerinn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customers_customerinn_fkey");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Hotelid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customers_hotelid_fkey");

            entity.HasOne(d => d.Room).WithMany(p => p.Customers)
                .HasForeignKey(d => new { d.Hotelid, d.Roomnumber })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cust_to_rooms_ref");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Hotelid).HasName("hotels_pkey");

            entity.ToTable("hotels");

            entity.HasIndex(e => e.Hotelinn, "hotels_hotelinn_key").IsUnique();

            entity.Property(e => e.Hotelid).HasColumnName("hotelid");
            entity.Property(e => e.Hoteldirectorinn).HasColumnName("hoteldirectorinn");
            entity.Property(e => e.Hotelimage)
                .HasDefaultValueSql("'hotel_icon.png'::text")
                .HasColumnName("hotelimage");
            entity.Property(e => e.Hotelinn).HasColumnName("hotelinn");
            entity.Property(e => e.Hotelname).HasColumnName("hotelname");
            entity.Property(e => e.Hotelownerinn).HasColumnName("hotelownerinn");
            entity.Property(e => e.Hotelpassword).HasColumnName("hotelpassword");

            entity.HasOne(d => d.HoteldirectorinnNavigation).WithMany(p => p.HotelHoteldirectorinnNavigations)
                .HasForeignKey(d => d.Hoteldirectorinn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotels_hoteldirectorinn_fkey");

            entity.HasOne(d => d.HotelownerinnNavigation).WithMany(p => p.HotelHotelownerinnNavigations)
                .HasForeignKey(d => d.Hotelownerinn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotels_hotelownerinn_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Inn).HasName("persons_pkey");

            entity.ToTable("persons");

            entity.Property(e => e.Inn).HasColumnName("inn");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .HasColumnName("gender");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Patronimic).HasColumnName("patronimic");
            entity.Property(e => e.Surname).HasColumnName("surname");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("reservations");

            entity.Property(e => e.Customerinn).HasColumnName("customerinn");
            entity.Property(e => e.Hotelid).HasColumnName("hotelid");
            entity.Property(e => e.Roomnumber).HasColumnName("roomnumber");

            entity.HasOne(d => d.CustomerinnNavigation).WithMany()
                .HasForeignKey(d => d.Customerinn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservations_customerinn_fkey");

            entity.HasOne(d => d.Hotel).WithMany()
                .HasForeignKey(d => d.Hotelid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservations_hotelid_fkey");

            entity.HasOne(d => d.Room).WithMany()
                .HasForeignKey(d => new { d.Hotelid, d.Roomnumber })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("res_to_rooms_ref");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => new { e.Hotelid, e.Roomnumber }).HasName("rooms_primary_key_check");

            entity.ToTable("rooms");

            entity.Property(e => e.Hotelid).HasColumnName("hotelid");
            entity.Property(e => e.Roomnumber).HasColumnName("roomnumber");
            entity.Property(e => e.Imagename)
                .HasDefaultValueSql("'room_icon.png'::text")
                .HasColumnName("imagename");
            entity.Property(e => e.Isavaible)
                .HasDefaultValue(true)
                .HasColumnName("isavaible");
            entity.Property(e => e.Placesamount)
                .HasDefaultValue(2)
                .HasColumnName("placesamount");
            entity.Property(e => e.Priceforday).HasColumnName("priceforday");
            entity.Property(e => e.Roomdescryprion)
                .HasDefaultValueSql("'Информация не добавлена'::text")
                .HasColumnName("roomdescryprion");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.Hotelid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rooms_hotelid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Inn, "users_inn_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Imagename)
                .HasDefaultValueSql("'user_icon.png'::text")
                .HasColumnName("imagename");
            entity.Property(e => e.Inn).HasColumnName("inn");
            entity.Property(e => e.Userlogin).HasColumnName("userlogin");
            entity.Property(e => e.Userpassword).HasColumnName("userpassword");

            entity.HasOne(d => d.InnNavigation).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.Inn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_inn_fkey");
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.HasKey(e => e.Workid).HasName("works_pkey");

            entity.ToTable("works");

            entity.HasIndex(e => e.Workname, "works_workname_key").IsUnique();

            entity.Property(e => e.Workid)
                .ValueGeneratedNever()
                .HasColumnName("workid");
            entity.Property(e => e.Workname).HasColumnName("workname");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Inn).HasName("workers_pkey");

            entity.ToTable("workers");

            entity.Property(e => e.Inn).HasColumnName("inn");
            entity.Property(e => e.Hotelid)
                .ValueGeneratedOnAdd()
                .HasColumnName("hotelid");
            entity.Property(e => e.Imagename)
                .HasDefaultValueSql("'user_icon.png'::text")
                .HasColumnName("imagename");
            entity.Property(e => e.Workid).HasColumnName("workid");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Workers)
                .HasForeignKey(d => d.Hotelid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("workers_hotelid_fkey");

            entity.HasOne(d => d.InnNavigation).WithOne(p => p.Worker)
                .HasForeignKey<Worker>(d => d.Inn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("workers_inn_fkey");

            entity.HasOne(d => d.Work).WithMany(p => p.Workers)
                .HasForeignKey(d => d.Workid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("workers_workid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
