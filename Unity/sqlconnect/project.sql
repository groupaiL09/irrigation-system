create database unity_app;
use unity_app;

create table users 
	(
		user_id int(11) not null primary key auto_increment,
        username varchar(50) not null unique,
	    email varchar(100) not null unique,
        hash varchar(100) not null,
	    salt varchar(50) not null
    );
    
create table devices
	(
		device_id int(11) not null primary key auto_increment,
        type varchar(20) not null,
        name varchar(50) not null,
		topic varchar(100) 
    );
    
create table farms
	(
		farm_id int(11) not null primary key auto_increment,
        user_id int(11),
        location varchar(50),
        description varchar(100),
        foreign key (user_id) references users(user_id) on delete cascade
    );

create table consists_of
	(
		farm_id int(11),
        device_id int(11),
        topic_url varchar(50), 
        AIO_key varchar(50),
		foreign key(farm_id) references farms(farm_id) on delete cascade,
        foreign key(device_id) references devices(device_id) on delete cascade,
        primary key(farm_id, device_id)
    );


create table notifications
	(
		date datetime,
		user_id int(11),
        content varchar(100), 
        foreign key(user_id) references users(user_id) on delete cascade,
        primary key(date, user_id)
    );

create table history
	(
		date datetime,
        farm_id int(11),
        temperature float(10,2), 
        soil_moisture float(10,2),
        pump_status boolean,
        foreign key(farm_id) references farms(farm_id) on delete cascade,
        primary key(date, farm_id)
    );


	-- insert into users values (username, email, hash, salt) 
	-- 	values ('tuannguyen1', 'tuannguyen1@gmail.com', '$5$rounds=5000$steamedhamstuann$nzRHRHopHhix09yPO8LmN/qSw0dM4u6/g8QZtt05ID5', '$5$rounds=5000$steamedhamstuannguyen1$')
	-- insert into users values (username, email, hash, salt)
	-- 	values ('tuannguyen2', 'tuannguyen2@gmail.com', '$5$rounds=5000$steamedhamstuann$LcDIi5KhMs5ugH4//PObBFafmdHavbYbdC41w8VG9E0', '$5$rounds=5000$steamedhamstuannguyen2$')


	insert into devices (type, name) values('input', 'DHT11');
	insert into devices (type, name) values('input', 'Soil Moisture');
	insert into devices (type, name) values('output', 'Relay Circuit');
	insert into devices (type, name) values('output', 'LCD I2C ');


	-- insert into farms (user_id) values(1);
    -- insert into farms (user_id) values(1);
	-- insert into farms (user_id) values(2);


	insert into consists_of (farm_id, device_id) values(1, 1);
    insert into consists_of (farm_id, device_id) values(1, 2);
	insert into consists_of (farm_id, device_id) values(1, 3);
	insert into consists_of (farm_id, device_id) values(1, 4);


	insert into notifications (date, user_id, content) values ('2022-03-19 10:12:54', 1, 'chao ban lan 1 trong ngay');
	insert into notifications (date, user_id, content) values ('2022-03-19 18:01:12', 1, 'chao ban lan 2 trong ngay');
	insert into notifications (date, user_id, content) values ('2022-03-22 18:01:12', 1, 'chao ban lan 1 trong ngay');
	insert into notifications (date, user_id, content) values ('2022-03-28 12:23:04', 1, 'chao ban lan 1 trong ngay');

 
	insert into history (date, farm_id, temperature, soil_moisture, pump_status) values ('2022-03-27 17:12:55', 1, 25.13, 43.11, true);
	insert into history (date, farm_id, temperature, soil_moisture, pump_status) values ('2022-03-27 23:23:47', 1, 10.21, 55.10, false);
	insert into history (date, farm_id, temperature, soil_moisture, pump_status) values ('2022-03-28 06:13:41', 1, 12.22, 11.97, false);
