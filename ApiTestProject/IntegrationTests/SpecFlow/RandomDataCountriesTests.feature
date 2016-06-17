Feature: RandomDataCountriesTests
	In order for addresses to have associated countries
	As an EHIM user
	I want to be able to manage the country data in our system

Scenario: Create a country
	Given a valid country
	When POST is invoked on the countries api
	Then the country should be returned from the POST
	And it should contain a service generated id
	And it should have the correct created audit information
	And the country should be stored in the system

Scenario: Get a country
	Given a valid country exists in the system
	When GET is invoked on the countries api with the country's id
	Then all of the country information should be returned

#Scenario: Update a country
#	Given a valid country
#	When some of the countries properties are updated
#	And PUT is invoked on the countries api
#	Then the country should be returned
#	And the updated country should be stored in the system
#
#Scenario: Soft delete a country
#	Given the following countries exist in the data store
#	| Alpha2Code | Alpha3Code | EffectiveEndDate | EffectiveStartDate | FullName                     | Iso3166Code | PhoneNumberRegex                                      | PostalCodeRegex   | ShortName                |
#	| US         | USA        |                  | 7/4/1776           | the United States of America | 840         | ^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$ | ^\d{5}[ -]?\d{4}$ | United States Of America |
#	When delete is invoked on the countries api with the country's id
#	Then the country should exist in the data store
#	And the country's effective end date should be updated
#
#Scenario: Delete a country
#	Given the following countries exist in the data store
#	| Alpha2Code | Alpha3Code | EffectiveEndDate | EffectiveStartDate | FullName                     | Iso3166Code | PhoneNumberRegex                                      | PostalCodeRegex   | ShortName                |
#	| US         | USA        |                  | 7/4/1776           | the United States of America | 840         | ^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$ | ^\d{5}[ -]?\d{4}$ | United States Of America |
#	When force delete is invoked on the countries api with the country's id
#	Then the country should not exist in the data store