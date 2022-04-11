CREATE TABLE off_time
(
    id          INT,
    user_id     INT,
    status      CHAR,
    from_date   DATETIME,
    to_date     DATETIME,
    description VARCHAR,
    created_at  DATETIME,
    PRIMARY KEY (id),
)