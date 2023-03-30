Feature: SimpleDockerTests

Tests to prove docker working in a honeycomb envrionment 

@DataSeeder
Scenario Outline: AddressLine1_AddressLine2_City_ConcatonateUsersAddressLines_ConcatonateSuccess
	Given data is seeded with an id of <testDataId>
	When a request is made to '/Demo/Address?id=<testDataId>'
	Then a 200 is returned and concatonation should be a success 
	
	Examples:
			| testDataId |
			| 1          |
