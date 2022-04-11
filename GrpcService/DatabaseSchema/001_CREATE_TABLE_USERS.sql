CREATE TABLE users
(
    id            INT IDENTITY (1,1),
    name          CHAR(32),
    lastname      CHAR(32),
    skill_level   CHAR(10), /* Set this to a lower value if needed*/
    company_level CHAR(10),
    created_at    DATETIME,
    PRIMARY KEY (id),
)