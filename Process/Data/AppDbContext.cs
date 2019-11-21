using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Process.Models.Diet;
using Process.Models.Pocket;
using Process.Models.Book;
using Process.Helpers;
using Process.Models.Notebook;
using Process.Models.Workout;
using Process.Models.ToDo;
using Process.Models.Diary;
using Process.Models.AppSetting;
using Process.Models.Calendar;

namespace Process.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.Migrate();
            if (!Database.EnsureCreated()) return;
        }

        #region Diary

        public DbSet<DiaryLog> DiaryLogs { get; set; }
        public DbSet<DiaryHistoryLog> DiaryHistoryLogs { get; set; }
        public DbSet<DiaryLogFeeling> DiaryLogFeelings { get; set; }
        public DbSet<DiaryLogRate> DiaryLogRates { get; set; }
        public DbSet<DiaryBlackWord> DiaryBlackWords { get; set; }

        #endregion

        #region ToDo

        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }

        #endregion

        #region User

        //public DbSet<User> Users { get; set; }
        //public DbSet<Session> Sessions { get; set; }
        //public DbSet<LoginLog> LoginLogs { get; set; }

        #endregion

        #region Diet

        public DbSet<DietFood> DietFoods { get; set; }
        public DbSet<DietNutrientAndEnergyValue> DietNutrientAndEnergyValues { get; set; }
        public DbSet<DietVitaminValue> DietVitaminValues { get; set; }
        public DbSet<DietMineralValue> DietMineralValues { get; set; }
        public DbSet<DietNutrientTypeValue> DietNutrientTypeValues { get; set; }
        public DbSet<DietLastSelectedFood> DietLastSelectedFoods { get; set; }
        public DbSet<DietLog> DietLogs { get; set; }
        public DbSet<DietLogKeyword> DietLogKeywords { get; set; }

        #endregion

        #region Workout

        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutTarget> WorkoutTargets { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutDay> WorkoutDays { get; set; }
        public DbSet<WorkoutMeasurement> WorkoutMeasurements { get; set; }
        public DbSet<WorkoutResult> WorkoutResults { get; set; }

        #endregion

        #region PocketBank

        public DbSet<PocketInOut> PocketInOuts { get; set; }
        public DbSet<PocketBillCategory> PocketBillCategories { get; set; }
        public DbSet<PocketTarget> PocketTargets { get; set; }
        public DbSet<PocketAction> PocketActions { get; set; }
        public DbSet<PocketBill> PocketBills { get; set; }
        public DbSet<PocketCategory> PocketCategories { get; set; }


        #endregion

        #region Notebook

        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<NotebookLog> NotebookLogs { get; set; }

        #endregion

        #region BookLog

        public DbSet<BookLogAuthor> BookLogAuthors { get; set; }
        public DbSet<BookLogBook> BookLogBooks { get; set; }
        public DbSet<BookLogReview> BookLogReviews { get; set; }
        public DbSet<BookLogGenre> BookLogGenres { get; set; }
        public DbSet<BookLogBookGenre> BookLogBookGenres { get; set; }

        #endregion

        #region AppSettings

        public DbSet<AppSetting> AppSettings { get; set; }

        #endregion

        #region Calendar

        public DbSet<CalendarLog> CalendarLogs { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            SQLitePCL.Batteries_V2.Init();
            optionsBuilder.UseSqlite("Data Source=Process.db;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Diary

            // DiaryLog
            modelBuilder.Entity<DiaryLog>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Date).IsUnique();
                entity.Property(x => x.Date)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.UpdateDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            // DiaryHistoryLog
            modelBuilder.Entity<DiaryHistoryLog>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            // DiaryLogFeeling
            modelBuilder.Entity<DiaryLogFeeling>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            // DiaryLogRate
            modelBuilder.Entity<DiaryLogRate>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            // DiaryBlackWord
            modelBuilder.Entity<DiaryBlackWord>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Word).IsUnique();
            });

            #endregion

            #region Others

            //// User
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //    entity.Property(x => x.Username).HasMaxLength(45);
            //    entity.HasIndex(x => x.Username).IsUnique();
            //});

            //// Session
            //modelBuilder.Entity<Session>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //});

            //// LoginLog
            //modelBuilder.Entity<LoginLog>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //});

            //// Activity
            //modelBuilder.Entity<Activity>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //    entity.HasIndex(x => new
            //    {
            //        x.UserId,
            //        x.ActivityName
            //    }).IsUnique();
            //});

            //// ActivityLog
            //modelBuilder.Entity<ActivityLog>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //});

            #endregion

            #region ToDo

            // ToDo
            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.UpdateDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));

            });

            // ToDoList
            modelBuilder.Entity<ToDoList>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.LastUpdateDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            #endregion

            #region Workout

            modelBuilder.Entity<Workout>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.TargetBodyPart).HasMaxLength(10)
                    .HasConversion(c => c.ToString(),
                        c => (BodyPart)Enum.Parse(typeof(BodyPart), c))
                    .IsUnicode(false);
                entity.HasIndex(x => x.WorkoutTitle).IsUnique();
            });

            modelBuilder.Entity<WorkoutPlan>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.ExpireDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.IsCompleted)
                    .HasConversion(c => Convert.ToInt32(c),
                        c => Convert.ToBoolean(c));
            });

            modelBuilder.Entity<WorkoutDay>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Date)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.IsCompleted)
                    .HasConversion(c => Convert.ToInt32(c),
                        c => Convert.ToBoolean(c));
                entity.Property(x => x.IsBreak)
                    .HasConversion(c => Convert.ToInt32(c),
                        c => Convert.ToBoolean(c));
            });

            modelBuilder.Entity<WorkoutTarget>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            modelBuilder.Entity<WorkoutLog>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.IsCompleted)
                    .HasConversion(c => Convert.ToInt32(c),
                        c => Convert.ToBoolean(c));
            });

            modelBuilder.Entity<WorkoutResult>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.Weight1)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c));
                entity.Property(x => x.Weight2)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c));
                entity.Property(x => x.Weight3)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c));
            });

            modelBuilder.Entity<WorkoutMeasurement>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.BodyPart).HasMaxLength(10)
                    .HasConversion(c => c.ToString(),
                        c => (BodyPart)Enum.Parse(typeof(BodyPart), c))
                    .IsUnicode(false);
                entity.Property(x => x.Measurement)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c));
            });

            #endregion

            #region PocketBank

            modelBuilder.Entity<PocketInOut>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Type).HasMaxLength(10)
                    .HasConversion(c => c.ToString(),
                        c => (PocketInOutType)Enum.Parse(typeof(PocketInOutType), c))
                    .IsUnicode(false);
                entity.Property(x => x.Amount)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => double.Parse(c, CultureInfo.InvariantCulture));
            });

            modelBuilder.Entity<PocketBillCategory>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<PocketTarget>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<PocketAction>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<PocketBill>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<PocketCategory>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Priority).HasMaxLength(10)
                    .HasConversion(c => c.ToString(),
                        c => (PocketCategoryPriority)Enum.Parse(typeof(PocketCategoryPriority), c))
                    .IsUnicode(false);
            });

            #endregion

            #region Diet

            modelBuilder.Entity<DietFood>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.Image)
                    .HasConversion(c => c.ImageToByteArray(),
                        c => c.ByteArrayToBitmapImage());
                entity.HasIndex(x => x.Title).IsUnique();
            });

            modelBuilder.Entity<DietNutrientAndEnergyValue>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.Calorie)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
                entity.Property(x => x.CalorieDailyVal)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
                entity.HasIndex(x => x.DietFoodId).IsUnique();
            });

            modelBuilder.Entity<DietMineralValue>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.Mineral)
                    .HasConversion(c => c.ToString(),
                        c => (Mineral)Enum.Parse(typeof(Mineral), c))
                    .IsUnicode(false);
                entity.Property(x => x.Unit)
                    .HasConversion(c => c.ToString(),
                        c => (Unit)Enum.Parse(typeof(Unit), c))
                    .IsUnicode(false);
                entity.Property(x => x.Value)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
                entity.Property(x => x.DailyValue)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
                //entity.HasIndex(x => new
                //{
                //    x.DietNutrientAndEnergyValueId,
                //    x.Mineral
                //}).IsUnique();
            });

            modelBuilder.Entity<DietVitaminValue>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.Vitamin)
                    .HasConversion(c => c.ToString(),
                        c => (Vitamin)Enum.Parse(typeof(Vitamin), c))
                    .IsUnicode(false);
                entity.Property(x => x.Unit)
                    .HasConversion(c => c.ToString(),
                        c => (Unit)Enum.Parse(typeof(Unit), c))
                    .IsUnicode(false);
                entity.Property(x => x.Value)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
                entity.Property(x => x.DailyValue)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
                //entity.HasIndex(x => new
                //{
                //    x.DietNutrientAndEnergyValueId,
                //    x.Vitamin
                //}).IsUnique();
            });

            modelBuilder.Entity<DietNutrientTypeValue>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.NutrientType)
                    .HasConversion(c => c.ToString(),
                        c => (NutrientType)Enum.Parse(typeof(NutrientType), c))
                    .IsUnicode(false);
                entity.Property(x => x.Unit)
                    .HasConversion(c => c.ToString(),
                        c => (Unit)Enum.Parse(typeof(Unit), c))
                    .IsUnicode(false);
                entity.Property(x => x.Value)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
                entity.Property(x => x.DailyValue)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
                //entity.HasIndex(x => new
                //{
                //    x.DietNutrientAndEnergyValueId,
                //    x.NutrientType
                //}).IsUnique();
            });

            modelBuilder.Entity<DietLastSelectedFood>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            modelBuilder.Entity<DietLog>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.Amount)
                    .HasConversion(c => c.ToString(CultureInfo.InvariantCulture),
                        c => Convert.ToDouble(c, CultureInfo.InvariantCulture));
            });

            modelBuilder.Entity<DietLogKeyword>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            #endregion

            #region Notebook

            modelBuilder.Entity<Notebook>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            modelBuilder.Entity<NotebookLog>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            #endregion

            #region BookLog

            modelBuilder.Entity<BookLogBook>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.Image)
                    .HasConversion(c => c.ImageToByteArray(),
                        c => c.ByteArrayToBitmapImage());
            });

            modelBuilder.Entity<BookLogAuthor>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.Image)
                    .HasConversion(c => c.ImageToByteArray(),
                        c => c.ByteArrayToBitmapImage());
            });

            modelBuilder.Entity<BookLogReview>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));

            });

            modelBuilder.Entity<BookLogGenre>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));

            });

            modelBuilder.Entity<BookLogBookGenre>(entity =>
            {
                entity.HasKey(x => x.Id);
            });
            #endregion

            #region AppSetting

            modelBuilder.Entity<AppSetting>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}