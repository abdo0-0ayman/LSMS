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
                if (!context.Courses.Any())
                {
                    context.Courses.AddRange(new List<Course>()
                    {
                        new Course()
                        {
                            Id="CS-1-1",
                            Name="Discrete Mathematics",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="CS-1-2",
                            Name="Programming Fundamentals",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="CS-1-3",
                            Name="Object Oriented Programming",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="CS-2-1",
                            Name="Probability And Statistics",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="CS-2-2",
                            Name="Visual Programming",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="CS-2-3",
                            Name="Software Engineering",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IT-1-1",
                            Name="Data Communication",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IT-1-2",
                            Name="Data Communication",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IT-1-3",
                            Name="Network Fundamentals",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IT-2-1",
                            Name="Data Communication",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IT-2-2",
                            Name="Network Fundamentals",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IT-2-3",
                            Name="Network Fundamentals",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IS-1-1",
                            Name="Information System",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IS-1-2",
                            Name="System Analysis And Design",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IS-1-3",
                            Name="Information System",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IS-2-1",
                            Name="System Analysis And Design",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IS-2-2",
                            Name="Information System",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="IS-2-3",
                            Name="System Analysis And Design",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="MM-1-1",
                            Name="Information System",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="MM-1-2",
                            Name="System Analysis And Design",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="MM-1-3",
                            Name="Information System",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="MM-2-1",
                            Name="System Analysis And Design",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="MM-2-2",
                            Name="Information System",
                            Hours=3,
                        },
                        new Course()
                        {
                            Id="MM-2-3",
                            Name="System Analysis And Design",
                            Hours=3,
                        },
                        //new Course()
                        //{
                        //    Id="CS-3-1",
                        //    Name="Cloud Computing",
                        //    Hours=3,
                        //},
                        //new Course()
                        //{
                        //    Id="CS-3-2",
                        //    Name="Machine Learning",
                        //    Hours=3,
                        //},
                        //new Course()
                        //{
                        //    Id="CS-3-3",
                        //    Name="Data Structure And Algorithms",
                        //    Hours=3,
                        //}
                    });
                    context.SaveChanges();
                }
                if (!context.Departments.Any())
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
                        ,
                        new Department()
                        {
                            Id="MM",
                            Name="Multi Media",
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
                    });
                    context.SaveChanges();
				}
                if (!context.Lectures.Any()&&context.Professors.Any())
                {
                    context.Lectures.AddRange(new List<Lecture>()
                    {
                        new Lecture()
                        {
                            Id="101",
                            CourseId="CS-1-1"
                            ,ProfessorSSN="10000000000001"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="102",
                            CourseId="CS-1-2"
                            ,ProfessorSSN="10000000000001"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="103",
                            CourseId="CS-1-3"
                            ,ProfessorSSN="10000000000001"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="104",
                            CourseId="CS-2-1"
                            ,ProfessorSSN="10000000000002"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="105",
                            CourseId="CS-2-2"
                            ,ProfessorSSN="10000000000002"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="106",
                            CourseId="CS-2-3"
                            ,ProfessorSSN="10000000000002"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="107",
                            CourseId="IS-1-1"
                            ,ProfessorSSN="10000000000003"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="108",
                            CourseId="IS-1-2"
                            ,ProfessorSSN="10000000000003"
                            ,lectureNum=-1

                        },new Lecture()
                        {

                            Id="109",
                            CourseId="IS-1-3"
                            ,ProfessorSSN="10000000000003"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="110",
                            CourseId="IS-2-1"
                            ,ProfessorSSN="10000000000004"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="111",
                            CourseId="IS-2-2"
                            ,ProfessorSSN="10000000000004"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="112",
                            CourseId="IS-2-3"
                            ,ProfessorSSN="10000000000004"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="113",
                            CourseId="IT-1-1"
                            ,ProfessorSSN="10000000000005"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="114",
                            CourseId="IT-1-2"
                            ,ProfessorSSN="10000000000005"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="115",
                            CourseId="IT-1-3"
                            ,ProfessorSSN="10000000000005"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="116",
                            CourseId="IT-2-1"
                            ,ProfessorSSN="10000000000006"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="117",
                            CourseId="IT-2-2"
                            ,ProfessorSSN="10000000000006"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="118",
                            CourseId="IT-2-3"
                            ,ProfessorSSN="10000000000006"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="119",
                            CourseId="MM-1-1"
                            ,ProfessorSSN="10000000000007"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="120",
                            CourseId="MM-1-2"
                            ,ProfessorSSN="10000000000007"
                            ,lectureNum=-1

                        },new Lecture()
                        {

                            Id="121",
                            CourseId="MM-1-3"
                            ,ProfessorSSN="10000000000007"
                            ,lectureNum=-1

                        },new Lecture()
                        {
                            Id="122",
                            CourseId="MM-2-1"
                            ,ProfessorSSN="10000000000008"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="123",
                            CourseId="MM-2-2"
                            ,ProfessorSSN="10000000000008"
                            ,lectureNum=-1

                        },
                        new Lecture()
                        {
                            Id="124",
                            CourseId="MM-2-3"
                            ,ProfessorSSN="10000000000008"
                            ,lectureNum=-1

                        },
                    }) ;
                    context.SaveChanges();
                }
                //var professors= context.Professors.ToList();
                //var courses =context.Courses.ToList();
                //var cp=new List<CourseProfessor>();

                //foreach(var item in professors)
                //{
                //    foreach (var course in courses)
                //    {
                //        cp.Add(new CourseProfessor()
                //        {
                //            CourseId = course.Id,
                //            ProfessorId=item.Id,
                //            Course=course,
                //            Professor=item
                //        });
                //    }
                //}
                //context.CourseProfessors.AddRange(cp);
                //context.SaveChanges();
            }
		}
    }
}
