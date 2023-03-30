Feature: SimpleDockerTests

Tests to prove docker working in a honeycomb envrionment using BDD

As a consumer of the Address endpoint
I want to know a users Address is concatonated

@DataSeeder
Scenario Outline: Concatonate Address Is A Success 3 Address Lines
	Given we have a user in our database: 
	| Field     | Value             |
	| PersonId  | <testDataId>      |
	| LastName  | Test              |
	| FirstName | Sarah             |
	| Address1  | 89 Not An Address |
	| Address2  | <Address2>        |
	| City      | DingleBerry       |
	When a request is made to '/Demo/Address?id=<testDataId>'
	Then a 200 result is returned
	And address1, address2, and city are in a single line
	
	Examples:
			| testDataId | Address2     | 
			| 1          | AddressLine2 |    
			
@DataSeeder
Scenario Outline: Concatonate Address Is A Success, Address Line 2 null
	Given we have a user in our database: 
	| Field     | Value             |
	| PersonId  | <testDataId>      |
	| LastName  | Test              |
	| FirstName | Sarah             |
	| Address1  | 89 Not An Address |
	| City      | DingleBerry       |
	When a request is made to '/Demo/Address?id=<testDataId>'
	Then a 200 result is returned
	And address1, and city are in a single line
	
	Examples:
			| testDataId |
			| 1          |

@DataSeeder
Scenario Outline: Concatonate Address Is A Success, Address Line 2 Empty
	Given we have a user in our database: 
	| Field     | Value             |
	| PersonId  | <testDataId>      |
	| LastName  | Test              |
	| FirstName | Sarah             |
	| Address1  | 89 Not An Address |
	| Address2  |                   |
	| City      | DingleBerry       |
	When a request is made to '/Demo/Address?id=<testDataId>'
	Then a 200 result is returned
	And address1, and city are in a single line
	
	Examples:
			| testDataId |
			| 1          |

@DataSeeder
Scenario Outline: More than one result returned from the database returns a failure
	Given we have the same user in our database <timesSeeded> times: 
	| Field     | Value             |
	| PersonId  | <testDataId>      |
	| LastName  | Test              |
	| FirstName | Sarah             |
	| Address1  | 89 Not An Address |
	| Address2  | Line 2            |
	| City      | DingleBerry       |
	Then a http exception is thrown when '/Demo/Address?id=<testDataId>' is called
	
	Examples:
			| testDataId | timesSeeded |
			| 1          | 2           |

@DataSeeder
Scenario Outline: No result from the database returns not found
	Given we have a user in our database:
	| Field     | Value             |
	| PersonId  | <testDataId>      |
	| LastName  | Test              |
	| FirstName | Sarah             |
	| Address1  | 89 Not An Address |
	| Address2  | Line 2            |
	| City      | DingleBerry       |
	When a request is made to '/Demo/Address?id=<testDataIdToFind>'
	Then a 404 result is returned
	
	Examples:
			| testDataId | testDataIdToFind |
			| 1          | 2                |