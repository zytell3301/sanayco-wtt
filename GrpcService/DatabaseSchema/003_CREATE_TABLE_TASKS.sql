CREATE TABLE tasks
(
    id            INT IDENTITY (1,1),
    title         CHAR(32),
    user_id       INT,
    project_id    INT,
    work_location CHAR(16),
    created_at    DATETIME,
    end_time      DATETIME,
    description   VARCHAR,
    status        CHAR(16),
    PRIMARY KEY (id),
)