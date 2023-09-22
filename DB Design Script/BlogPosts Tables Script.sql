CREATE TABLE BlogPosts (
    PostId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
	ImagePath NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL,
	UpdatedAt DATETIME NULL
);

GO

CREATE TABLE Comments (
    CommentId INT PRIMARY KEY IDENTITY(1,1),
    PostId INT NOT NULL,
    CommentText NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME NOT NULL,
	UpdatedAt DATETIME NULL
);

GO

ALTER TABLE Comments
ADD FOREIGN KEY (PostId)
REFERENCES BlogPosts(PostId);

GO
