USE
wtt;

CREATE TABLE projects
(
    id          INT IDENTITY (1,1),
    name        VARCHAR(32),
    description VARCHAR(1024),
    created_at  DATETIME,
    PRIMARY KEY (id),
)