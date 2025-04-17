using System.Linq.Expressions;
using Database.Abstracts;
using Database.Entities;
using DTO.AppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace Database
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }

        private readonly SystemUserSettings _sysUser;

        public Context(DbContextOptions<Context> options, IOptions<SystemUserSettings> sysUser) : base(options)
        {
            _sysUser = sysUser.Value;
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public Context(DbContextOptions options, IOptions<SystemUserSettings> sysUser) : base(options)
        {
            _sysUser = sysUser.Value;
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType) && !t.ClrType.IsAbstract)
                .ToList()
                .ForEach(t =>
                {
                    modelBuilder.Entity(t.ClrType)
                        .Property(nameof(BaseEntity.IsDeleted))
                        .HasDefaultValue(false);

                    var parameter = Expression.Parameter(t.ClrType, "e");
                    var filter = Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                            Expression.Constant(false)
                        ),
                        parameter
                    );
                    modelBuilder.Entity(t.ClrType).HasQueryFilter(filter);

                    var entity = modelBuilder.Entity(t.ClrType);
                    var indexes = entity.Metadata.GetIndexes();

                    var indexesToRemove = new List<IMutableIndex>();
                    var indexesToAdd = new List<(string[] props, string name, bool isUnique)>();

                    foreach (var index in indexes)
                    {
                        if (!index.Properties.Any(p => p.Name == nameof(BaseEntity.IsDeleted)))
                        {
                            var properties = index.Properties.Select(p => p.Name).ToList();
                            properties.Add(nameof(BaseEntity.IsDeleted));

                            indexesToRemove.Add(index);
                            indexesToAdd.Add((properties.ToArray(), index.Name!, index.IsUnique));
                        }
                    }

                    indexesToRemove.ForEach(ind => entity.Metadata.RemoveIndex(ind));
                    indexesToAdd.ForEach(ind => entity.HasIndex(ind.props).HasDatabaseName(ind.name).IsUnique(ind.isUnique));
                })
                ;

            modelBuilder.Entity<User>().Property(u => u.Admin).HasDefaultValue(false);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = _sysUser.UserId,
                    LastUpdateTick = 0,
                    UserName = _sysUser.UserName,
                    Password = _sysUser.Password,
                    Admin = _sysUser.IsAdmin,
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
