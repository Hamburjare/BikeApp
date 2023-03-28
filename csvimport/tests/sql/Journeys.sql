SET GLOBAL max_allowed_packet=524288000;

CREATE TABLE IF NOT EXISTS JourneysTest ( 
    Id INTEGER NOT NULL PRIMARY KEY AUTO_INCREMENT,
	DepartureTime DATETIME NOT NULL,
    ReturnTime DATETIME NOT NULL,
    DepartureStationId VARCHAR(255) NOT NULL,
    DepartureStationName VARCHAR(255) NOT NULL,
    ReturnStationId VARCHAR(255) NOT NULL,
    ReturnStationName VARCHAR(255) NOT NULL,
    CoveredDistance INT NOT NULL,
    Duration INT NOT NULL
);