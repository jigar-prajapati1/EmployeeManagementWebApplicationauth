//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace EmployeeMangementWebApi.DbContext
//{
//    public class EmployeeAuthDbContext:IdentityDbContext
//    {
//        public EmployeeAuthDbContext(DbContextOptions<EmployeeAuthDbContext> options) : base(options)
//        {
//        }
//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            var readerRoleId="e4a1495e - b68f - 4db1 - 8d72 - 559198a7075e";
//            var writerRoleId = "c0b20d8d-655e-4223-8ea1-51db6d148f70";
//            var roles = new List<IdentityRole>
//            {
//                new IdentityRole
//                {
//                    Id=readerRoleId,
//                    ConcurrencyStamp=readerRoleId,
//                    Name="Reader",
//                    NormalizedName="Reader".ToUpper()
//                },
//                new IdentityRole
//                {
//                    Id=writerRoleId,
//                    ConcurrencyStamp=writerRoleId,
//                    Name="writer",
//                    NormalizedName="writer".ToUpper()
//                },
//            };
//            builder.Entity<IdentityRole>().HasData(roles);
//        }
//    }
//}
