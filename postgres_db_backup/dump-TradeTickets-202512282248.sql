--
-- PostgreSQL database cluster dump
--

-- Started on 2025-12-28 22:48:41 +05

\restrict YWVlVYcbddvVjYRMPOJIGlhHTqBS2Wg1aJ7OGyNXtUqducVXDsP9cPUAhtEIS1f

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS;

--
-- User Configurations
--








\unrestrict YWVlVYcbddvVjYRMPOJIGlhHTqBS2Wg1aJ7OGyNXtUqducVXDsP9cPUAhtEIS1f

--
-- Databases
--

--
-- Database "template1" dump
--

\connect template1

--
-- PostgreSQL database dump
--

\restrict yguOUVNBg3MEbw6cGKwyjmTGg21m3Q1G75AO4lpaQqU7B3HbeZuH9NGneWwUtyS

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

-- Started on 2025-12-28 22:48:42 +05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2025-12-28 22:48:43 +05

--
-- PostgreSQL database dump complete
--

\unrestrict yguOUVNBg3MEbw6cGKwyjmTGg21m3Q1G75AO4lpaQqU7B3HbeZuH9NGneWwUtyS

--
-- Database "TradeTickets" dump
--

--
-- PostgreSQL database dump
--

\restrict lfvZc955gKb2xA3jCmol8XpytlxTbrf0A9nrpbVUSgf4gwwe3F49FUIMjXYHsya

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

-- Started on 2025-12-28 22:48:43 +05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3615 (class 1262 OID 16388)
-- Name: TradeTickets; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "TradeTickets" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'ru_RU.UTF-8';


ALTER DATABASE "TradeTickets" OWNER TO postgres;

\unrestrict lfvZc955gKb2xA3jCmol8XpytlxTbrf0A9nrpbVUSgf4gwwe3F49FUIMjXYHsya
\connect "TradeTickets"
\restrict lfvZc955gKb2xA3jCmol8XpytlxTbrf0A9nrpbVUSgf4gwwe3F49FUIMjXYHsya

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3027 (class 3456 OID 16389)
-- Name: russian; Type: COLLATION; Schema: public; Owner: postgres
--

CREATE COLLATION public.russian (provider = libc, locale = 'ru_RU.utf8');


ALTER COLLATION public.russian OWNER TO postgres;

--
-- TOC entry 238 (class 1255 OID 16390)
-- Name: compare_flight_dates(timestamp without time zone, timestamp without time zone); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.compare_flight_dates(departure_time timestamp without time zone, arrival_time timestamp without time zone) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
begin
	
	return departure_time != arrival_time and arrival_time > departure_time;
	
end
$$;


ALTER FUNCTION public.compare_flight_dates(departure_time timestamp without time zone, arrival_time timestamp without time zone) OWNER TO postgres;

