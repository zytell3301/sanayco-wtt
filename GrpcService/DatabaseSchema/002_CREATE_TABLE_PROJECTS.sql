CREATE TABLE projects
(
    id          INT IDENTITY (1,1),
    name        CHAR(32),
    description VARCHAR,
    created_at  DATETIME,
    PRIMARY KEY (id),
)