create table TYPE_CONTROLLER
(
    ID   int identity,
    NAME nvarchar(255),
    constraint TYPE_CONTROLLER_pk
        primary key (ID)
)
go

create table TYPE_DEVICE
(
    ID        int identity,
    NAME      nvarchar(255),
    MAX_VALUE int,
    MIN_VALUE int,
    PWM       bit,
    constraint TYPE_DEVICE_pk
        primary key (ID)
)
go

create table TYPE_CONTROLLER_DEVICE
(
    ID                 int identity,
    TYPE_CONTROLLER_ID int,
    TYPE_DEVICE_ID     int,
    PIN                int,
    constraint TYPE_CONTROLLER_DEVICE_pk
        primary key (ID),
    constraint TYPE_CONTROLLER_DEVICE_TYPE_CONTROLLER_null_fk
        foreign key (TYPE_CONTROLLER_ID) references TYPE_CONTROLLER,
    constraint TYPE_CONTROLLER_DEVICE_TYPE_DEVICE_null_fk
        foreign key (TYPE_DEVICE_ID) references TYPE_DEVICE
)
go

create table TYPE_SENSOR
(
    ID        int identity,
    NAME      nvarchar(255),
    MIN_VALUE float,
    MAX_VALUE float,
    UNIT      nvarchar(255),
    constraint TYPE_SENSOR_pk
        primary key (ID)
)
go

create table TYPE_CONTROLLER_SENSOR
(
    ID                 int identity,
    TYPE_CONTROLLER_ID int,
    TYPE_SENSOR_ID     int,
    constraint TYPE_CONTROLLER_SENSOR_pk
        primary key (ID),
    constraint TYPE_CONTROLLER_SENSOR_TYPE_CONTROLLER_null_fk
        foreign key (TYPE_CONTROLLER_ID) references TYPE_CONTROLLER,
    constraint TYPE_CONTROLLER_SENSOR_TYPE_SENSOR_null_fk
        foreign key (TYPE_SENSOR_ID) references TYPE_SENSOR
)
go

create table [USER]
(
    ID       int identity,
    LOGIN    nvarchar(255) not null,
    PASSWORD nvarchar(255),
    constraint USER_pk
        primary key (ID)
)
go

create table CONTROLLER
(
    ID      int identity,
    MAC     nvarchar(255),
    SERIAL  nvarchar(255),
    USER_ID int,
    TYPE_ID int,
    constraint CONTROLLER_pk
        primary key (ID),
    constraint CONTROLLLER_KEY
        unique (SERIAL),
    constraint CONTROLLER_TYPE_CONTROLLER_null_fk
        foreign key (TYPE_ID) references TYPE_CONTROLLER,
    constraint CONTROLLER_USER_null_fk
        foreign key (USER_ID) references [USER]
)
go

create table CONTROLLER_SENSOR_DATA
(
    ID             int identity,
    CONTROLLER_ID  int,
    TYPE_SENSOR_ID int,
    VALUE          float,
    DATE           datetime2,
    constraint CONTROLLER_SENSOR_DATA_pk
        primary key (ID),
    constraint CONTROLLER_SENSOR_DATA_CONTROLLER_null_fk
        foreign key (CONTROLLER_ID) references CONTROLLER,
    constraint CONTROLLER_SENSOR_DATA_TYPE_SENSOR_null_fk
        foreign key (TYPE_SENSOR_ID) references TYPE_SENSOR
)
go


