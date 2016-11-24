CREATE TABLE IF NOT EXISTS APP_CONFIG
(
	CONFIG_KEY text INTEGER CLUSTERED PRIMARY KEY,
	CONFIG_VALUE text NULL
);

-- Titles --------------------------------------------------------------------------------


CREATE TABLE IF NOT EXISTS TITLE
(
	TITLE_ID INTEGER PRIMARY KEY,
	TITLE_NAME TEXT NOT NULL,
	KIND INTEGER NOT NULL, 
	DATE_ADDED_UTC TEXT NOT NULL,
	DATE_MODIFIED_UTC TEXT NOT NULL,
	PARENT_TITLE_ID  INTEGER,
	ORD INTEGER NOT NULL,
             RELEASE_YEAR INTEGER NOT NULL,
             IMDB_ID TEXT NOT NULL,
             DESCRIPTION TEXT NOT NULL
);

CREATE INDEX IF NOT EXISTS IX_TITLE__TITLE_NAME__KIND ON TITLE(TITLE_NAME, KIND);

CREATE INDEX IF NOT EXISTS IX_TITLE__PARENT_TITLE_ID ON TITLE(PARENT_TITLE_ID);

-- Device specific stuff
CREATE TABLE IF NOT EXISTS DEVICE
(
	DEVICE_ID INTEGER PRIMARY KEY,
	DEVICE_NAME TEXT NOT NULL,
	DEVICE_KIND INTEGER NOT NULL,
	DEVICE_DATA TEXT NOT NULL
);



-- Media location -------------------------------------------------------------------------
--DROP  TABLE  LOCATION_BASE;
CREATE TABLE IF NOT EXISTS LOCATION_BASE
(
	LOCATION_BASE_ID INTEGER PRIMARY KEY,
	LOCATION_KIND INTEGER  NOT NULL,
	LOCATION text NOT NULL
);

--DROP TABLE EXISTS DEVICE_LOCATION_MAP
CREATE TABLE IF NOT EXISTS DEVICE_LOCATION_MAP
(
	DEVICE_ID INTEGER NOT NULL CONSTRAINT FK_DEVICE_LOCATION_MAP__DEVICE_ID REFERENCES DEVICE(DEVICE_ID), 
	LOCATION_BASE_ID INTEGER NOT NULL CONSTRAINT FK_LOCATION__LOCATION_BASE_ID REFERENCES LOCATION_BASE(LOCATION_BASE_ID),
	LOCATION_MAPPING text NOT NULL,
	PRIMARY KEY(DEVICE_ID, LOCATION_BASE_ID)
);


CREATE TABLE IF NOT EXISTS LOCATION
(
	LOCATION_ID INTEGER PRIMARY KEY,
	TITLE_ID INTEGER NOT NULL CONSTRAINT FK_LOCATION__PARENT_TITLE_ID REFERENCES TITLE(TITLE_ID),
	LOCATION_BASE_ID INTEGER NOT NULL,
	MEDIA_KIND INTEGER NOT NULL,
	DATE_ADDED_UTC text,
	DATE_MODIFIED_UTC text,
	LOCATION_DATA text
);
CREATE INDEX IF NOT EXISTS IX_LOCATION__TITLE_ID ON LOCATION(TITLE_ID);
CREATE INDEX IF NOT EXISTS IX_LOCATION__LOCATION_KIND__LOCATION_DATA ON LOCATION(LOCATION_DATA);

-- Media samples (images, etc) -------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS MEDIA_SAMPLE (
    SAMLE_ID   INTEGER PRIMARY KEY,
    TITLE_ID   INTEGER NOT NULL
                       CONSTRAINT FK_MEDIA_SAMPLE__TITLE_ID REFERENCES TITLE (TITLE_ID),
    MEDIA_KIND INTEGER NOT NULL,
    EXTENTSON  TEXT    NOT NULL
);




-- Custom properties -------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS PROPERTY
(
	PROPERTY_ID INTEGER PRIMARY KEY,
	PROPERTY_KIND char(1) NOT NULL,
	PROPERTY_NAME text NOT NULL
);

CREATE INDEX IF NOT EXISTS IX_PROPERTY__PROPERTY_NAME__PROPERTY_KIND ON PROPERTY(PROPERTY_NAME, PROPERTY_KIND);



