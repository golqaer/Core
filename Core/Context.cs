using System.Linq.Expressions;
using Database.Abstracts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }


        public Context(DbContextOptions<Context> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
