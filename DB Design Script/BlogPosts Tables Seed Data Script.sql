-- Insert sample blog posts
INSERT INTO BlogPosts (Title, Content, CreatedAt, ImagePath)
VALUES
    ('Introduction to Blogging', 'This is a sample blog post about blogging.', GETDATE(), '/images/blog1.jpg'),
    ('How to Write Great Content', 'Learn tips and tricks for writing engaging content.', GETDATE(), '/images/blog2.jpg'),
    ('Choosing the Right Blogging Platform', 'Explore different platforms for hosting your blog.', GETDATE(), '/images/blog3.jpg');

GO

-- Insert sample comments
INSERT INTO Comments (PostId, CommentText, CreatedAt)
VALUES
    (1, 'Great introductory post!', GETDATE()),
    (1, 'Looking forward to more articles.', GETDATE()),
    (2, 'Excellent tips for content writing.', GETDATE()),
    (3, 'I found this information very helpful.', GETDATE());

GO