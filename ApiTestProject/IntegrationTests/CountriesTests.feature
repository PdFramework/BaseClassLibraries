@Countries
Feature: CountriesTests
	In order for addresses to have associated countries
	As an EHIM user
	I want to be able to manage the country data in our system

Scenario: Create a country
	Given a valid country
	When POST is invoked on the countries' api
	Then the country should be returned from the POST
	And it should contain a service generated id
	And it should have the correct created audit information
	And the Created country should be stored in the system

Scenario: Get a country
	Given a valid country exists in the system
	When GET is invoked on the countries' api with the country's id
	Then the country should be returned

Scenario: Update a country
	Given a valid country exists in the system
	When the country is updated
	And PUT is invoked on the countries' api
	Then the country should be returned from the PUT
	And the Updated country should have the correct updated audit information
	And the Updated country should be stored in the system

Scenario: Soft delete a country
	Given a valid country exists in the system
	When DELETE is invoked on the countries' api with the country's id
	Then the Deleted country should be stored in the system
	And the Deleted country should have the correct updated audit information
	And the country's effective end date should be updated

Scenario: Delete a country
	Given a valid country exists in the system
	When force DELETE is invoked on the countries' api with the country's id
	Then the country should not be stored in the system