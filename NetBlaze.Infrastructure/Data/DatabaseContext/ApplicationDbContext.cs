using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.Infrastructure.Data.Configurations.MiscConfigurations;
using System.Reflection;

namespace NetBlaze.Infrastructure.Data.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<User,
                                                          Role,
                                                          long,
                                                          IdentityUserClaim<long>,
                                                          UserRole,
                                                          IdentityUserLogin<long>,
                                                          IdentityRoleClaim<long>,
                                                          IdentityUserToken<long>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<SampleEntity> Samples => Set<SampleEntity>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<UserDetails> UserDetails => Set<UserDetails>();
        public DbSet<Policy> Policies => Set<Policy>();
        public DbSet<EmployeeAttendance> EmployeeAttendances => Set<EmployeeAttendance>();
        public DbSet<AttendancePolicyAction> AttendancePolicyActions => Set<AttendancePolicyAction>();
        public DbSet<Vacation> Vacations => Set<Vacation>();
        public DbSet<RandomCheck> RandomChecks => Set<RandomCheck>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.ConfigureIdentityTablesNames();

            builder.SetGlobalIsDeletedFilterToAllEntities();
        }
    }
}