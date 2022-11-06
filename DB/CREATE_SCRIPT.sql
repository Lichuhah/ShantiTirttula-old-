create table [USER]
(
    ID       int identity
        constraint USER_pk
            primary key,
    LOGIN    nvarchar(255) not null,
    PASSWORD nvarchar(255)
)
go

create table CONTROLLER
(
    ID      int identity
        constraint CONTROLLER_pk
            primary key,
    MAC     nvarchar(255),
    U_KEY   nvarchar(255)
        constraint CONTROLLLER_KEY
            unique,
    USER_ID int
        constraint CONTROLLLER_USER_null_fk
            references [USER]
)
go

create table MC_COMMAND
(
    ID     int identity
        constraint MC_COMMAND_pk
            primary key,
    MC_KEY nvarchar(255)
        constraint MC_COMMAND_CONTROLLER_null_fk
            references CONTROLLER (U_KEY),
    A      nvarchar(15),
    B      int
)
go

create table MC_LIGHT_DATA
(
    ID       int identity
        constraint MC_LIGHT_DATA_pk
            primary key,
    MC_KEY   nvarchar(255)
        constraint MC_LIGHT_DATA_CONTROLLER_null_fk
            references CONTROLLER (U_KEY),
    VALUE    float,
    DATETIME datetime2
)
go

