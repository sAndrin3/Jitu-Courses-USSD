using System;
using System.Collections.Generic;
using System.IO;

public class Authentication
{
    private void SimulateTopUp(UserDTO user, decimal amount)
{
    
    // Console.WriteLine($"Simulating top-up of {amount:C} for user {user.Username}");
}

private void SavePurchaseToAnalytics(UserDTO user, Course course)
{
    string purchaseData = $"{course.Name},{user.Username},{course.Price}";
    File.AppendAllText("Data/analytics.txt", purchaseData + Environment.NewLine);
}


    public void RunAuthentication()
    {
        Courses courses = new Courses();
        while (true)
        {
            Console.WriteLine("Welcome to The Jitu");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();

                    UserDTO authenticatedUser = UserLogin(username, password);

                    if (authenticatedUser != null)
                    {
                        Console.WriteLine($"Welcome, {authenticatedUser.Username}!");
                        if (authenticatedUser.Role == UserRole.Admin)
                        {
                            Console.WriteLine("Admin login successful");
                            RunAdminDashboard(authenticatedUser, courses);
                        }
                        else if (authenticatedUser.Role == UserRole.User)
                        {
                            Console.WriteLine("User login successful");
                            RunUserDashboard(authenticatedUser, courses);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Login failed");
                    }
                    break;

                case 2:
                    Console.WriteLine("Register as:");
                    Console.WriteLine("1. User");
                    Console.WriteLine("2. Admin");
                    Console.Write("Enter your choice: ");
                    int registerChoice = int.Parse(Console.ReadLine());

                    Console.Write("Enter a new username: ");
                    string newUsername = Console.ReadLine();
                    Console.Write("Enter a new password: ");
                    string newPassword = Console.ReadLine();

                    UserRole registerRole = (registerChoice == 2) ? UserRole.Admin : UserRole.User;

                    UserDTO newUser = new UserDTO
                    {
                        Username = newUsername,
                        Role = registerRole,
                        Password = newPassword
                    };

                    AddNewUser(newUser);
                    Console.WriteLine($"{registerRole} registration successful");
                    break;

                case 3:
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

            Console.WriteLine();
        }
    }

    public void AddNewUser(UserDTO user)
    {
        string userLine = $"{user.Username},{user.Role},{user.Password}";
        File.AppendAllText("Data/users.txt", userLine + Environment.NewLine);
    }

    public UserDTO UserLogin(string username, string password)
    {
        foreach (string line in File.ReadLines("Data/users.txt"))
        {
            string[] userFields = line.Split(',');
            if (userFields.Length == 3 && userFields[0] == username && userFields[2] == password)
            {
                return new UserDTO
                {
                    Username = userFields[0],
                    Role = Enum.Parse<UserRole>(userFields[1]),
                    Password = userFields[2]
                };
            }
        }
        return null;
    }
     public void RunAdminDashboard(UserDTO authenticatedUser, Courses courses)
    {
        while (true)
        {
            Console.WriteLine("Admin Dashboard:");
            Console.WriteLine("1. Add New Course");
            Console.WriteLine("2. View All Courses");
            Console.WriteLine("3. Update Course");
            Console.WriteLine("4. Delete Course");
            Console.WriteLine("5. View Analytics");
            Console.WriteLine("6. Logout");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddNewCourse(courses);
                    break;

                case 2:
                    DisplayCourses(courses.GetCourses());
                    break;

                case 3:
                    UpdateCourse(courses);
                    break;

                case 4:
                    DeleteCourse(courses);
                    break;

                case 5:
                    ViewAnalytics();
                    break;

                case 6:
                    Console.WriteLine("Logging out...");
                    return;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

            Console.WriteLine();
        }
    }

    public void RunUserDashboard(UserDTO user, Courses courses)
    {
        while (true)
        {
            Console.WriteLine($"Welcome, {user.Username}!");
            Console.WriteLine("User Dashboard:");
            Console.WriteLine("1. View All Courses");
            Console.WriteLine("2. Purchase Course");
            Console.WriteLine("3. Logout");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayCourses(courses.GetCourses());
                    break;

                case 2:
                    PurchaseCourse(user, courses);
                    break;

                case 3:
                    Console.WriteLine("Logging out...");
                    return;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

            Console.WriteLine();
        }
    }

    private void PurchaseCourse(UserDTO user, Courses courses)
    {
        Console.WriteLine("Available Courses:");
    List<Course> availableCourses = courses.GetCourses();

    if (availableCourses.Count == 0)
    {
        Console.WriteLine("No courses available for purchase.");
        return;
    }

    Console.WriteLine("Select a course to purchase:");
    for (int i = 0; i < availableCourses.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {availableCourses[i].Name} - {availableCourses[i].Price:C}");
    }

    Console.Write("Enter the number of the course you want to purchase: ");
    int courseNumber = int.Parse(Console.ReadLine());

    if (courseNumber >= 1 && courseNumber <= availableCourses.Count)
    {
        Course selectedCourse = availableCourses[courseNumber - 1];

       
        decimal topUpAmount = selectedCourse.Price;
        SimulateTopUp(user, topUpAmount);

        
        SavePurchaseToAnalytics(user, selectedCourse);

        Console.WriteLine($"Congratulations! You have purchased the course: {selectedCourse.Name} for {selectedCourse.Price:C}");
    }
    else
    {
        Console.WriteLine("Invalid course selection.");
    }
    }

    private void AddNewCourse(Courses courses)
    {
        Console.Write("Enter course name: ");
        string courseName = Console.ReadLine();
        Console.Write("Enter course price: ");
        decimal coursePrice = decimal.Parse(Console.ReadLine());

        Course newCourse = new Course
        {
            Name = courseName,
            Price = coursePrice
        };

        courses.AddCourse(newCourse);
        Console.WriteLine("Course added successfully");
    }

    private void DisplayCourses(List<Course> courses)
    {
        Console.WriteLine("Available Courses:");
        foreach (var course in courses)
        {
            Console.WriteLine($"{course.Name} - {course.Price:C}");
        }
    }

    private void UpdateCourse(Courses courses)
    {
        Console.Write("Enter the name of the course to update: ");
        string oldCourseName = Console.ReadLine();
        Console.Write("Enter new course name: ");
        string newCourseName = Console.ReadLine();
        Console.Write("Enter new course price: ");
        decimal newCoursePrice = decimal.Parse(Console.ReadLine());

        Course updatedCourse = new Course
        {
            Name = newCourseName,
            Price = newCoursePrice
        };

        courses.UpdateCourse(oldCourseName, updatedCourse);
        Console.WriteLine("Course updated successfully");
    }

    private void DeleteCourse(Courses courses)
    {
        Console.Write("Enter the name of the course to delete: ");
        string courseName = Console.ReadLine();

        courses.DeleteCourse(courseName);
        Console.WriteLine("Course deleted successfully");
    }

 public void ViewAnalytics()
{
    Console.WriteLine("Analytics:");
    Console.WriteLine("1. List of Courses and Students");
    Console.WriteLine("2. Back to Dashboard");
    Console.Write("Enter your choice: ");
    int choice = int.Parse(Console.ReadLine());

    switch (choice)
    {
        case 1:
            DisplayCourseAndStudentAnalytics();
            break;

        case 3:
            Console.WriteLine("Returning to Dashboard...");
            break;

        default:
            Console.WriteLine("Invalid choice");
            break;
    }
}

public void DisplayCourseAndStudentAnalytics()
{
    Console.WriteLine("List of Courses and Students with Purchased Courses:");
    List<string[]> analyticsData = ReadAnalyticsData();
    Dictionary<string, List<string>> studentCourses = new Dictionary<string, List<string>>();

    foreach (string[] fields in analyticsData)
    {
        if (fields.Length == 3)
        {
            string courseName = fields[0];
            string student = fields[1];

            if (!studentCourses.ContainsKey(student))
            {
                studentCourses[student] = new List<string>();
            }

            studentCourses[student].Add(courseName);
        }
    }

   foreach (var kvp in studentCourses)
    {
        string student = kvp.Key;
        List<string> purchasedCourses = kvp.Value;

        Console.WriteLine($"Student: {student}");
        Console.WriteLine("Courses Purchased:");
        foreach (string course in purchasedCourses)
        {
            Console.WriteLine($"- {course}");
        }
        Console.WriteLine(); 
    }
}


 private List<string[]> ReadAnalyticsData()
{
    List<string[]> analyticsData = new List<string[]>();
    string[] lines = File.ReadAllLines("Data/analytics.txt");

    foreach (string line in lines)
    {
        Console.WriteLine($"Read Line: {line}");
        string[] fields = line.Split(',');
        analyticsData.Add(fields);
    }

    return analyticsData;
}
}


