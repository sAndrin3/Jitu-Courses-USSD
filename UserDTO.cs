public enum UserRole {
    User,
    Admin
}

public class UserDTO {
    public string Username {get; set;}
    public UserRole Role {get; set;}
    public string Password {get; set; }
}