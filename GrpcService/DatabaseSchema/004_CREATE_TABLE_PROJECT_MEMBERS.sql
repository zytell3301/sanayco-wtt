USE
wtt;

CREATE TABLE project_members
(
    id         INT IDENTITY (1,1),
    user_id    INT,
    project_id INT,
    level      VARCHAR(32),
    created_at DATETIME,
    PRIMARY KEY (id),
)