CREATE TABLE IF NOT EXISTS TITLE_PROPERTY
(
	TITLE_ID INTEGER NOT NULL CONSTRAINT FK_TITLE_PROPERTY__TITLE_ID REFERENCES TITLE(TITLE_ID),
	PROPERTY_ID INTEGER NOT NULL CONSTRAINT FK_TITLE_PROPERTY__PROPERTY_ID REFERENCES PROPERTY(PROPERTY_ID),
	PROPERTY_VALUE text,
	PRIMARY KEY(TITLE_ID, PROPERTY_ID)
);

-- External info providers  ----------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS EXTERNAL_PROVIDER
(
	PROVIDER_ID INTEGER PRIMARY KEY,
	PROVIDER_KIND char(1) NOT NULL,
	PROVIDER_NAME text NOT NULL,
	URL_TEMPLATE text NOT NULL
);
CREATE INDEX IF NOT EXISTS IX_PROPERTY__PROVIDER_NAME_PROVIDER_KIND ON EXTERNAL_PROVIDER(PROVIDER_NAME, PROVIDER_KIND);


CREATE TABLE IF NOT EXISTS TITLE_EXTERNAL_LINK
(
	TITLE_ID INTEGER NOT NULL CONSTRAINT FK_TITLE_EXTERNAL_LINK__TITLE_ID REFERENCES TITLE(TITLE_ID),
	PROVIDER_ID INTEGER NOT NULL CONSTRAINT FK_TITLE_EXTERNAL_LINK__PROVIDER_ID REFERENCES EXTERNAL_PROVIDER(PROVIDER_ID),
	LINK_DATA text NOT NULL,
	PRIMARY KEY(TITLE_ID, PROVIDER_ID)
);
CREATE INDEX IF NOT EXISTS IX_TITLE_EXTERNAL_LINK__PROVIDER_ID__LINK_DATA ON TITLE_EXTERNAL_LINK(PROVIDER_ID, LINK_DATA);

-- Raitings  ----------------------------------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS RATING_PROVIDER
(
	RATING_ID INTEGER PRIMARY KEY,
	RATING_KIND INTEGER NOT NULL,
	RATING_NAME text NOT NULL,
             RATING_MIN  REAL NOT NULL,
             RATING_MAX  REAL NOT NULL,
             RATING_STEP REAL 
);
CREATE INDEX IF NOT EXISTS IX_RATING_PROVIDER__RATING_NAME__RATING_KIND ON RATING_PROVIDER(RATING_NAME, RATING_KIND);

CREATE TABLE IF NOT EXISTS TITLE_RATING
(
	TITLE_ID INTEGER NOT NULL CONSTRAINT FK_TITLE_RATING__TITLE_ID REFERENCES TITLE(TITLE_ID),
	RATING_ID INTEGER NOT NULL CONSTRAINT FK_TITLE_RATING__RATING_ID REFERENCES RATING_PROVIDER(RATING_ID),
	RATING_VALUE REAL NOT NULL,
	PRIMARY KEY(TITLE_ID, RATING_ID)
);
CREATE INDEX IF NOT EXISTS IX_TITLE_RATING__RATING_ID__RATING_VALUE ON TITLE_RATING(RATING_ID, RATING_VALUE);

INSERT INTO PROPERTY(PROPERTY_ID, PROPERTY_KIND, PROPERTY_NAME)
SELECT 1, 'S', 'Description'
WHERE not exists (select 1 from PROPERTY where PROPERTY_ID = 1);

INSERT INTO PROPERTY(PROPERTY_ID, PROPERTY_KIND, PROPERTY_NAME)
SELECT 2, 'I', 'Release Year'
WHERE not exists (select 2 from PROPERTY where PROPERTY_ID = 2);

INSERT INTO PROPERTY(PROPERTY_ID, PROPERTY_KIND, PROPERTY_NAME)
SELECT 3, 'S', 'IMDB ID'
WHERE not exists (select 2 from PROPERTY where PROPERTY_ID = 3);

INSERT INTO PROPERTY(PROPERTY_ID, PROPERTY_KIND, PROPERTY_NAME)
SELECT 4, 'I', 'Season'
WHERE not exists (select 2 from PROPERTY where PROPERTY_ID = 4);


INSERT INTO PROPERTY(PROPERTY_ID, PROPERTY_KIND, PROPERTY_NAME)
SELECT 5, 'I', 'Disk'
WHERE not exists (select 2 from PROPERTY where PROPERTY_ID = 5);


INSERT INTO PROPERTY(PROPERTY_ID, PROPERTY_KIND, PROPERTY_NAME)
SELECT 6, 'I', 'Episode'
WHERE not exists (select 2 from PROPERTY where PROPERTY_ID = 6);


