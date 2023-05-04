using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxisTest
{
	internal class Program
	{
		static void Main(string[] args)
		{
			using (var context = new AxisContext())
			{
				var axesCollection = context
					.AxisModels.SelectMany(x => x.AxesCollection
						.Select(axes => new AxesDto()
						{
							X = axes.X,
							Y = axes.Y,
							Z = axes.Z
						})
					)
					.AsNoTracking()
					.ToList();
			}
		}

		public class AxisContext : DbContext
		{
			protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			{
				if (!optionsBuilder.IsConfigured)
				{
					var connectionString = "Data Source=(localdb)\\LocalDb; Initial Catalog=AxesTest; user Id=phroute_user; Password=j[Uc7lwFoM&K";
					optionsBuilder.UseSqlServer(connectionString);
				}
			}

			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				modelBuilder.ApplyConfiguration(new AxisModelConfiguration());
			}

			public virtual DbSet<AxisModel> AxisModels { get; set;}
		}

		public class AxisModel
		{
			public int Id { get; set; }

			public ICollection<Axes> AxesCollection { get; set; }
		}

		public class Axes
		{
			[Column("x", TypeName = "decimal(10,8)")]
			public decimal? X { get; set; }

			[Column("y", TypeName = "decimal(10,8)")]
			public decimal? Y { get; set; }

			[Column("z", TypeName = "decimal(10,8)")]
			public decimal? Z { get; set; }
		}

		public class AxesDto
		{
			public decimal? X { get; set; }

			public decimal? Y { get; set; }

			public decimal? Z { get; set; }
		}

		public class AxisModelConfiguration : IEntityTypeConfiguration<AxisModel>
		{
			public void Configure(EntityTypeBuilder<AxisModel> builder)
			{
				builder.ToTable(nameof(AxisModel));
				builder.OwnsMany(a => a.AxesCollection, options =>
				{
					options.ToJson();
				});
			}
		}
	}
}