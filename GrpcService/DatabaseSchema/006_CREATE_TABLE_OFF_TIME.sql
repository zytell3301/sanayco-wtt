CREATE TABLE off_time
(
    id          INT IDENTITY (1,1),
    user_id     INT,
    status      CHAR(16),
    from_date   DATETIME,
    to_date     DATETIME,
    description VARCHAR,
    created_at  DATETIME,
    PRIMARY KEY (id),
)