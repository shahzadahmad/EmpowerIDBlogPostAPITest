CREATE TABLE UserAccounts (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    -- Add other user profile information here.
);
GO

CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    -- Add any other role-related information here.
);

GO

CREATE TABLE UserRoles (
    UserRoleID INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
);

GO

ALTER TABLE UserRoles
ADD FOREIGN KEY (UserId)
REFERENCES UserAccounts(UserId);

ALTER TABLE UserRoles
ADD FOREIGN KEY (RoleId)
REFERENCES Roles(RoleId);

