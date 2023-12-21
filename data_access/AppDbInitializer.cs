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
                            id="CS-1-1",
                            name="Discrete Mathematics",
                            hours=3,
                        },
                        new Course()
                        {
                            id="CS-1-2",
                            name="Programming Fundamentals",
                            hours=3,
                        },
                        new Course()
                        {
                            id="CS-1-3",
                            name="Object Oriented Programming",
                            hours=3,
                        },
                        new Course()
                        {
                            id="CS-2-1",
                            name="Probability And Statistics",
                            hours=3,
                        },
                        new Course()
                        {
                            id="CS-2-2",
                            name="Visual Programming",
                            hours=3,
                        },
                        new Course()
                        {
                            id="CS-2-3",
                            name="Software Engineering",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IT-1-1",
                            name="Data Communication",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IT-1-2",
                            name="Data Communication",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IT-1-3",
                            name="Network Fundamentals",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IT-2-1",
                            name="Data Communication",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IT-2-2",
                            name="Network Fundamentals",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IT-2-3",
                            name="Network Fundamentals",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IS-1-1",
                            name="Information System",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IS-1-2",
                            name="System Analysis And Design",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IS-1-3",
                            name="Information System",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IS-2-1",
                            name="System Analysis And Design",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IS-2-2",
                            name="Information System",
                            hours=3,
                        },
                        new Course()
                        {
                            id="IS-2-3",
                            name="System Analysis And Design",
                            hours=3,
                        },
                        new Course()
                        {
                            id="MM-1-1",
                            name="Information System",
                            hours=3,
                        },
                        new Course()
                        {
                            id="MM-1-2",
                            name="System Analysis And Design",
                            hours=3,
                        },
                        new Course()
                        {
                            id="MM-1-3",
                            name="Information System",
                            hours=3,
                        },
                        new Course()
                        {
                            id="MM-2-1",
                            name="System Analysis And Design",
                            hours=3,
                        },
                        new Course()
                        {
                            id="MM-2-2",
                            name="Information System",
                            hours=3,
                        },
                        new Course()
                        {
                            id="MM-2-3",
                            name="System Analysis And Design",
                            hours=3,
                        },
                        //new Course()
                        //{
                        //    id="CS-3-1",
                        //    name="Cloud Computing",
                        //    hours=3,
                        //},
                        //new Course()
                        //{
                        //    id="CS-3-2",
                        //    name="Machine Learning",
                        //    hours=3,
                        //},
                        //new Course()
                        //{
                        //    id="CS-3-3",
                        //    name="Data Structure And Algorithms",
                        //    hours=3,
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
                            id="CS",
                            name="Computer Science",
                        }
                        ,
						new Department()
						{
                            id="IS",
							name="Information System",
						}
                        ,
						new Department()
						{
                            id="IT",
							name="Information Technology",
						}
                        ,
                        new Department()
                        {
                            id="MM",
                            name="Multi Media",
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
                            name="Abdo Hany",
                            userName="abdohany",
                            password="00000000" // 8 zeros
                        },
                        new Admin()
                        {
                            name="Abdo Ayman",
                            userName="abdoayman",
                            password="00000000"
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
                            userName="abdohany",
                            password="00000000", // 8 zeros
                            role="Admins"
                        },
                        new User()
                        {
                            userName="abdoayman",
                            password="00000000",
					        role="Admins"
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
                            id="101",
                            courseId="CS-1-1"
                            ,professorSSN="10000000000001"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="102",
                            courseId="CS-1-2"
                            ,professorSSN="10000000000001"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="103",
                            courseId="CS-1-3"
                            ,professorSSN="10000000000001"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="104",
                            courseId="CS-2-1"
                            ,professorSSN="10000000000002"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="105",
                            courseId="CS-2-2"
                            ,professorSSN="10000000000002"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="106",
                            courseId="CS-2-3"
                            ,professorSSN="10000000000002"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="107",
                            courseId="IS-1-1"
                            ,professorSSN="10000000000003"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="108",
                            courseId="IS-1-2"
                            ,professorSSN="10000000000003"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {

                            id="109",
                            courseId="IS-1-3"
                            ,professorSSN="10000000000003"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="110",
                            courseId="IS-2-1"
                            ,professorSSN="10000000000004"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="111",
                            courseId="IS-2-2"
                            ,professorSSN="10000000000004"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="112",
                            courseId="IS-2-3"
                            ,professorSSN="10000000000004"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="113",
                            courseId="IT-1-1"
                            ,professorSSN="10000000000005"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="114",
                            courseId="IT-1-2"
                            ,professorSSN="10000000000005"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="115",
                            courseId="IT-1-3"
                            ,professorSSN="10000000000005"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="116",
                            courseId="IT-2-1"
                            ,professorSSN="10000000000006"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="117",
                            courseId="IT-2-2"
                            ,professorSSN="10000000000006"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="118",
                            courseId="IT-2-3"
                            ,professorSSN="10000000000006"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="119",
                            courseId="MM-1-1"
                            ,professorSSN="10000000000007"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="120",
                            courseId="MM-1-2"
                            ,professorSSN="10000000000007"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {

                            id="121",
                            courseId="MM-1-3"
                            ,professorSSN="10000000000007"
                            ,lectureNum=-1,hallId=null

                        },new Lecture()
                        {
                            id="122",
                            courseId="MM-2-1"
                            ,professorSSN="10000000000008"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="123",
                            courseId="MM-2-2"
                            ,professorSSN="10000000000008"
                            ,lectureNum=-1,hallId=null

                        },
                        new Lecture()
                        {
                            id="124",
                            courseId="MM-2-3"
                            ,professorSSN="10000000000008"
                            ,lectureNum=-1,hallId=null

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
                //            courseId = course.id,
                //            Professorid=item.id,
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
