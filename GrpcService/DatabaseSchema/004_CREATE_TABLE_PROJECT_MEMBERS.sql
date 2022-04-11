CREATE TABLE project_members
(
    id         INT,
    user_id    INT,
    project_id INT,
    user_level CHAR,
    created_at DATETIME,
    PRIMARY KEY (id),
)