using Ef2;
using Ef2.Models;
using System.Linq;

bool exit = false;

while (!exit)
{
    Console.Clear();
    Console.WriteLine("...Main Menu...");
    Console.WriteLine("1 - Adding Operations");
    Console.WriteLine("2 - List Teachers");
    Console.WriteLine("3 - Assign Course to Student");
    Console.WriteLine("4 - List Students in a Course");
    Console.WriteLine("5 - List Courses of a Student");
    Console.WriteLine("6 - Add/Update Grade for Student in a Course");
    Console.WriteLine("7 - Show Transcript for Student");
    Console.WriteLine("0 - Exit");
    Console.Write("Choice : ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            {
                Console.Clear();
                Console.WriteLine("1 - Add Student");
                Console.WriteLine("2 - Add Teacher");
                Console.WriteLine("3 - Add Course");
                Console.WriteLine("0 - Back to Main Menu");
                Console.Write("Choice : ");
                var addChoice = Console.ReadLine();

                using (var context = new SchoolAppContext())
                {
                    switch (addChoice)
                    {
                        case "1":
                            Console.Write("Student Name: ");
                            var studentName = Console.ReadLine();
                            var student = new Student { StudentName = studentName };
                            context.Students.Add(student);
                            context.SaveChanges();
                            Console.WriteLine($"Student Added (ID: {student.StudentId})");
                            break;

                        case "2":
                            Console.Write("Teacher Name: ");
                            var teacherName = Console.ReadLine();
                            var teacher = new Teacher { TeacherName = teacherName };
                            context.Teachers.Add(teacher);
                            context.SaveChanges();
                            Console.WriteLine($"Teacher Added (ID: {teacher.TeacherId})");
                            break;

                        case "3":
                            var teachersWithCourses = context.Courses.Select(c => c.TeacherId).ToList();
                            var availableTeachers = context.Teachers
                                                          .Where(t => !teachersWithCourses.Contains(t.TeacherId))
                                                          .ToList();

                            if (availableTeachers.Count == 0)
                            {
                                Console.WriteLine("No available teachers for new courses. Please add a teacher first.");
                                break;
                            }

                            Console.WriteLine("Available Teachers:");
                            foreach (var t in availableTeachers)
                            {
                                Console.WriteLine($"{t.TeacherId} - {t.TeacherName}");
                            }

                            Console.Write("Course Name: ");
                            var courseTitle = Console.ReadLine();

                            Console.Write("Teacher ID: ");
                            if (int.TryParse(Console.ReadLine(), out int teacherId) &&
                                availableTeachers.Any(t => t.TeacherId == teacherId))
                            {
                                var course = new Course { CourseName = courseTitle, TeacherId = teacherId };
                                context.Courses.Add(course);
                                context.SaveChanges();
                                Console.WriteLine("Course Added");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Teacher ID");
                            }
                            break;

                        case "0":
                            break;
                    }
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                break;
            }

        case "2":
            {
                using (var context = new SchoolAppContext())
                {
                    var teachers = context.Teachers.ToList();

                    if (teachers.Count == 0)
                    {
                        Console.WriteLine("No teachers found.");
                    }
                    else
                    {
                        Console.WriteLine("List of Teachers:");
                        foreach (var teacher in teachers)
                        {
                            Console.WriteLine($"{teacher.TeacherId} - {teacher.TeacherName}");
                        }
                    }
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                break;
            }

        case "3":
            {
                using (var context = new SchoolAppContext())
                {
                    var students = context.Students.ToList();
                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                        break;
                    }
                    Console.WriteLine("Students:");
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.StudentId} - {student.StudentName}");
                    }

                    var courses = context.Courses.ToList();
                    if (courses.Count == 0)
                    {
                        Console.WriteLine("No courses found.");
                        break;
                    }
                    Console.WriteLine("Courses:");
                    foreach (var course in courses)
                    {
                        Console.WriteLine($"{course.CourseId} - {course.CourseName}");
                    }

                    Console.Write("Enter Student ID: ");
                    int studentId = int.Parse(Console.ReadLine());

                    Console.Write("Enter Course ID: ");
                    int courseId = int.Parse(Console.ReadLine());

                    var register = new Register
                    {
                        StudentId = studentId,
                        CourseId = courseId
                    };

                    context.Registers.Add(register);
                    context.SaveChanges();

                    Console.WriteLine("Student enrolled in the course.");
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                break;
            }

        case "4":
            {
                using (var context = new SchoolAppContext())
                {
                    var courses = context.Courses.ToList();
                    if (courses.Count == 0)
                    {
                        Console.WriteLine("No courses found.");
                        break;
                    }

                    Console.WriteLine("Courses:");
                    foreach (var course in courses)
                    {
                        Console.WriteLine($"{course.CourseId} - {course.CourseName}");
                    }

                    Console.Write("Enter Course ID to see enrolled students: ");
                    int courseId = int.Parse(Console.ReadLine());

                    var studentsInCourse = context.Registers
                                                  .Where(e => e.CourseId == courseId)
                                                  .Select(e => e.Student)
                                                  .ToList();

                    if (studentsInCourse.Count == 0)
                    {
                        Console.WriteLine("No students enrolled in this course.");
                    }
                    else
                    {
                        Console.WriteLine($"Students enrolled in {courseId}: {context.Courses.First(c => c.CourseId == courseId).CourseName}");
                        foreach (var student in studentsInCourse)
                        {
                            Console.WriteLine($"{student.StudentId} - {student.StudentName}");
                        }
                    }
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                break;
            }

        case "5":
            {
                using (var context = new SchoolAppContext())
                {
                    var students = context.Students.ToList();
                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                        break;
                    }

                    Console.WriteLine("Students:");
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.StudentId} - {student.StudentName}");
                    }

                    Console.Write("Enter Student ID to see enrolled courses: ");
                    int studentId = int.Parse(Console.ReadLine());

                    var coursesForStudent = context.Registers
                                                   .Where(r => r.StudentId == studentId)
                                                   .Select(r => r.Course)
                                                   .ToList();

                    if (coursesForStudent.Count == 0)
                    {
                        Console.WriteLine("This student is not enrolled in any courses.");
                    }
                    else
                    {
                        Console.WriteLine($"Courses enrolled by {context.Students.First(s => s.StudentId == studentId).StudentName}:");
                        foreach (var course in coursesForStudent)
                        {
                            Console.WriteLine($"{course.CourseId} - {course.CourseName}");
                        }
                    }
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                break;
            }

        case "6":
            {
                using (var context = new SchoolAppContext())
                {
                    var students = context.Students.ToList();
                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                        break;
                    }

                    Console.WriteLine("Students:");
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.StudentId} - {student.StudentName}");
                    }

                    Console.Write("Enter Student ID: ");
                    int studentId = int.Parse(Console.ReadLine());

                    var coursesForStudent = context.Registers
                                                   .Where(r => r.StudentId == studentId)
                                                   .Select(r => r.Course)
                                                   .ToList();

                    if (coursesForStudent.Count == 0)
                    {
                        Console.WriteLine("This student is not enrolled in any courses.");
                        break;
                    }

                    Console.WriteLine("Courses for this student:");
                    foreach (var course in coursesForStudent)
                    {
                        Console.WriteLine($"{course.CourseId} - {course.CourseName}");
                    }

                    Console.Write("Enter Course ID to add/update grade: ");
                    int courseId = int.Parse(Console.ReadLine());

                    var register = context.Registers
                                           .FirstOrDefault(r => r.StudentId == studentId && r.CourseId == courseId);

                    if (register == null)
                    {
                        Console.WriteLine("Student is not enrolled in this course.");
                        break;
                    }

                    Console.Write("Enter Grade (0-100): ");
                    if (double.TryParse(Console.ReadLine(), out double grade))
                    {
                        register.Grade = grade;
                        context.SaveChanges();
                        Console.WriteLine("Grade saved successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid grade input.");
                    }
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                break;
            }

        case "7":
            {
                using (var context = new SchoolAppContext())
                {
                    var students = context.Students.ToList();
                    if (students.Count == 0)
                    {
                        Console.WriteLine("No students found.");
                        break;
                    }

                    Console.WriteLine("Students:");
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.StudentId} - {student.StudentName}");
                    }

                    Console.Write("Enter Student ID to view transcript: ");
                    int studentId = int.Parse(Console.ReadLine());

                    var transcript = context.Registers
                                            .Where(r => r.StudentId == studentId)
                                            .Select(r => new //sadece istediğimiz alanları tutan geçici, küçük nesneler yaratmak için kullanılıyor.
                                            {
                                                r.Course.CourseName,
                                                r.Grade
                                            })
                                            .ToList();

                    if (transcript.Count == 0)
                    {
                        Console.WriteLine("No courses found for this student.");
                    }
                    else
                    {
                        Console.WriteLine($"Transcript for {context.Students.First(s => s.StudentId == studentId).StudentName}:");
                        foreach (var entry in transcript)
                        {
                            string gradeText = entry.Grade.HasValue ? entry.Grade.Value.ToString("0.00") : "Not graded yet";
                            Console.WriteLine($"{entry.CourseName} - {gradeText}");
                        }
                    }
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                break;
            }

        case "0":
            exit = true;
            break;
    }
}
