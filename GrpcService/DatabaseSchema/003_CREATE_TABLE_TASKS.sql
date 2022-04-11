CREATE TABLE tasks
(
    id            INT,
    title         CHAR,
    project_id    INT,
    work_location CHAR,
    created_at    DATETIME,
    end_time      DATETIME,
    description   VARCHAR,
    status        CHAR,
    PRIMARY KEY (id),
)