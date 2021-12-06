using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Data
{
    public static class AppConstants
    {
        public const int OkStatus = 200;
        public const int CreatedStatus = 201;
        public const int BadRequestStatus = 400;
        public const int NotFoundStatus = 200;
        public const string ConnectionStringName = "DefaultConnectionString";
        public const string AdminRole = "Admin";
        public const string UserRole = "User";
        public const string InvalidUsernameAndPassword = "Invalid username or password.";
        public const string UserAlreadyExist = "User already exist.";
        public const string UserCreated = "User is created successfully";
    }
}
