using System;
using System.Collections.Generic;
using System.IO;

public class Authentication
{
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
        Analytics analytics = new Analytics();
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
                    analytics.ViewAnalytics();
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
        Courses cs = new Courses();

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
                    cs.PurchaseCourse(user, courses);
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

   

    private void AddNewCourse(Courses courses)
    {
        courses.AddCourse();
    }

     public void DisplayCourses(List<Course> courses)
    {
        Console.WriteLine("Available Courses:");
        foreach (var course in courses)
        {
            Console.WriteLine($"{course.Name} - {course.Price:C}");
        }
    }

    private void UpdateCourse(Courses courses)
    {
        courses.UpdateCourse();
    }

    private void DeleteCourse(Courses courses)
    {
        courses.DeleteCourse();
    }

 
}


