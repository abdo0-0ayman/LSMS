using LSMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LSMS.data_access
{
    public class AppDbInitializer
    {
        public static void seed(IApplicationBuilder app)
        {
            using(var serviceScope=app.ApplicationServices.CreateScope())
            {
                var context=serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                if(!context.Departments.Any())
                {
                    context.Departments.AddRange(new List<Department>()
                    {
                        new Department()
                        {
                            Id="CS",
                            Name="Computer Science",
                        }
                        ,
						new Department()
						{
                            Id="IS",
							Name="Information System",
						}
                        ,
						new Department()
						{
                            Id="IT",
							Name="Information Technology",
						}
					});
                    context.SaveChanges();
                }
				var dep1 = context.Departments.FirstOrDefault(u => u.Name == "Computer Science");
				var dep2 = context.Departments.FirstOrDefault(u => u.Name == "Information System");
				var dep3 = context.Departments.FirstOrDefault(u => u.Name == "Information Technology");
				if (!context.Professors.Any())
				{

					context.Professors.AddRange(new List<Professor>()
					{
						new Professor()
						{
							Name="Ahmed Hosny",
							SSN="30310152501532",
							PhoneNumber="0",
							Password="Asd159753",
                            DepartmentId=dep1.Id
						},
						new Professor()
						{
							Name="Ahmed mohamed ",
							SSN="30310162501632",
							PhoneNumber="0",
							Password="aSd159753",
							DepartmentId=dep2.Id

						},
						new Professor()
						{
							Name="Ahmed abdelrahman",
							SSN="30310172501732",
							PhoneNumber="0",
							Password="asD159753",
                            DepartmentId=dep3.Id

						}
					});

					context.SaveChanges();
				}
				if (!context.Students.Any())
                {
                    context.Students.AddRange(new List<Student>()
                    {
                        new Student()
                        {
                            Name="Abdelrahman Ayman",
                            SSN="30310152501531",
                            PhoneNumber="0",
                            Password="Asd159753",
                            AcademicEmail="abdulrahman.ayman632@compit.aun.edu.eg",
							DepartmentId=dep1.Id
						},
                        new Student()
                        {
                            Name="Abdelrahman Hany",
                            SSN="30310162501631",
                            PhoneNumber="0",
                            Password="aSd159753",
                            AcademicEmail="abdulrahman.ayman633@compit.aun.edu.eg",
							DepartmentId=dep2.Id
						},
                        new Student()
                        {
                            Name="Abdelrahman Saad",
                            SSN="30310172501731",
                            PhoneNumber="0",
                            Password="asD159753",
                            AcademicEmail="abdulrahman.ayman634@compit.aun.edu.eg",
							DepartmentId=dep3.Id
						}
                    });
                    context.SaveChanges();

                }
                if (!context.Admins.Any())
                {
                    context.Admins.AddRange(new List<Admin>()
                    {
                        new Admin()
                        {
                            Name="Abdo Hany",
                            UserName="abdohany",
                            Password="00000000" // 8 zeros
                        },
                        new Admin()
                        {
                            Name="Abdo Ayman",
                            UserName="abdoayman",
                            Password="00000000"
                        }
                    });
                    context.SaveChanges();

                }
				if (!context.Users.Any())
				{
                    context.Users.AddRange(new List<User>()
                    {
                        new User()
                        {
                            Username="abdohany",
                            Password="00000000", // 8 zeros
                            Role="Admins"
                        },
                        new User()
                        {
                            Username="abdoayman",
                            Password="00000000",
					        Role="Admins"
						},

                        new User()
                        {
                            Username="30310152501532",
                            Password="Asd159753",
                            Role="Professors",
                        },
                        new User()
                        {
                            Username="30310162501632",
                            Password="aSd159753",
						    Role="Professors",

						},
                        new User()
                        {
                            Username="30310172501732",
                            Password="asD159753",
							Role="Professors",
						},
                        new User()
                        {
                            Username="30310152501531",
                            Password="Asd159753",
							Role="Students",
						},
                        new User()
                        {
                            Username="30310162501631",
                            Password="aSd159753",
							Role="Students",
						},
                        new User()
                        {
                            Username="30310172501731",
                            Password="asD159753",
							Role="Students",
						},
                    });
                    context.SaveChanges();
				}
			}
		}
    }
}
