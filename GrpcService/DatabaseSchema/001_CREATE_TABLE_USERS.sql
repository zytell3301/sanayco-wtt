USE wtt;

CREATE TABLE users
(
    id            INT IDENTITY (1,1),
    name          VARCHAR(32),
    lastname      VARCHAR(32),
    skill_level   VARCHAR(10), /* Set this to a lower value if needed*/
    company_level VARCHAR(10),
    created_at    DATETIME,
    PRIMARY KEY (id),
)