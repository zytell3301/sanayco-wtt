USE wtt;

CREATE TABLE users
(
    id            INT IDENTITY (1,1),
    name          VARCHAR(32),
    lastname      VARCHAR(32),
    skill_level   VARCHAR(32), /* Set this to a lower value if needed*/
    company_level VARCHAR(32),
    created_at    DATETIME,
    PRIMARY KEY (id),
)