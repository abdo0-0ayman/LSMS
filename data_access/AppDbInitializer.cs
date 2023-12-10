using LSMS.Models;

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
                if(!context.Professors.Any())
                {
                    context.Professors.AddRange(new List<Professor>()
                    {
                        new Professor()
                        {
                            Name="Ahmed Hosny",
                            SSN="30310152501532",
                            PhoneNumber="0",
                            Password="Asd159753"
                        },
                        new Professor()
                        {
                            Name="Ahmed mohamed ",
                            SSN="30310162501632",
                            PhoneNumber="0",
                            Password="aSd159753"
                        },
                        new Professor()
                        {
                            Name="Ahmed abdelrahman",
                            SSN="30310172501732",
                            PhoneNumber="0",
                            Password="asD159753"
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
                            SSN="30310152501532",
                            PhoneNumber="0",
                            Password="Asd159753",
                            AcademicEmail="abdulrahman.ayman632@compit.aun.edu.eg"
                        },
                        new Student()
                        {
                            Name="Abdelrahman Hany",
                            SSN="30310162501632",
                            PhoneNumber="0",
                            Password="aSd159753",
                            AcademicEmail="abdulrahman.ayman633@compit.aun.edu.eg"
                        },
                        new Student()
                        {
                            Name="Abdelrahman Saad",
                            SSN="30310172501732",
                            PhoneNumber="0",
                            Password="asD159753",
                            AcademicEmail="abdulrahman.ayman634@compit.aun.edu.eg"
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
                            Password="00000000" // 8 zeros
                        },
                        new Admin()
                        {
                            Name="Abdo Ayman",
                            Password="00000000"
                        }
                    });
                    context.SaveChanges();

                }
            }
        }
    }
}
