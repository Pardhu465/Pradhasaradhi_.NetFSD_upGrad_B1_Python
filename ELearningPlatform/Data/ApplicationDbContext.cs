
using System;
using Microsoft.EntityFrameworkCore;
using ELearningPlatform.Entities;

namespace ELearningPlatform.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // ── DbSets ──────────────────────────────────────────────────────────
    public DbSet<User>     Users     { get; set; }
    public DbSet<Course>   Courses   { get; set; }
    public DbSet<Lesson>   Lessons   { get; set; }
    public DbSet<Quiz>     Quizzes   { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Result>   Results   { get; set; }
    public DbSet<CourseCompletion> CourseCompletions { get; set; }

    // ── Fluent API Configuration ─────────────────────────────────────────
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ──────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(150);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // ── Course ────────────────────────────────────────────────────────
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(c => c.CourseId);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Description).HasMaxLength(1000);
            entity.Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // One User → Many Courses
            entity.HasOne(c => c.Creator)
                  .WithMany(u => u.Courses)
                  .HasForeignKey(c => c.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Lesson ────────────────────────────────────────────────────────
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(l => l.LessonId);
            entity.Property(l => l.Title).IsRequired().HasMaxLength(200);
            entity.Property(l => l.OrderIndex).HasDefaultValue(1);

            // One Course → Many Lessons
            entity.HasOne(l => l.Course)
                  .WithMany(c => c.Lessons)
                  .HasForeignKey(l => l.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Quiz ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(q => q.QuizId);
            entity.Property(q => q.Title).IsRequired().HasMaxLength(200);

            // One Course → Many Quizzes
            entity.HasOne(q => q.Course)
                  .WithMany(c => c.Quizzes)
                  .HasForeignKey(q => q.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Question ──────────────────────────────────────────────────────
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(q => q.QuestionId);
            entity.Property(q => q.QuestionText).IsRequired().HasMaxLength(500);
            entity.Property(q => q.CorrectAnswer).IsRequired().HasMaxLength(1);

            // One Quiz → Many Questions
            entity.HasOne(q => q.Quiz)
                  .WithMany(q => q.Questions)
                  .HasForeignKey(q => q.QuizId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Result ────────────────────────────────────────────────────────
        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(r => r.ResultId);
            entity.Property(r => r.AttemptDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // One User → Many Results
            entity.HasOne(r => r.User)
                  .WithMany(u => u.Results)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // One Quiz → Many Results
            entity.HasOne(r => r.Quiz)
                  .WithMany(q => q.Results)
                  .HasForeignKey(r => r.QuizId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}