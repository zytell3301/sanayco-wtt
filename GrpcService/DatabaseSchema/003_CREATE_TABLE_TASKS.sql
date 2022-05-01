USE wtt;

CREATE TABLE tasks
(
    id            INT IDENTITY (1,1),
    title         VARCHAR(32),
    user_id       INT,
    project_id    INT,
    work_location VARCHAR(16),
    created_at    DATETIME,
    end_time      DATETIME,
    description   VARCHAR(1024),
    status        VARCHAR(32),
    PRIMARY KEY (id),
)