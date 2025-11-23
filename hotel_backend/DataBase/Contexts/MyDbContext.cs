using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using hotel_backend.DataBase.Entities;

namespace hotel_backend.DataBase.Contexts;

public partial class MyDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MyDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }


    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("MyDbContext"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookings_pkey");

            entity.ToTable("bookings");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");
            entity.Property(e => e.CheckOutDate).HasColumnName("check_out_date");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("bookings_room_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("bookings_user_id_fkey");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hotels_pkey");

            entity.ToTable("hotels");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.ImgUrl)
                .HasDefaultValueSql("'ImgHotel/default.png'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("img_url");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LevelAccess).HasColumnName("level_access");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rooms_pkey");

            entity.ToTable("rooms");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.ImgUrl)
                .HasDefaultValueSql("'ImgRoom/default.png'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("img_url");
            entity.Property(e => e.PricePerNight).HasColumnName("price_per_night");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.RoomType)
                .HasDefaultValue(0)
                .HasColumnName("room_type");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("rooms_hotel_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.ImgUrl)
                .HasDefaultValueSql("'ImgProfile/default.png'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("img_url");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