--
-- TOC entry 239 (class 1255 OID 16391)
-- Name: compare_flight_places(bigint, bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.compare_flight_places(departure_place bigint, destination_place bigint) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
begin
	
	return departure_place != destination_place;
	
end
$$;


ALTER FUNCTION public.compare_flight_places(departure_place bigint, destination_place bigint) OWNER TO postgres;

--
-- TOC entry 240 (class 1255 OID 16392)
-- Name: compare_ticket_limit_by_flight(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.compare_ticket_limit_by_flight(f_id bigint) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
begin
	
	return get_count_tickets_by_flight_id(f_id) < get_totalplacesbyaircraft(get_aircraft_by_flight_id(f_id)); 
	
end
$$;


ALTER FUNCTION public.compare_ticket_limit_by_flight(f_id bigint) OWNER TO postgres;

--
-- TOC entry 241 (class 1255 OID 16393)
-- Name: get_aircraft_by_flight_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_aircraft_by_flight_id(f_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
declare
	aircraft_id_r int8;
begin
	
	select aircraft_id
	into aircraft_id_r
	from flight f 
	where f.id = f_id;
	
	return greatest(-1, aircraft_id_r);
end
$$;


ALTER FUNCTION public.get_aircraft_by_flight_id(f_id bigint) OWNER TO postgres;

--
-- TOC entry 242 (class 1255 OID 16394)
-- Name: get_count_tickets_by_flight_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_count_tickets_by_flight_id(fl_id bigint) RETURNS bigint
    LANGUAGE plpgsql
    AS $$
declare
	countTickets int8;
begin
	
	select count(*)
	into countTickets
	from ticket t 
	where t.flight_id = fl_id;
	
	return greatest(0, countTickets);
end
$$;


ALTER FUNCTION public.get_count_tickets_by_flight_id(fl_id bigint) OWNER TO postgres;

--
-- TOC entry 243 (class 1255 OID 16395)
-- Name: get_countrentedplaces(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_countrentedplaces(f_id bigint) RETURNS integer
    LANGUAGE plpgsql IMMUTABLE STRICT
    AS $$
declare
	countRentedPlaces_r int8;
begin
	
	select count(*)
	into countRentedPlaces_r
	from ticket t 
	where t.flight_id = f_id and t.is_sold;
	
	return greatest(0, countRentedPlaces_r);	
end
$$;


ALTER FUNCTION public.get_countrentedplaces(f_id bigint) OWNER TO postgres;

--
-- TOC entry 244 (class 1255 OID 16396)
-- Name: get_discountprice(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_discountprice(discount_id bigint) RETURNS bigint
    LANGUAGE plpgsql IMMUTABLE STRICT
    AS $$
declare
	discount_size_r int8;
begin
	
	select discount_size
	into discount_size_r
	from discount d
	where d.id = discount_id;
	
	return greatest(0, discount_size_r);
end
$$;


ALTER FUNCTION public.get_discountprice(discount_id bigint) OWNER TO postgres;

--
-- TOC entry 245 (class 1255 OID 16397)
-- Name: get_discountprice(integer, bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_discountprice(price integer, discount_id bigint) RETURNS bigint
    LANGUAGE plpgsql IMMUTABLE STRICT
    AS $$
declare
	discount_size_r int4;
begin
	
	select discount_size
	into discount_size_r
	from discount d
	where d.id = discount_id;
	
	return greatest(0, price - ((price * discount_size_r) / 100));
end
$$;


ALTER FUNCTION public.get_discountprice(price integer, discount_id bigint) OWNER TO postgres;

--
-- TOC entry 246 (class 1255 OID 16398)
-- Name: get_photo_is_deleted(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_photo_is_deleted(photo_id bigint) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
begin
	
	return (select is_deleted from photo p where p.id = photo_id); 
	
end
$$;


ALTER FUNCTION public.get_photo_is_deleted(photo_id bigint) OWNER TO postgres;

--
-- TOC entry 247 (class 1255 OID 16399)
-- Name: get_price_by_flight_id(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_price_by_flight_id(f_id bigint) RETURNS integer
    LANGUAGE plpgsql IMMUTABLE
    AS $$
declare
	flight_price_r int8;
begin
	
	select price
	into flight_price_r
	from flight f 
	where f.id = f_id;
	
	return greatest(-1, flight_price_r);
end
$$;


ALTER FUNCTION public.get_price_by_flight_id(f_id bigint) OWNER TO postgres;

--
-- TOC entry 248 (class 1255 OID 16400)
-- Name: get_totalplacesbyaircraft(bigint); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_totalplacesbyaircraft(aircraft_id bigint) RETURNS integer
    LANGUAGE plpgsql IMMUTABLE STRICT
    AS $$
declare
	total_place_r int8;
begin
	
	select total_place
	into total_place_r
	from aircraft a 
	where a.id = aircraft_id;
	
	return greatest(0, total_place_r);	
end
$$;


ALTER FUNCTION public.get_totalplacesbyaircraft(aircraft_id bigint) OWNER TO postgres;

--
-- TOC entry 249 (class 1255 OID 16401)
-- Name: trigger_exception_aircraft_changed_totalplace(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.trigger_exception_aircraft_changed_totalplace() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
declare 
	count_incorrect_values int8; 
begin
	
	select count(*)
	from 
	(
		select (new.total_place - (new.total_place - f.free_place)) as diff_val
		from flight f 
		where new.id = f.aircraft_id
	)
	where diff_val < 0 
	into count_incorrect_values;

	if count_incorrect_values > 0 then 
		raise exception 'Exception: The number of seats cannot be less than the rented tickets. (% - total, % - count)', new.total_place, count_incorrect_values;	
	end if;
	
	return new;
	
end
$$;


ALTER FUNCTION public.trigger_exception_aircraft_changed_totalplace() OWNER TO postgres;

--
-- TOC entry 250 (class 1255 OID 16402)
-- Name: trigger_exception_ticket_limit_by_flight(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.trigger_exception_ticket_limit_by_flight() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	
	raise exception 'Exception: the number of tickets cannot exceed the number of passenger seats';

end
$$;


ALTER FUNCTION public.trigger_exception_ticket_limit_by_flight() OWNER TO postgres;

--
-- TOC entry 251 (class 1255 OID 16403)
-- Name: trigger_update_deleted_photo_info(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.trigger_update_deleted_photo_info() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	
	update aircraft set photo_id = 1 where new.id = aircraft.photo_id ;
	update place  set photo_id = 1 where new.id = place.photo_id ;	

	return new;

end
$$;


ALTER FUNCTION public.trigger_update_deleted_photo_info() OWNER TO postgres;

--
-- TOC entry 252 (class 1255 OID 16404)
-- Name: trigger_update_tables_info(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.trigger_update_tables_info() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	
	/* RAISE EXCEPTION 'Table name is: %', TG_TABLE_NAME; */
	
	if TG_TABLE_NAME = 'ticket' then 	
		
		if TG_OP = 'INSERT' then 
		
			update flight
				set aircraft_id = flight.aircraft_id
			where id = new .flight_id;
		
			return new;
		
		end if;
	
		update flight
			set aircraft_id = flight.aircraft_id
		where id = old .flight_id;
	
	end if;

	if TG_TABLE_NAME = 'aircraft' then
		
		update flight
			set aircraft_id = flight.aircraft_id
		where aircraft_id  = old .id ;
	
	end if;
	

	return new;

end
$$;


ALTER FUNCTION public.trigger_update_tables_info() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 16405)
-- Name: aircraft; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.aircraft (
    id integer NOT NULL,
    model character varying(50) NOT NULL,
    type character varying(30) NOT NULL,
    total_place integer,
    photo_id bigint NOT NULL,
    CONSTRAINT ck_check_on_deleted_photo CHECK ((NOT public.get_photo_is_deleted(photo_id)))
);


ALTER TABLE public.aircraft OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16413)
-- Name: aircraft_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.aircraft ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.aircraft_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 221 (class 1259 OID 16414)
-- Name: airline; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.airline (
    id integer NOT NULL,
    name character varying(50) NOT NULL
);


ALTER TABLE public.airline OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16419)
-- Name: airline_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.airline ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.airline_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 223 (class 1259 OID 16420)
-- Name: code_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.code_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.code_seq OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 16421)
-- Name: discount; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.discount (
    id integer NOT NULL,
    name character varying(30) NOT NULL,
    discount_size integer NOT NULL,
    description character varying(50) NOT NULL
);


ALTER TABLE public.discount OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 16428)
-- Name: discount_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.discount ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.discount_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 226 (class 1259 OID 16429)
-- Name: flight; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.flight (
    id integer NOT NULL,
    flight_number bigint NOT NULL,
    departure_place bigint NOT NULL,
    departure_time timestamp without time zone NOT NULL,
    destination_place bigint NOT NULL,
    arrival_time timestamp without time zone NOT NULL,
    aircraft_id bigint NOT NULL,
    duration_time interval GENERATED ALWAYS AS ((arrival_time - departure_time)) STORED,
    airline_id bigint,
    is_canceled boolean NOT NULL,
    price integer NOT NULL,
    total_place integer GENERATED ALWAYS AS (public.get_totalplacesbyaircraft(aircraft_id)) STORED,
    free_place integer GENERATED ALWAYS AS ((public.get_totalplacesbyaircraft(aircraft_id) - public.get_countrentedplaces((id)::bigint))) STORED,
    CONSTRAINT ck_compare_flight_dates CHECK (public.compare_flight_dates(departure_time, arrival_time)),
    CONSTRAINT ck_compare_flight_places CHECK (public.compare_flight_places(departure_place, destination_place))
);


ALTER TABLE public.flight OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 16446)
-- Name: flight_class; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.flight_class (
    id integer NOT NULL,
    class_name character varying(50) NOT NULL
);


ALTER TABLE public.flight_class OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 16451)
-- Name: flight_class_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.flight_class ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.flight_class_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 229 (class 1259 OID 16452)
-- Name: flight_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.flight ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.flight_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 230 (class 1259 OID 16453)
-- Name: photo; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.photo (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    url_path character varying(200) NOT NULL,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.photo OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 16460)
-- Name: photo_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.photo ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.photo_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 232 (class 1259 OID 16461)
-- Name: place; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.place (
    id integer NOT NULL,
    name character varying(30) NOT NULL,
    description character varying(50) NOT NULL,
    photo_id bigint NOT NULL,
    CONSTRAINT ck_check_on_deleted_photo CHECK ((NOT public.get_photo_is_deleted(photo_id)))
);


ALTER TABLE public.place OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 16469)
-- Name: place_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.place ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.place_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 234 (class 1259 OID 16470)
-- Name: ticket; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ticket (
    id integer NOT NULL,
    flight_id bigint NOT NULL,
    class_id bigint NOT NULL,
    place_number integer NOT NULL,
    discount_id bigint NOT NULL,
    is_sold boolean,
    user_id integer,
    CONSTRAINT ck_ticket_range_place_number CHECK (((place_number >= 1) AND (place_number <= public.get_totalplacesbyaircraft(public.get_aircraft_by_flight_id(flight_id))))),
    CONSTRAINT ck_ticket_sold_userid CHECK ((NOT (is_sold AND (user_id IS NULL))))
);


ALTER TABLE public.ticket OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 16479)
-- Name: ticket_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.ticket ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.ticket_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 236 (class 1259 OID 16480)
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    id integer NOT NULL,
    password character varying NOT NULL,
    login character varying(30) NOT NULL,
    role character varying,
    balance money,
    name character varying(50)
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 16488)
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."user" ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 3591 (class 0 OID 16405)
-- Dependencies: 219
-- Data for Name: aircraft; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.aircraft (id, model, type, total_place, photo_id) FROM stdin;
27	Gulfstream IVSP	Бизнес-класс	16	4
28	Boeing-737	Пассажирский	165	3
29	ИЛ-86	Пассажирский	350	2
30	ИЛ-866	Пассажирский	350	1
\.


--
-- TOC entry 3593 (class 0 OID 16414)
-- Dependencies: 221
-- Data for Name: airline; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.airline (id, name) FROM stdin;
1	Уральские авиалинии (Ural Airlines)
3	Американские Авиалинии (American Airlines)
2	Победа (Victory)
\.


--
-- TOC entry 3596 (class 0 OID 16421)
-- Dependencies: 224
-- Data for Name: discount; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.discount (id, name, discount_size, description) FROM stdin;
2	Молодежная	10	Возраст от 8 до 16 лет
5	Пенсионная	10	При предъявлении пенсионного удостоверения.
6	Отсутствует	5	-
1	Детская	0	Для детей в возрасте до 7 лет включительно
\.


--
-- TOC entry 3598 (class 0 OID 16429)
-- Dependencies: 226
-- Data for Name: flight; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.flight (id, flight_number, departure_place, departure_time, destination_place, arrival_time, aircraft_id, airline_id, is_canceled, price) FROM stdin;
32	102	17	2024-10-07 17:00:00	15	2024-10-08 15:00:00	27	2	f	45000
34	133	16	2024-02-19 22:00:00	17	2024-02-20 06:00:00	27	2	f	22800
33	100	19	2024-02-03 20:15:00	17	2024-02-04 05:00:00	27	1	f	27000
29	101	16	2024-09-01 19:00:00	18	2024-09-02 17:00:00	29	3	f	50000
35	106	16	2024-09-01 07:30:00	18	2024-09-02 02:02:00	28	1	t	22800
\.


--
-- TOC entry 3599 (class 0 OID 16446)
-- Dependencies: 227
-- Data for Name: flight_class; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.flight_class (id, class_name) FROM stdin;
1	Первый класс
2	Второй класс
3	Третий класс
\.


--
-- TOC entry 3602 (class 0 OID 16453)
-- Dependencies: 230
-- Data for Name: photo; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.photo (id, name, url_path, is_deleted) FROM stdin;
1	Удаленное фото	https://www.sknis.gov.kn/wp-content/uploads/2018/05/removed-occupations-australia-2017.jpg	f
3	Boeing-737	https://i.imgur.com/vzk3CoP.jpeg	f
4	Gulfstream IVSP	https://i.imgur.com/wxyEuRK.jpeg	f
5	США - Сан-Диего (Сан-Диего, штат Калифорния)	https://i.imgur.com/Ls1WqFj.jpeg	f
6	США - Washington Dulles (Вашингтон)	https://i.imgur.com/Ij7oM6H.jpeg	f
7	Россия - Москва (Домодедово)	https://i.imgur.com/rofRzDj.jpeg	f
8	Россия - Москва (Внуково)	https://i.imgur.com/OvM15c0.jpeg	f
2	ИЛ-86	https://i.imgur.com/WyEhqyU.png	f
9	Россия - Екатеринбург (Кольцово)	https://i.imgur.com/kEpRFqp.jpeg	f
\.


--
-- TOC entry 3604 (class 0 OID 16461)
-- Dependencies: 232
-- Data for Name: place; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.place (id, name, description, photo_id) FROM stdin;
15	США	Сан-Диего (Сан-Диего, штат Калифорния)	5
16	США	Washington Dulles (Вашингтон)	6
17	Россия	Москва (Домодедово)	7
18	Россия	Москва (Внуково)	8
19	Россия	Екатеринбург (Кольцово)	9
\.


--
-- TOC entry 3606 (class 0 OID 16470)
-- Dependencies: 234
-- Data for Name: ticket; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.ticket (id, flight_id, class_id, place_number, discount_id, is_sold, user_id) FROM stdin;
154	33	2	2	6	t	4
161	35	2	1	6	f	\N
157	29	2	2	6	t	4
160	34	2	1	5	t	4
159	33	1	1	5	t	4
155	33	3	3	1	t	4
158	29	2	7	6	f	\N
156	29	1	1	6	t	7
\.


--
-- TOC entry 3608 (class 0 OID 16480)
-- Dependencies: 236
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."user" (id, password, login, role, balance, name) FROM stdin;
1	123	user1	dispatcher	0,00 ₽	-
2	321	user2	admin	0,00 ₽	-
4	456	client1	user	0,00 ₽	Иванов Андрей Викторович
7	777	client2	user	0,00 ₽	Петров Геннадий Константинович
\.


--
-- TOC entry 3616 (class 0 OID 0)
-- Dependencies: 220
-- Name: aircraft_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.aircraft_id_seq', 30, true);


--
-- TOC entry 3617 (class 0 OID 0)
-- Dependencies: 222
-- Name: airline_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.airline_id_seq', 8, true);


--
-- TOC entry 3618 (class 0 OID 0)
-- Dependencies: 223
-- Name: code_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.code_seq', 1, false);


--
-- TOC entry 3619 (class 0 OID 0)
-- Dependencies: 225
-- Name: discount_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.discount_id_seq', 9, true);


--
-- TOC entry 3620 (class 0 OID 0)
-- Dependencies: 228
-- Name: flight_class_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.flight_class_id_seq', 7, true);


--
-- TOC entry 3621 (class 0 OID 0)
-- Dependencies: 229
-- Name: flight_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.flight_id_seq', 35, true);


--
-- TOC entry 3622 (class 0 OID 0)
-- Dependencies: 231
-- Name: photo_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.photo_id_seq', 13, true);


--
-- TOC entry 3623 (class 0 OID 0)
-- Dependencies: 233
-- Name: place_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.place_id_seq', 19, true);


--
-- TOC entry 3624 (class 0 OID 0)
-- Dependencies: 235
-- Name: ticket_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ticket_id_seq', 161, true);


--
-- TOC entry 3625 (class 0 OID 0)
-- Dependencies: 237
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_id_seq', 31, true);


--
-- TOC entry 3395 (class 2606 OID 16490)
-- Name: aircraft aircraft_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.aircraft
    ADD CONSTRAINT aircraft_pk PRIMARY KEY (id);


--
-- TOC entry 3399 (class 2606 OID 16492)
-- Name: airline airline_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.airline
    ADD CONSTRAINT airline_pk PRIMARY KEY (id);


--
-- TOC entry 3413 (class 2606 OID 16494)
-- Name: photo ck_unique_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.photo
    ADD CONSTRAINT ck_unique_name UNIQUE (name);


--
-- TOC entry 3415 (class 2606 OID 16496)
-- Name: photo ck_unique_urlpath; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.photo
    ADD CONSTRAINT ck_unique_urlpath UNIQUE (url_path);


--
-- TOC entry 3402 (class 2606 OID 16498)
-- Name: discount discount_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.discount
    ADD CONSTRAINT discount_pk PRIMARY KEY (id);


--
-- TOC entry 3410 (class 2606 OID 16500)
-- Name: flight_class flight_class_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flight_class
    ADD CONSTRAINT flight_class_pk PRIMARY KEY (id);


--
-- TOC entry 3405 (class 2606 OID 16502)
-- Name: flight flight_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flight
    ADD CONSTRAINT flight_pk PRIMARY KEY (id);


--
-- TOC entry 3417 (class 2606 OID 16504)
-- Name: photo photo_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.photo
    ADD CONSTRAINT photo_pk PRIMARY KEY (id);


--
-- TOC entry 3421 (class 2606 OID 16506)
-- Name: place place_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.place
    ADD CONSTRAINT place_pk PRIMARY KEY (id);


--
-- TOC entry 3424 (class 2606 OID 16508)
-- Name: ticket ticket_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ticket
    ADD CONSTRAINT ticket_pk PRIMARY KEY (id);


--
-- TOC entry 3426 (class 2606 OID 16510)
-- Name: user user_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pk PRIMARY KEY (id);


--
-- TOC entry 3428 (class 2606 OID 24582)
-- Name: user user_unique_login; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_unique_login UNIQUE (login);


--
-- TOC entry 3406 (class 1259 OID 16513)
-- Name: flight_uniqueflightnumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX flight_uniqueflightnumber ON public.flight USING btree (flight_number);


--
-- TOC entry 3400 (class 1259 OID 16514)
-- Name: idx_unique_airline_name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_unique_airline_name ON public.airline USING btree (name);


--
-- TOC entry 3407 (class 1259 OID 16515)
-- Name: idx_unique_departureplace_departure_time; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_unique_departureplace_departure_time ON public.flight USING btree (departure_place, departure_time) WHERE (is_canceled = false);


--
-- TOC entry 3408 (class 1259 OID 16516)
-- Name: idx_unique_destplace_arrival_time; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_unique_destplace_arrival_time ON public.flight USING btree (destination_place, arrival_time) WHERE (is_canceled = false);


--
-- TOC entry 3403 (class 1259 OID 16517)
-- Name: idx_unique_discount_name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_unique_discount_name ON public.discount USING btree (name);


--
-- TOC entry 3411 (class 1259 OID 16518)
-- Name: idx_unique_flightclass_name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_unique_flightclass_name ON public.flight_class USING btree (class_name);


--
-- TOC entry 3422 (class 1259 OID 16519)
-- Name: idx_unique_flightid_placenumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_unique_flightid_placenumber ON public.ticket USING btree (flight_id, place_number);


--
-- TOC entry 3396 (class 1259 OID 16520)
-- Name: idx_unique_model; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_unique_model ON public.aircraft USING btree (model);


--
-- TOC entry 3418 (class 1259 OID 16521)
-- Name: idx_unique_name_description; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_unique_name_description ON public.place USING btree (name, description);


--
-- TOC entry 3397 (class 1259 OID 16522)
-- Name: photo_unique_photo_aircraft; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX photo_unique_photo_aircraft ON public.aircraft USING btree (photo_id) WHERE (photo_id <> 1);


--
-- TOC entry 3419 (class 1259 OID 16523)
-- Name: photo_unique_photo_place; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX photo_unique_photo_place ON public.place USING btree (photo_id) WHERE (photo_id <> 1);


--
-- TOC entry 3439 (class 2620 OID 16524)
-- Name: aircraft triger_update_flight_aircraft; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER triger_update_flight_aircraft AFTER UPDATE ON public.aircraft FOR EACH ROW EXECUTE FUNCTION public.trigger_update_tables_info();


--
-- TOC entry 3442 (class 2620 OID 16525)
-- Name: ticket triger_update_flight_ticket; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER triger_update_flight_ticket AFTER INSERT OR DELETE OR UPDATE ON public.ticket FOR EACH ROW EXECUTE FUNCTION public.trigger_update_tables_info();


--
-- TOC entry 3443 (class 2620 OID 16526)
-- Name: ticket trigger_compare_ticket_limit_by_flight; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trigger_compare_ticket_limit_by_flight BEFORE INSERT ON public.ticket FOR EACH ROW WHEN ((NOT public.compare_ticket_limit_by_flight(new.flight_id))) EXECUTE FUNCTION public.trigger_exception_ticket_limit_by_flight();


--
-- TOC entry 3440 (class 2620 OID 16527)
-- Name: aircraft trigger_constraint_aircraft_incorrect_totalplace; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trigger_constraint_aircraft_incorrect_totalplace AFTER UPDATE ON public.aircraft FOR EACH ROW EXECUTE FUNCTION public.trigger_exception_aircraft_changed_totalplace();


--
-- TOC entry 3441 (class 2620 OID 16528)
-- Name: photo trigger_update_deleted_photo_info; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trigger_update_deleted_photo_info AFTER UPDATE ON public.photo FOR EACH ROW WHEN (new.is_deleted) EXECUTE FUNCTION public.trigger_update_deleted_photo_info();


--
-- TOC entry 3429 (class 2606 OID 16529)
-- Name: aircraft aircraft_photo_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.aircraft
    ADD CONSTRAINT aircraft_photo_fk FOREIGN KEY (photo_id) REFERENCES public.photo(id);


--
-- TOC entry 3430 (class 2606 OID 16534)
-- Name: flight fk_flight_aircraft_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flight
    ADD CONSTRAINT fk_flight_aircraft_id FOREIGN KEY (aircraft_id) REFERENCES public.aircraft(id);


--
-- TOC entry 3431 (class 2606 OID 16539)
-- Name: flight fk_flight_departure_place; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flight
    ADD CONSTRAINT fk_flight_departure_place FOREIGN KEY (departure_place) REFERENCES public.place(id);


--
-- TOC entry 3432 (class 2606 OID 16544)
-- Name: flight fk_flight_destination_place; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flight
    ADD CONSTRAINT fk_flight_destination_place FOREIGN KEY (destination_place) REFERENCES public.place(id);


--
-- TOC entry 3435 (class 2606 OID 16549)
-- Name: ticket fk_ticket_classid; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ticket
    ADD CONSTRAINT fk_ticket_classid FOREIGN KEY (class_id) REFERENCES public.flight_class(id);


--
-- TOC entry 3436 (class 2606 OID 16554)
-- Name: ticket fk_ticket_discountid; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ticket
    ADD CONSTRAINT fk_ticket_discountid FOREIGN KEY (discount_id) REFERENCES public.discount(id);


--
-- TOC entry 3437 (class 2606 OID 16559)
-- Name: ticket fk_ticket_flight_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ticket
    ADD CONSTRAINT fk_ticket_flight_id FOREIGN KEY (flight_id) REFERENCES public.flight(id);


--
-- TOC entry 3438 (class 2606 OID 24590)
-- Name: ticket fk_ticket_userid; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ticket
    ADD CONSTRAINT fk_ticket_userid FOREIGN KEY (user_id) REFERENCES public."user"(id);


--
-- TOC entry 3433 (class 2606 OID 16564)
-- Name: flight flight_airline_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.flight
    ADD CONSTRAINT flight_airline_fk FOREIGN KEY (airline_id) REFERENCES public.airline(id);


--
-- TOC entry 3434 (class 2606 OID 16569)
-- Name: place place_photo_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.place
    ADD CONSTRAINT place_photo_fk FOREIGN KEY (photo_id) REFERENCES public.photo(id);


-- Completed on 2025-12-28 22:48:44 +05

--
-- PostgreSQL database dump complete
--

\unrestrict lfvZc955gKb2xA3jCmol8XpytlxTbrf0A9nrpbVUSgf4gwwe3F49FUIMjXYHsya

--
-- Database "postgres" dump
--

\connect postgres

--
-- PostgreSQL database dump
--

\restrict tzFhrNLhYtf3u56WasetpCvoQuzps5SClbuzL8PUp9y7ybAiJS1kmxchcGcGKy5

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

-- Started on 2025-12-28 22:48:44 +05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2025-12-28 22:48:44 +05

--
-- PostgreSQL database dump complete
--

\unrestrict tzFhrNLhYtf3u56WasetpCvoQuzps5SClbuzL8PUp9y7ybAiJS1kmxchcGcGKy5

-- Completed on 2025-12-28 22:48:44 +05

--
-- PostgreSQL database cluster dump complete
--

